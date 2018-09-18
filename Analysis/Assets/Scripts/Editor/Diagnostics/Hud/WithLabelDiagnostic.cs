using System.Collections.Generic;
using Editor.Diagnostics.Core;
using UnityEngine;

namespace Editor.Diagnostics.Hud
{
  public class WithLabelDiagnostic : IDiagnostic
  {
    private static class Style
    {
      private static GUIStyle _style;

      public static GUIStyle Cached()
      {
        if (_style != null)
          return _style;

        return _style = new GUIStyle(GUI.skin.box)
        {
          fontSize = 7,
          fontStyle = FontStyle.Bold,
          padding = new RectOffset(4, 4, 0, 0),
          alignment = TextAnchor.MiddleCenter,
          fixedWidth = 60,
          wordWrap = false,
          normal = { textColor = new Color(0.78f, 0.78f, 0.78f) }
        };
      }
    }

    private static class Content
    {
      private static readonly Dictionary<string, GUIContent>
        Cache = new Dictionary<string, GUIContent>();

      public static GUIContent Of(string text)
      {
        GUIContent content;
        if (!Cache.TryGetValue(text, out content))
          Cache[text] = content = new GUIContent(text);

        return content;
      }
    }

    private const int MaxWidth = 60;
    private const int RightOffset = 23;

    private readonly IDiagnostic _inner;
    private readonly string _text;

    public WithLabelDiagnostic(IDiagnostic inner, string text)
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

      rect.x += rect.width - MaxWidth - RightOffset;
      rect.width = MaxWidth;

      GUI.Box(rect, Content.Of(_text), Style.Cached());
    }
  }
}