using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class CheckpointManager : SingletonMonoBehaviour<CheckpointManager>
{
    [SerializeField]
    private List<Transform> _checkPointList = new List<Transform>();

    private readonly int UnSetIndex = -1;

    public int CurrentPointNum { get; private set; } = -1;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // チェックポイント更新
    public void CheckpointUpdate(int pointNum) => CurrentPointNum = pointNum;

    // チェックポイントリセット
    public void ResetCheckpoint() => CurrentPointNum = UnSetIndex;

    // 現在のチェックポイント取得
    public Transform GetCurrentPoint()
    {
        if (CurrentPointNum == UnSetIndex) return null;
        if (_checkPointList.Count < CurrentPointNum) return null;

        return _checkPointList[CurrentPointNum];

    }
}
