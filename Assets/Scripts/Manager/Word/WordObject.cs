using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WordObject : ScriptableObject
{
    public SerializeDictionary<string, string> _words;

    public string GetWord(string key)
    {
        var word = _words[key];
        if (string.IsNullOrEmpty(word))
        {
            Debug.LogError("not setting word key");
            word = "not setting word";
        }
        return word;
    }
}