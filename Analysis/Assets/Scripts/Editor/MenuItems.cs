using System.Linq;
using Editor.Analyzers;
using Editor.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
  public static class MenuItems
  {
    [MenuItem("Analysis/Run Benchmark")]
    public static void Benchmark()
    {
      var gameObjects = SceneManager
        .GetActiveScene()
        .GetRootGameObjects()
        .SelectMany(x => x.RootWithChildren())
        .ToList();
      
      PrefabsAnalysis.Reset();
      
      using (SimpleWatch.New(ms => Debug.Log($"Analysis finished in {ms} ms.")))
        foreach (var gameObject in gameObjects)
          PrefabsAnalysis.Execute(gameObject);
    }
  }
}