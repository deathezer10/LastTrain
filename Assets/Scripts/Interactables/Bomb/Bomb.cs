using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bomb : GrabbableObject
{
    public MeshRenderer phLightRenderer;
    public Color red, green;

    float timeRemaining = 180f;
    TextMeshPro timerTextMesh;

    private void Start()
    {
        timerTextMesh = GetComponentInChildren<TextMeshPro>();
        StartCoroutine(BombCountdown());
    }

    private IEnumerator BombCountdown()
    {
        while (timeRemaining >= 0)
        {
            timeRemaining -= Time.deltaTime;
            var integer = (int)timeRemaining;
            
            timerTextMesh.text = integer.ToString("0");
            
            yield return null;
        }

        TimerTimeOut();
    }

    public override void OnControllerEnter(PlayerViveController currentController, PlayerViveController.HandSource handSource)
    {
    }

    public override void OnControllerExit()
    {
    }

    public override void OnControllerStay()
    {
    }

    public override void OnGrab()
    {
    }

    public override void OnGrabReleased()
    {
    }

    public override void OnUse()
    {
    }

    private void TimerTimeOut()
    {
        phLightRenderer.material.color = red;
        FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_bomb");
    }

    public void CutWrongWire()
    {
        phLightRenderer.material.color = red;
        FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_bombwrongwire");
    }

    public void CutRightWire()
    {
        phLightRenderer.material.color = green;
        StopCoroutine(BombCountdown());
    }

    public void UnlockRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.None;
    }
}