using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerCheckpointReset : MonoBehaviour
{
    Vector3 playerResetPos = new Vector3(1f, 1.5f, 0f);
    Vector3 playerResetRot = new Vector3(0f, -90f, 0f);

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "TrainStation")
        {
            if (Checkpoint.CHECKPOINT_ACTIVATED)
            {
                transform.position = playerResetPos;
                transform.eulerAngles = playerResetRot;
            }
        }
    }
}
