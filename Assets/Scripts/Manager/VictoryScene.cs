using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScene : MonoBehaviour
{

    public void LoadTrainStationScene()
    {
        TakeOverDataManager.Instance.CheckPointReset();
        SceneManager.LoadScene("TrainStation");
    }
}
