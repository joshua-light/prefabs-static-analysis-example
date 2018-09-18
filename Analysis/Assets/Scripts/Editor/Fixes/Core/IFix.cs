using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Fixes.Core
{
  public interface IFix
  {
    DiagnosticId DiagnosticId { get; }

    string Describe(Object context);
    void Execute(Object context);
  }
}