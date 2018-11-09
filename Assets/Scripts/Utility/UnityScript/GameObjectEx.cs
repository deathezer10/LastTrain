using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectEx
{
    /// <summary>
    /// 指定されたコンポーネントがアタッチされているかどうかを返す
    /// </summary>
    public static bool HasComponent<T>(this GameObject self) where T : Component
    {
        return self.GetComponent<T>() != null;
    }

    public static T SafeAddComponent<T>(this GameObject self) where T : Component
    {
		var component = self.GetComponent<T>();

        return component ? component : self.AddComponent<T>();
    }
}
