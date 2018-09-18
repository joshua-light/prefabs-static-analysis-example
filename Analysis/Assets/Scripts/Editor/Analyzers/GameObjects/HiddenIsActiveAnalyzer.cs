using Editor.Analyzers.Core;
using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Analyzers.GameObjects
{
  public class HiddenIsActiveAnalyzer : GameObjectAnalyzer
  {
    public override DiagnosticId DiagnosticId => DiagnosticId.HiddenIsActive;
    
    public override IDiagnostic Execute(GameObject gameObject)
    {
      var name = gameObject.name;
      if (name.Length < 2)
        return Diagnostic.None;

      var isIncorrect = name[name.Length - 1] == 'h' &&
                        name[name.Length - 2] == '_' &&
                        gameObject.activeInHierarchy;
      if (!isIncorrect)
        return Diagnostic.None;

      return Diagnostic.Hint
        .WithTooltip("Remove _h from active object or make it inactive.")
        .WithLabel("_H");
    }
  }
}