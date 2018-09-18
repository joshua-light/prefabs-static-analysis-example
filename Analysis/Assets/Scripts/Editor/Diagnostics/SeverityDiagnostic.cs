using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Diagnostics
{
  public class SeverityDiagnostic : IDiagnostic
  {
    public SeverityDiagnostic(DiagnosticSeverity severity)
    {
      Severity = severity;
    }
    
    public DiagnosticSeverity Severity { get; }

    public DiagnosticId Id => DiagnosticId.None;
    public void Draw(Rect rect) { }
  }
}