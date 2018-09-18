using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.Diagnostics.Core;
using Editor.Fixes.Core;

namespace Editor.Fixes
{
  public static class PrefabsFixes
  {
    private static Dictionary<DiagnosticId, IFix[]> FixesByDiagnostic;

    public static void Initialize()
    {
      var types = Assembly.GetExecutingAssembly().GetTypes();

      var gameObjectFixes = GetFixes<GameObjectFix>(types);
      var componentFixes = GetFixes<ComponentFix>(types);

      FixesByDiagnostic = gameObjectFixes
        .Concat(componentFixes)
        .GroupBy(x => x.DiagnosticId)
        .ToDictionary(x => x.Key, x => x.ToArray());
    }

    private static IEnumerable<IFix> GetFixes<T>(Type[] types)
    {
      return types
        .Where(x => typeof (T).IsAssignableFrom(x) && x != typeof (T) && !x.IsAbstract)
        .Select(Activator.CreateInstance)
        .Cast<IFix>();
    }

    public static IEnumerable<IFix> Fixes(IDiagnostic diagnostic)
    {
      if (diagnostic == null)
        return Enumerable.Empty<IFix>();

      IFix[] fixes;
      if (!FixesByDiagnostic.TryGetValue(diagnostic.Id, out fixes))
        return Enumerable.Empty<IFix>();

      return fixes;
    }
  }
}