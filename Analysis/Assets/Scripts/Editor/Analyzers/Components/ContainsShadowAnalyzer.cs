using Editor.Analyzers.Core;
using Editor.Diagnostics.Core;
using UnityEngine.UI;

namespace Editor.Analyzers.Components
{
  public class ContainsShadowAnalyzer : ComponentAnalyzer<Shadow>
  {
    public override DiagnosticId DiagnosticId => DiagnosticId.ContainsShadow;
    
    public override IDiagnostic Execute(Shadow component)
    {
      // If we get there, then game object already contains `Shadow`.
      return Diagnostic.Warning
        .WithTooltip("`Shadow` component is creates redundant copies of objects in memory.")
        .WithLabel("SHADOW");
    }
  }
}