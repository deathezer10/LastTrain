using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOverDataManager : SingletonMonoBehaviour<TakeOverDataManager>
{
    [SerializeField]
    public int CurrentPointNum { get; set; } = CheckpointManager.UnSetIndex;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
