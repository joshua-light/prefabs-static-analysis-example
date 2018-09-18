using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Analyzers.Core
{
  public abstract class ComponentAnalyzer : IAnalyzer
  {
    public abstract DiagnosticId DiagnosticId { get; }

    public IDiagnostic Execute(Object context)
    {
      var component = context as Component;
      var diagnostic = Execute(component);
      if (diagnostic != Diagnostic.None)
        diagnostic = diagnostic
          .Identified(DiagnosticId)
          .WithComponent(component);

      return diagnostic;
    }

    public abstract IDiagnostic Execute(Component component);
  }
  
  public abstract class ComponentAnalyzer<T> : ComponentAnalyzer where T : Component
  {
    public override IDiagnostic Execute(Component component)
    {
      var castedComponent = component as T;
      if (castedComponent == null)
        return Diagnostic.None;

      var diagnostic = Execute(castedComponent);
      if (diagnostic != Diagnostic.None)
        diagnostic = diagnostic
          .Identified(DiagnosticId)
          .WithComponent(castedComponent);

      return diagnostic;
    }

    public abstract IDiagnostic Execute(T component);
  }
}