using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordingText : MonoBehaviour {

	[SerializeField]
    private string wordKey = string.Empty;

    void Start () {
		StartCoroutine(Setup());
	}
	
	IEnumerator Setup () {
		yield return new WaitUntil(() => !WordManager.Instance.IsSetup);
        var text = GetComponent<Text>();
        text.text = WordManager.Instance.GetWord(wordKey);
	}
}
