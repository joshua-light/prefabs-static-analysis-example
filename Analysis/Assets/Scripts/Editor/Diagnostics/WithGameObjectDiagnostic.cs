using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Diagnostics
{
  public class WithGameObjectDiagnostic : IDiagnostic
  {
    private readonly IDiagnostic _inner;

    public WithGameObjectDiagnostic(IDiagnostic inner, GameObject gameObject)
    {
      GameObject = gameObject;
      
      _inner = inner;
    }
    
    public GameObject GameObject { get; }

    public DiagnosticId Id => _inner.Id;
    public DiagnosticSeverity Severity => _inner.Severity;
    public void Draw(Rect rect) => _inner.Draw(rect);
  }
}