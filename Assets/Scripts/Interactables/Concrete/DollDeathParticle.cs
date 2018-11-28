using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollDeathParticle : MonoBehaviour
{
    [SerializeField]
    private GameObject dollEye;

    [SerializeField]
    private Transform leftEyeTrans;

    void Start()
    {
        GetComponent<AudioPlayer>().Play();

        StartCoroutine(ParticleTimer());
    }

    private IEnumerator ParticleTimer()
    {
        yield return new WaitForSeconds(2.5f);
        Instantiate(dollEye, leftEyeTrans.position, leftEyeTrans.rotation, null);
        Destroy(gameObject);
    }
}
