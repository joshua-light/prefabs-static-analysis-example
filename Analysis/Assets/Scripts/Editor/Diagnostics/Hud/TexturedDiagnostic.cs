using System;
using Editor.Diagnostics.Core;
using UnityEditor;
using UnityEngine;

namespace Editor.Diagnostics.Hud
{
  public class TexturedDiagnostic : IDiagnostic
  {
    private readonly IDiagnostic _inner;
    private readonly string _path;
    private readonly float _alpha;

    public TexturedDiagnostic(IDiagnostic inner, string path, float alpha = 1.0f)
    {
      _inner = inner;
      _path = path;
      _alpha = alpha;
    }

    public DiagnosticSeverity Severity => _inner.Severity;
    public DiagnosticId Id => _inner.Id;

    public void Draw(Rect rect)
    {
      _inner.Draw(rect);

      var texture = AssetDatabase.LoadAssetAtPath<Texture>(_path);
      var aspect = (float) texture.width / texture.height;

      rect.y += 1;
      rect.height = rect.height - 2;

      var width = rect.height * aspect;
      rect.x += rect.width - width - 4 - Math.Max(0, (15 - width) / 2);
      rect.width = width;

      var color = GUI.color;
      GUI.color = new Color(color.r, color.g, color.b, _alpha);

      GUI.DrawTexture(rect, texture, ScaleMode.StretchToFill, true, aspect);

      GUI.color = new Color(color.r, color.g, color.b, 1.0f);
    }
  }
}