using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Fixes.Core
{
  public abstract class ComponentFix : IFix
  {
    public abstract DiagnosticId DiagnosticId { get; }

    public string Describe(Object context) => Describe((Component)context);
    protected abstract string Describe(Component component);

    public void Execute(Object context) => Execute((Component)context);
    protected abstract void Execute(Component component);
  }

  public abstract class ComponentFix<T> : ComponentFix where T : Component
  {
    protected override string Describe(Component component) => Describe((T) component);
    protected abstract string Describe(T component);
    
    protected override void Execute(Component component) => Execute((T) component);
    protected abstract void Execute(T component);
  }
}