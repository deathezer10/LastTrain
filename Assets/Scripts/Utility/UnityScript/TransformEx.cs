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
    public static List<GameObject> GetAllChild(this Transform transform, List<GameObject> objects = null)
    {
        List<GameObject> list;
        if(objects == null) list = new List<GameObject>();
        else list = objects;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            list.Add(child);
            var childs = child.transform.GetAllChild();
            list.AddRange(childs);
        }
        return list;
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
