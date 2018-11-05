﻿using System.Collections;
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

    public void CheckpointActivated()
    {
        CHECKPOINT_ACTIVATED = true;
    }

    public void ResetCheckpoint()
    {
        CHECKPOINT_ACTIVATED = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GetComponent<BoxCollider>().enabled = false;
            Debug.Log("Player hit the checkpoint.");
            CheckpointActivated();
            FindObjectOfType<TrainArriver>().CallTheTrain();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<BoxCollider>().enabled = false;
            Debug.Log("Player hit the checkpoint.");
            CheckpointActivated();
            FindObjectOfType<TrainArriver>().CallTheTrain();
        }
    }
}
