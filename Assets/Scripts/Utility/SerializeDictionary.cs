using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializeDictionary<T1, T2> : Dictionary<T1, T2>, ISerializationCallbackReceiver
{
    [SerializeField] private List<T1> key = new List<T1>();
    [SerializeField] private List<T2> value = new List<T2>();

    public void OnBeforeSerialize()
    {
        key.Clear();
        value.Clear();

        var e = GetEnumerator();

        while (e.MoveNext())
        {
            key.Add(e.Current.Key);
            value.Add(e.Current.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        int cnt = (key.Count <= value.Count) ? key.Count : value.Count;
        for (int i = 0; i < cnt; ++i)
            this[key[i]] = value[i];

    }
}