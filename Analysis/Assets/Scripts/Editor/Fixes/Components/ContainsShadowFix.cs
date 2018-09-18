using Editor.Diagnostics.Core;
using Editor.Fixes.Core;
using UnityEditor;
using UnityEngine.UI;

namespace Editor.Fixes.Components
{
  public class ContainsShadowFix : ComponentFix<Shadow>
  {
    public override DiagnosticId DiagnosticId => DiagnosticId.ContainsShadow;

    protected override string Describe(Shadow shadow) =>
      "Remove `Shadow`.";

    protected override void Execute(Shadow shadow) =>
      Undo.DestroyObjectImmediate(shadow);
  }
}