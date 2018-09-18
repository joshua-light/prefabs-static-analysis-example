using UnityEngine;

namespace Editor.Diagnostics.Core
{
  public interface IDiagnostic
  {
    DiagnosticId Id { get; }
    DiagnosticSeverity Severity { get; }

    void Draw(Rect rect);
  }
}