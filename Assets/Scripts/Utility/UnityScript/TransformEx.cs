using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class TransformEx {
    /// <summary>
    /// 全ての子を取得
    /// /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static GameObject[] GetAllChild(this Transform transform)
    {
        var list = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            list.Add(transform.GetChild(i).gameObject);
        }
        return list.ToArray();
    }

    public static Vector3 SetPositionGetDiff(this Transform trans,Vector3 vec)
    {
        var tmp = trans.position;
        trans.position = vec;
        return trans.position - tmp;
    }

    public static void AddPosition(this Transform trans, Vector3 vec)
    {
        var tmp = trans.position;
        trans.position += vec;
    }
}
