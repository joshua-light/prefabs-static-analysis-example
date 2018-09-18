using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Diagnostics
{
  public class WithComponentDiagnostic : IDiagnostic
  {
    private readonly IDiagnostic _inner;

    public WithComponentDiagnostic(IDiagnostic inner, Component component)
    {
      Component = component;
      
      _inner = inner;
    }
    
    public Component Component { get; }

    public DiagnosticId Id => _inner.Id;
    public DiagnosticSeverity Severity => _inner.Severity;
    public void Draw(Rect rect) => _inner.Draw(rect);
  }
}