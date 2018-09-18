using System.Collections.Generic;
using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Diagnostics.Hud
{
  public class WithTooltipDiagnostic : IDiagnostic
  {
    private static class Content
    {
      private static readonly Dictionary<string, GUIContent> Cache = new Dictionary<string, GUIContent>(); 
 
      public static GUIContent Of(string description)
      {
        GUIContent content;
        if (!Cache.TryGetValue(description, out content))
          Cache[description] = content = new GUIContent(string.Empty, description);

        return content;
      }
    }
    
    private readonly IDiagnostic _inner;
    private readonly string _text;

    public WithTooltipDiagnostic(IDiagnostic inner, string text)
    {
      _inner = inner;
      _text = text;
    }
    
    public DiagnosticSeverity Severity => _inner.Severity;
    public DiagnosticId Id => _inner.Id;
    
    public void Draw(Rect rect)
    {
      _inner.Draw(rect);
      
      rect.y += 1;
      rect.height = rect.height - 2;

      var width = rect.height;
      rect.x += rect.width - width - 4;
      rect.width = width;

      var content = Content.Of(_text);
      GUI.Box(rect, content, GUIStyle.none);
    }
  }
}