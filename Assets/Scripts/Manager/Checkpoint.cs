using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static bool CHECKPOINT_ACTIVATED;

    public static Checkpoint Instance { get; private set; } = null;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Test PH
        if (Input.GetKeyDown(KeyCode.B))
        {
            CheckpointActivated();
        }
    }

    public void CheckpointActivated()
    {
        CHECKPOINT_ACTIVATED = true;
    }

    public void ResetCheckpoint()
    {
        CHECKPOINT_ACTIVATED = false;
    }
}
