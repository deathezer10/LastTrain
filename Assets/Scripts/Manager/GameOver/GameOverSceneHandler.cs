using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverSceneHandler : MonoBehaviour
{

    public Text m_DeathDescription;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(SetDeathText());
    }

    IEnumerator SetDeathText()
    {
        while (WordManager.Instance.IsSetup)
        {
            yield return null;
        }

        m_DeathDescription.text = WordManager.Instance.GetWord(PlayerDeathHandler.GameOverTextKey);
    }

    public void LoadTrainStationScene()
    {
        SceneManager.LoadScene("TrainStation");
    }

}
