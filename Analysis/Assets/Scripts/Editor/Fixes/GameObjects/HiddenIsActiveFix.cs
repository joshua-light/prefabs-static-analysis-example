using Editor.Diagnostics.Core;
using Editor.Fixes.Core;
using UnityEngine;

namespace Editor.Fixes.GameObjects
{
  public class HiddenIsActiveFix : GameObjectFix
  {
    public override DiagnosticId DiagnosticId => DiagnosticId.HiddenIsActive;

    protected override string Describe(GameObject gameObject) =>
      "Make `" + gameObject.name + "` inactive.";

    protected override void Execute(GameObject gameObject) =>
      gameObject.SetActive(false);
  }
}