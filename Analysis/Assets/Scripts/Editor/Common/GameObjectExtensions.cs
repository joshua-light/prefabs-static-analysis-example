using Boo.Lang;
using UnityEngine;

namespace Editor.Common
{
  public static class GameObjectExtensions
  {
    private static readonly List<GameObject> GameObjectsBuffer = new List<GameObject>(10);
    
    // The result is read-only!
    public static List<GameObject> RootWithChildren(this GameObject self)
    {
      var result = GameObjectsBuffer;
      
      result.Clear();
      result.Add(self);
      
      GetRootWithChildrenRecursively(self, result);

      return result;
    }
    
    // The result is read-only!
    public static List<GameObject> Children(this GameObject self)
    {
      var result = GameObjectsBuffer;
      
      result.Clear();

      GetRootWithChildrenRecursively(self, result);

      return result;
    }

    private static void GetRootWithChildrenRecursively(GameObject root, List<GameObject> pool)
    {
      var transform = root.transform;

      for (int i = 0, iLength = transform.childCount; i < iLength; i++)
      {
        var child = transform.GetChild(i).gameObject;

        pool.Add(child);
        GetRootWithChildrenRecursively(child, pool);
      }
    }
  }
}