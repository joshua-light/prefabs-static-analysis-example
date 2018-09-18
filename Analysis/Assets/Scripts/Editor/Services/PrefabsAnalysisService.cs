using System;
using Editor.Analyzers;
using Editor.Common;
using Editor.Diagnostics.Core;
using UnityEditor;
using UnityEngine;

namespace Editor.Services
{
  public static class PrefabsAnalysisService
  {
    private static readonly TimeSpan ResetDelay = TimeSpan.FromMilliseconds(250);

    private static DateTime _resetTime = DateTime.Now;
    private static bool _isEnabled;

    public static void Initialize() => Enable();

    public static void Enable()
    {
      if (_isEnabled)
        return;

      PrefabsAnalysis.Reset();

      EditorApplication.update += OnUpdate;

      EditorApplication.RepaintHierarchyWindow();
      EditorApplication.hierarchyWindowItemOnGUI += OnItemGUI;
      EditorApplication.hierarchyChanged += OnHierarchyWindowChange;

      Selection.selectionChanged += OnSelectionChange;
      
      _isEnabled = true;
    }

    public static void Disable()
    {
      if (!_isEnabled)
        return;

      Selection.selectionChanged -= OnSelectionChange;
      EditorApplication.update -= OnUpdate;

      EditorApplication.hierarchyWindowItemOnGUI -= OnItemGUI;
      EditorApplication.hierarchyChanged -= OnHierarchyWindowChange;
      EditorApplication.RepaintHierarchyWindow();

      _isEnabled = false;
    }

    private static void OnHierarchyWindowChange()
    {
      // EditorApplication.hierarchyWindowChanged is called two times in a row after CTRL + D.
      if (DateTime.Now - _resetTime < ResetDelay)
        return;

      _resetTime = DateTime.Now;

      PrefabsAnalysis.Reset();
    }

    private static void OnSelectionChange()
    {
      var active = Selection.activeGameObject;
      if (active != null)
        OnUpdate();
    }

    private static IDiagnostic _diagnostic = Diagnostic.None;

    private static void OnUpdate()
    {
      var active = Selection.activeGameObject;
      if (active == null)
        return;

      PrefabsAnalysis.Reset(active);

      var diagnostic = PrefabsAnalysis.Execute(active);
      if (diagnostic.Id != _diagnostic.Id)
      {
        EditorUtility.SetDirty(active);
        _diagnostic = diagnostic;
      }
    }

    private static void OnItemGUI(int instanceID, Rect rect)
    {
      if (!_isEnabled)
        return;

      var instance = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
      if (instance == null)
        return;

      var diagnostic = PrefabsAnalysis.Execute(instance);
      if (diagnostic.Severity != DiagnosticSeverity.None)
      {
        diagnostic.Draw(rect);
        return;
      }

      var children = instance.Children();
      for (var i = 0; i < children.Count; i++)
      {
        var childDiagnostic = PrefabsAnalysis.Execute(children[i]);
        if (childDiagnostic.Severity == DiagnosticSeverity.None)
          continue;

        if (childDiagnostic.Severity > diagnostic.Severity)
          diagnostic = childDiagnostic;
      }

      diagnostic.AsTransparent().Draw(rect);
    }
  }
}