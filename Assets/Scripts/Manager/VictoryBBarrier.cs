using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryBBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Balloon>().PopBalloon();
    }
}
