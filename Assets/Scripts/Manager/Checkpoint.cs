using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public static bool CHECKPOINT_ACTIVATED;

    public static Checkpoint Instance { get; private set; } = null;

    Vector3 playerResetPos = new Vector3(1f, 1.5f, 0f);
    Vector3 playerResetRot = new Vector3(0f, -90f, 0f);

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

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "TrainStation")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (CHECKPOINT_ACTIVATED)
            {
                player.transform.position = playerResetPos;
                player.transform.eulerAngles = playerResetRot;
            }
        }
    }

    private void Update()
    {
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
