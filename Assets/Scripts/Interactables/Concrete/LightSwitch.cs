using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using DG.Tweening;

public class LightSwitch : GrabbableObject, IShootable
{


    public string SwitchCabinsName;
    private bool bSwitchIsOn = false;
    private bool bIsBroken = false;
    private int ActivateCount = 0;
    private int BreakAtCount;
    private float x;
    private bool bCount = false;
    private AudioPlayer Audio;
    private List<ToggleTrainLights> toggleTrainLights = new List<ToggleTrainLights>();
    private ToggleTrainLights ownedLights;
    private Vector3 OriginalPosition;
    private Quaternion OriginalRotation;

    private void Start()
    {
        Audio = GetComponent<AudioPlayer>();
        OriginalPosition = transform.localPosition;
        OriginalRotation = transform.localRotation;
        BreakAtCount = Mathf.RoundToInt(Random.Range(3, 5));
        toggleTrainLights.AddRange(FindObjectsOfType<ToggleTrainLights>());
        for (int i = 0; i < toggleTrainLights.Count; i++)
        {
            if (toggleTrainLights[i].SwitchCabinsName == SwitchCabinsName)
            {
                ownedLights = toggleTrainLights[i];
            }
        }
    }

    void Update()
    {
        if (bCount)
        {
            x += Time.deltaTime;
        }
    }

    public override bool hideControllerOnGrab { get { return false; } }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);

        if (bIsBroken)
            return;

        if (ActivateCount >= BreakAtCount)
        {
            //Light break sound here?
            ownedLights.LightsOff();
            bIsBroken = true;
            bSwitchIsOn = false;
            transform.GetComponent<Rigidbody>().useGravity = true;
            transform.GetComponent<Rigidbody>().isKinematic = false;
            transform.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            bCount = true;
            return;
        }

        Audio.Play();

        var source = currentController.GetCurrentHand();
        currentController.Vibration(0, 0.2f, 5, 1, source);

        if (bSwitchIsOn)
        {
            bSwitchIsOn = false;

            transform.localRotation = Quaternion.Euler(0, 0, 0);

            ownedLights.LightsOff();
        }
        else
        {
            bSwitchIsOn = true;
            ActivateCount += 1;
            transform.localRotation = Quaternion.Euler(0, 0, -90);

            ownedLights.LightsOn();
        }

    }

    public override void OnControllerExit()
    {
        base.OnControllerExit();
    }

    public override void OnControllerStay()
    {
        if (bIsBroken)
        {
            if (x > 2)
                if (AlmostEqual(transform.localPosition, OriginalPosition, 0.05f))
                {
                    transform.GetComponent<Rigidbody>().useGravity = false;
                    transform.GetComponent<Rigidbody>().isKinematic = true;
                    transform.localPosition = OriginalPosition;
                    transform.localRotation = OriginalRotation;
                    ActivateCount = 0;
                    bCount = false;
                    x = 0;
                    bIsBroken = false;
                    return;
                }
        }
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

    public override void OnUseDown()
    {
    }

    public override void OnUseUp()
    {
    }

    public void OnShot(Revolver revolver)
    {
        if (bSwitchIsOn)
        {
            ownedLights.LightsOff();
        }

        Destroy(transform.gameObject);
        Destroy(this);

    }

    private bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;

        if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
        if (Mathf.Abs(v1.y - v2.y) > precision) equal = false;
        if (Mathf.Abs(v1.z - v2.z) > precision) equal = false;

        return equal;
    }

}
