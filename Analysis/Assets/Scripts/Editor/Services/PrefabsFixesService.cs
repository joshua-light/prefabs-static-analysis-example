using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Editor.Analyzers;
using Editor.Diagnostics.Core;
using Editor.Fixes;
using UnityEditor;
using UnityEngine;

namespace Editor.Services
{
  public class PrefabsFixesService
  {
    private static readonly TimeSpan UpdateDelay = TimeSpan.FromMilliseconds(250);
    
    private static DateTime _updateTime = DateTime.Now;
    
    public static void Initialize()
    {
      EditorApplication.update += OnUpdate;
    }

    private static void OnUpdate()
    {
      if (DateTime.Now - _updateTime < UpdateDelay)
        return;

#if UNITY_EDITOR_WIN
      // Here we check active window pointer, that is related to the current thread messages queue.
      // So it's probably equal to zero if we don't focused Unity.
      if (GetActiveWindow() == IntPtr.Zero)
        return;

      // user32.dll function GetKeyboardState in result byte array do not distinguish right- and left-controls.
      // So even if real pressed keys count is 2 (LControl + Return), the actual number will be 3 (2 + Control).
      if (!IsPressed(VirtualKey.Control, VirtualKey.LControl, VirtualKey.Return))
        return;

      var gameObject = Selection.activeGameObject;
      if (gameObject == null)
        return;

      var diagnostic = PrefabsAnalysis.Execute(gameObject);
      if (diagnostic == Diagnostic.None)
        return;

      _updateTime = DateTime.Now;

      ApplyFix(gameObject);
#endif
    }

    private static void ApplyFix(GameObject gameObject)
    {
      var diagnostics = PrefabsAnalysis.ExecuteOn(gameObject);
      foreach (var diagnostic in diagnostics)
        ApplyFix(diagnostic);

      EditorUtility.SetDirty(gameObject);
    }

    private static void ApplyFix(IDiagnostic diagnostic)
    {
      var fix = PrefabsFixes.Fixes(diagnostic).FirstOrDefault();
      if (fix == null)
        return;

      var context = diagnostic.Context();

      Debug.Log("Applying fix with description: " + fix.Describe(context));
      fix.Execute(context);
    }

    #region User32 Helpers

    private enum VirtualKey
    {
      Return = 0x0D,
      Control = 0x11,
      LControl = 0xA2,
    }

    private const int KeyPressed = 0x80;

    private static readonly byte[] _keysBuffer = new byte[256];
    private static object _topLevel;
    private static FieldInfo _rectField;
    private static PropertyInfo _field;

    private static bool IsPressed(params VirtualKey[] keys)
    {
      GetKeyboardState(_keysBuffer);
      _keysBuffer[0x0] = 0; // Reset the unused virtual key.

      var pressedKeysCount = _keysBuffer.Count(x => (x & KeyPressed) != 0);
      if (pressedKeysCount != keys.Length)
        return false;

      return keys.All(x => (_keysBuffer[(int)x] & KeyPressed) != 0);
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetKeyboardState(byte[] lpKeyState);

    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    #endregion
  }
}