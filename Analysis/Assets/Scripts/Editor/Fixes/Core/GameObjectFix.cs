using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Fixes.Core
{
  public abstract class GameObjectFix : IFix
  {
    public abstract DiagnosticId DiagnosticId { get; }

    public string Describe(Object context) => Describe((GameObject)context);
    protected abstract string Describe(GameObject gameObject);

    public void Execute(Object context) => Execute((GameObject)context);
    protected abstract void Execute(GameObject gameObject);
  }
}