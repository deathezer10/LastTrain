using System;
using System.IO; // ファイル書き込みに必要
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trident;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WordManager : SingletonMonoBehaviour<WordManager> {
	// application url
    const string URL = "https://script.google.com/macros/s/AKfycbzRSrUhjvQSuQBPbpXfV0kAukbZi5l7QFyxOklgMz_Htro5A6E/exec";
    private string _sheetName = "word";
    private Dictionary<string,Dictionary<string, string>> _wordObjects = new Dictionary<string,Dictionary<string, string>>();
    private Dictionary<string,string> _currentWord;

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
        yield return StartCoroutine(LoadAllWords());

        _currentWord = _wordObjects[_currentLanguage.ToStringQuickly()];

        Debug.Log("complete.");

        IsSetup = false;
    }

    private IEnumerator LoadAllWords()
    {
        foreach (Language language in Enum.GetValues(typeof(Language)))
        {
            var jsonString = string.Empty;

            SerializeDictionary<string,string> json = null;

            // TODO: Make Loading at the same time
            yield return StartCoroutine(LoadWebJson(language, (_) =>
            {
                jsonString = _;
                if(!string.IsNullOrEmpty(jsonString)){
                    json = JsonToDic(jsonString);
                    // 保存
                    SaveJson(language, jsonString);
                }
            }));

            if (string.IsNullOrEmpty(jsonString))
            {
                // 読み込み
                yield return StartCoroutine(LoadLocalJson(language, (text) =>
                {
                    jsonString = text;
                    json = JsonToDic(jsonString);
                }));
            }

            // dicを設定
            _wordObjects[language.ToStringQuickly()] = json;
        }
    }

    private IEnumerator LoadWebJson(Language language, System.Action<string> callback)
    {
        var download = new WWW(URL + "?sheetName=" + _sheetName + "&language=" + language.ToStringQuickly());
        IsSetup = true;
        while (!download.isDone)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (!string.IsNullOrEmpty(download.error))
        {
            Debug.LogError("download Error:" + download.error);
        }
        Debug.Log(download.text);

        callback(download.text);
    }

    private SerializeDictionary<string, string> JsonToDic(string jsonString)
    {
        var json = JsonUtility.FromJson<SerializeDictionary<string, string>>(jsonString);
        if (json == null)
        {
            Debug.LogError(jsonString);
        }
        else
        {
            Debug.Log("Loading Complete.");
        }
        return json;
    }

    private void SaveJson(Language language,string text)
    {
        string path = "Resources/WordJson/" + language.ToStringQuickly()+".txt";

        var fullPath = Application.dataPath + "/" + path;

        StreamWriter writer;

        if (!System.IO.File.Exists(fullPath))
        {
#if UNITY_EDITOR
            // 存在しない場合作成
            writer = File.CreateText(fullPath); 
#else
            Debug.LogError("text don't exist");
            return;
#endif
        }else{
            writer = new StreamWriter(fullPath, false); // 上書き
        }
        writer.WriteLine(text);
        writer.Flush();
        writer.Close();
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    private IEnumerator LoadLocalJson(Language language ,Action<string> callback)
    {
        string path = "WordJson/" + language.ToStringQuickly();

        var fullPath = Application.dataPath + "/" + path;
        // Assetsフォルダからロード
        var request = Resources.LoadAsync(path);  
        yield return new WaitUntil(()=>!request.isDone);

        var text = request.asset as TextAsset;

        callback(text.text);
    }

    public string GetWord(string key)
	{
		string word = _currentWord[key];

        if (string.IsNullOrEmpty(word))
        {
            Debug.LogError("not setting word key");
            word = "not setting word";
        }

		return word;
	}
}