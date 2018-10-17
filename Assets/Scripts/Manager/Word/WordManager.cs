using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Trident;

public class WordManager : SingletonMonoBehaviour<WordManager> {
	// application url
    const string URL = "https://script.google.com/macros/s/AKfycbzRSrUhjvQSuQBPbpXfV0kAukbZi5l7QFyxOklgMz_Htro5A6E/exec";
    private string _sheetName = "word";
    private SerializeDictionary<string, string> _word;

	[SerializeField]
    private Language _currentLanguage = Language.En;
    public Language CurrentLanguage
    {
        get { return _currentLanguage; }
        set { _currentLanguage = value; }
    }

    public bool IsSetup { get; set; }

    private void Awake() {
        this.IsSetup = false;
        StartCoroutine(Setup());
	}

    public IEnumerator Setup()
    {
        var download = new WWW(URL + "?sheetName=" + _sheetName+"&language="+_currentLanguage.ToStringQuickly());
        IsSetup = true;
        while (!download.isDone)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log(download.text);

        var json = JsonUtility.FromJson<SerializeDictionary<string, string>>(download.text);
        if (json == null)
        {
            Debug.LogError(download.text);
        }
        else
        {
            Debug.Log("Loading Complete.");
        }

        _word = json;

        IsSetup = false;
    }

	public string GetWord(string key)
	{
		string word = _word[key];
		if(string.IsNullOrEmpty(word))
		{
			Debug.LogError("not setting word key");
		}

		return word;
	}
}