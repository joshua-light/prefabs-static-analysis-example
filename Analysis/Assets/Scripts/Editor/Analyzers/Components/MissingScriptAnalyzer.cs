using Editor.Analyzers.Core;
using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Analyzers.Components
{
  public class MissingScriptAnalyzer : ComponentAnalyzer
  {
    public override DiagnosticId DiagnosticId => DiagnosticId.ScriptIsMissing;
    
    public override IDiagnostic Execute(Component component)
    {
      // Here we optimize `==` overloading by `UnityEngine.Object`.
      return ReferenceEquals(component, null)
        ? Diagnostic.Error
          .WithTooltip("There is probably a missing script in game object.")
          .WithLabel("MISSING")
        : Diagnostic.None;
    }
  }
}