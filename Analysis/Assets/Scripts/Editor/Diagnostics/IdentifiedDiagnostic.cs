using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Diagnostics
{
  public class IdentifiedDiagnostic : IDiagnostic
  {
    private readonly IDiagnostic _inner;

    public IdentifiedDiagnostic(IDiagnostic inner, DiagnosticId id)
    {
      Id = id;
      
      _inner = inner;
    }
    
    public DiagnosticId Id { get; }
    
    public DiagnosticSeverity Severity => _inner.Severity;
    public void Draw(Rect rect) => _inner.Draw(rect);
  }
}