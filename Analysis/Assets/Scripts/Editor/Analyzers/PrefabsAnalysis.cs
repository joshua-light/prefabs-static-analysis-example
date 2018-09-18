using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.Analyzers.Core;
using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Analyzers
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

    private static readonly List<IDiagnostic> DiagnosticsBuffer = new List<IDiagnostic>();
    private static readonly List<Component> ComponentsBuffer = new List<Component>();

    #region Initialization

    public static void Initialize()
    {
      var types = Assembly.GetExecutingAssembly().GetTypes().ToArray();

      _gameObjectAnalyzers = GetAnalyzers<GameObjectAnalyzer>(types);
      _componentAnalyzers = GetAnalyzers<ComponentAnalyzer>(types);
    }

    private static T[] GetAnalyzers<T>(IEnumerable<Type> types) =>
      types
        .Where(x => typeof (T).IsAssignableFrom(x) && x != typeof (T) && !x.IsAbstract)
        .Select(Activator.CreateInstance)
        .Cast<T>()
        .ToArray();

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
      
      DiagnosticsBuffer.Clear();

      diagnostic = Execute(instance, DiagnosticsBuffer);
      Session.Add(id, diagnostic);

      return diagnostic;
    }

    private static IDiagnostic Execute(GameObject instance, List<IDiagnostic> diagnostics)
    {
      var result = Diagnostic.None;

      GatherDiagnostics(instance, diagnostics);
      foreach (var diagnostic in diagnostics)
      {
        if (!diagnostic.Severity.IsGreaterThen(result.Severity))
          continue;

        result = diagnostic;

        if (result.Severity.IsHighestLevel())
          return result;
      }

      return result;
    }

    public static void GatherDiagnostics(GameObject instance, List<IDiagnostic> diagnostics)
    {
      instance.GetComponents(ComponentsBuffer);

      var components = ComponentsBuffer;
      for (int i = 0, iLength = components.Count; i < iLength; i++)
        GatherDiagnostics(components[i], diagnostics);

      var analyzers = _gameObjectAnalyzers;
      for (int i = 0, iLength = analyzers.Length; i < iLength; i++)
      {
        var diagnostic = analyzers[i].Execute(instance);
        if (diagnostic != Diagnostic.None)
          diagnostics.Add(diagnostic);
      }
    }

    private static void GatherDiagnostics(Component component, List<IDiagnostic> diagnostics)
    {
      var analyzers = _componentAnalyzers;
      for (int i = 0, length = analyzers.Length; i < length; i++)
      {
        var diagnostic = analyzers[i].Execute(component);
        if (diagnostic != Diagnostic.None)
          diagnostics.Add(diagnostic);
      }
    }
  }
}