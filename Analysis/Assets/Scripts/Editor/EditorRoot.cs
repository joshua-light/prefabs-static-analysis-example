using Editor.Services;
using UnityEditor;

namespace Editor
{
  [InitializeOnLoad]
  public static class EditorRoot
  {
    static EditorRoot()
    {
      if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
        return;

      EditorApplication.playModeStateChanged += InitializeIfEditor;
      
      InitializeIfEditor(PlayModeStateChange.EnteredEditMode);
    }

    private static void InitializeIfEditor(PlayModeStateChange e)
    {
      if (e == PlayModeStateChange.EnteredEditMode)
        Initialize();
    }

    private static void Initialize()
    {
      PrefabsAnalysis.Initialize();
      PrefabsAnalysisService.Initialize();
    }
  }
}