using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.Analyzers.Core;
using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor
{
  public class PrefabsAnalysis
  {
    private class AnalysisSession
    {
      private readonly Dictionary<int, IDiagnostic> _diagnosticsById = new Dictionary<int, IDiagnostic>();

      public IDiagnostic Diagnostic(int instanceId)
      {
        IDiagnostic diagnostic;
        return _diagnosticsById.TryGetValue(instanceId, out diagnostic)
          ? diagnostic
          : null;
      }
      
      public void Add(int instanceId, IDiagnostic diagnostic) =>
        _diagnosticsById[instanceId] = diagnostic;

      public void Clear() => _diagnosticsById.Clear();
      public void Clear(int instanceId) => _diagnosticsById[instanceId] = null;
    }
    
    private static IAnalyzer[] _gameObjectAnalyzers;
    private static IAnalyzer[] _componentAnalyzers;

    private static readonly AnalysisSession Session = new AnalysisSession();

    private static readonly List<Component> ComponentsBuffer = new List<Component>();

    #region Initialization

    public static void Initialize()
    {
      var types = Assembly.GetExecutingAssembly().GetTypes().ToArray();

      _gameObjectAnalyzers = GetAnalyzers<GameObjectAnalyzer>(types).ToArray();
      _componentAnalyzers = GetAnalyzers<ComponentAnalyzer>(types).ToArray();
    }

    public static void Refresh()
    {
      Initialize();
      
      Session.Clear();
    }

    private static T[] GetAnalyzers<T>(IEnumerable<Type> types) =>
      types
        .Where(x => typeof (T).IsAssignableFrom(x) && x != typeof (T) && !x.IsAbstract)
        .OrderByDescending(AnalysisPriority)
        .Select(Activator.CreateInstance)
        .Cast<T>()
        .ToArray();

    private static int AnalysisPriority(Type type)
    {
      // There can be implemented some kind of priority sorting.
      return 0;
    }

    #endregion

    public static void Reset(GameObject instance)
    {
      Session.Clear(instance.GetHashCode());
    }

    public static void Reset()
    {
      Session.Clear();
    }

    public static IDiagnostic Execute(GameObject instance)
    {
      // Same as `GetInstanceId()`, but without checking current thread.
      var id = instance.GetHashCode();
      var diagnostic = Session.Diagnostic(id);
      if (diagnostic != null)
        return diagnostic;

      diagnostic = ExecuteImpl(instance);
      Session.Add(id, diagnostic);

      return diagnostic;
    }

    private static IDiagnostic ExecuteImpl(GameObject instance)
    {
      var result = Diagnostic.None;

      foreach (var diagnostic in ExecuteOn(instance))
      {
        if (!diagnostic.Severity.IsGreaterThen(result.Severity))
          continue;

        result = diagnostic;

        if (result.Severity.IsHighestLevel())
          return result;
      }

      return result;
    }

    private static IEnumerable<IDiagnostic> ExecuteOn(GameObject instance)
    {
      instance.GetComponents(ComponentsBuffer);

      var components = ComponentsBuffer;
      for (int i = 0, iLength = components.Count; i < iLength; i++)
      {
        foreach (var diagnostic in ExecuteOn(components[i]))
          yield return diagnostic;
      }

      var analyzers = _gameObjectAnalyzers;
      for (int i = 0, iLength = analyzers.Length; i < iLength; i++)
      {
        var diagnostic = analyzers[i].Execute(instance);
        if (diagnostic != Diagnostic.None)
          yield return diagnostic;
      }
    }

    private static IEnumerable<IDiagnostic> ExecuteOn(Component component)
    {
      var analyzers = _componentAnalyzers;
      for (int i = 0, length = analyzers.Length; i < length; i++)
      {
        var diagnostic = analyzers[i].Execute(component);
        if (diagnostic != Diagnostic.None)
          yield return diagnostic;
      }
    }
  }
}