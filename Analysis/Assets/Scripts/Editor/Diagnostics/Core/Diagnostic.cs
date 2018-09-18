using Editor.Diagnostics.Hud;
using UnityEngine;

namespace Editor.Diagnostics.Core
{
  public static class Diagnostic
  {
    private class NoneDiagnostic : IDiagnostic
    {
      public DiagnosticId Id => DiagnosticId.None;
      public DiagnosticSeverity Severity => DiagnosticSeverity.None;

      public void Draw(Rect rect) { }
    }
    
    public static readonly IDiagnostic None = new NoneDiagnostic();
    
    public static readonly IDiagnostic Hint = new SeverityDiagnostic(DiagnosticSeverity.Hint)
      .WithTexture("Assets/Resources/Editor/hint.png");
    public static readonly IDiagnostic HintTransparent = new SeverityDiagnostic(DiagnosticSeverity.Hint)
      .WithTexture("Assets/Resources/Editor/hint.png", alpha: 0.3f);
    
    public static readonly IDiagnostic Warning = new SeverityDiagnostic(DiagnosticSeverity.Warning)
      .WithTexture("Assets/Resources/Editor/warning.png");
    public static readonly IDiagnostic WarningTransparent = new SeverityDiagnostic(DiagnosticSeverity.Warning)
      .WithTexture("Assets/Resources/Editor/warning.png", alpha: 0.3f);
    
    public static readonly IDiagnostic Error = new SeverityDiagnostic(DiagnosticSeverity.Error)
      .WithTexture("Assets/Resources/Editor/error.png");
    public static readonly IDiagnostic ErrorTransparent = new SeverityDiagnostic(DiagnosticSeverity.Error)
      .WithTexture("Assets/Resources/Editor/error.png", alpha: 0.3f);

    public static IDiagnostic WithTexture(this IDiagnostic self, string path, float alpha = 1.0f) =>
      new TexturedDiagnostic(self, path, alpha);
    
    public static IDiagnostic Identified(this IDiagnostic self, DiagnosticId id) =>
      new IdentifiedDiagnostic(self, id);
    
    public static IDiagnostic WithTooltip(this IDiagnostic self, string text) =>
      new WithTooltipDiagnostic(self, text);
    
    public static IDiagnostic WithLabel(this IDiagnostic self, string text) =>
      new WithLabelDiagnostic(self, text);

    public static IDiagnostic WithComponent(this IDiagnostic self, Component component) =>
      new WithComponentDiagnostic(self, component);
    
    public static IDiagnostic WithGameObject(this IDiagnostic self, GameObject gameObject) =>
      new WithGameObjectDiagnostic(self, gameObject);

    public static IDiagnostic AsTransparent(this IDiagnostic self)
    {
      switch (self.Severity)
      {
        case DiagnosticSeverity.Hint: return HintTransparent;
        case DiagnosticSeverity.Warning: return WarningTransparent;
        case DiagnosticSeverity.Error: return ErrorTransparent;
        
        default: return None;
      }
    }
    
    public static Object Context(this IDiagnostic self)
    {
      var gameObjectDiagnostic = self as WithGameObjectDiagnostic;
      if (gameObjectDiagnostic != null)
        return gameObjectDiagnostic.GameObject;

      var componentDiagnostic = self as WithComponentDiagnostic;
      if (componentDiagnostic != null)
        return componentDiagnostic.Component;

      return null;
    }
  }
}