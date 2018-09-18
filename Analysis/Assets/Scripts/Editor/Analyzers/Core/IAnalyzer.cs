using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Analyzers.Core
{
  public interface IAnalyzer
  {
    DiagnosticId DiagnosticId { get; }

    IDiagnostic Execute(Object context);
  }
}