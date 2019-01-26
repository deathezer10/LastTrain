using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _checkPointList = new List<Transform>();

    public static readonly int UnSetIndex = -1;

    // チェックポイント更新
    public void CheckpointUpdate(int pointNum) => TakeOverDataManager.Instance.CurrentPointNum = pointNum;

    // チェックポイントリセット
    public void ResetCheckpoint() => TakeOverDataManager.Instance.CurrentPointNum = UnSetIndex;

    // 現在のチェックポイント取得
    public Transform GetCurrentPoint()
    {
        var currentNum = TakeOverDataManager.Instance.CurrentPointNum;

        if (currentNum == UnSetIndex) return null;
        if (_checkPointList.Count < currentNum) return null;

        return _checkPointList[currentNum];

    }
}
