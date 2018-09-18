using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Analyzers.Core
{
  public abstract class GameObjectAnalyzer : IAnalyzer
  {
    public abstract DiagnosticId DiagnosticId { get; }

    public IDiagnostic Execute(Object context)
    {
      var gameObject = context as GameObject;
      var diagnostic = Execute(gameObject);
      if (diagnostic != Diagnostic.None)
        diagnostic = diagnostic
          .Identified(DiagnosticId)
          .WithGameObject(gameObject);

      return diagnostic;
    }

    public abstract IDiagnostic Execute(GameObject gameObject);
  }
}