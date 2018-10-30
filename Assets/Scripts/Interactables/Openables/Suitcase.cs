using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suitcase : GrabbableObject
{
    public GameObject cardHolder;

    AudioPlayer openAudio;
    Animator suitcaseAnimator;
    bool opened;

    void Start()
    {
        suitcaseAnimator = GetComponent<Animator>();
        openAudio = GetComponent<AudioPlayer>();
    }

    private void OpenSuitCase()
    {
        suitcaseAnimator.Play("suitcase_open");
        openAudio.Play();

        RearrangeColliders();

        var tmp = new List<Transform>();

        for (int i = 0; i < cardHolder.transform.childCount; i++)
        {
            var obj = cardHolder.transform.GetChild(i);
            obj.gameObject.SetActive(true);
            tmp.Add(obj);
        }

        foreach (var trans in tmp)
        {
            trans.SetParent(null);
        }
    }

    private void RearrangeColliders()
    {
        BoxCollider oldBC = GetComponent<BoxCollider>();

        BoxCollider newBC = gameObject.AddComponent<BoxCollider>();

        newBC.center = new Vector3(0, 0.13f, 0.1f);
        newBC.size = new Vector3(0.31f, 0.2f, 0.015f);

        newBC = gameObject.AddComponent<BoxCollider>();

        newBC.center = new Vector3(0, 0.012f, 0f);
        newBC.size = new Vector3(0.31f, 0.015f, 0.205f);

        Destroy(oldBC);
    }

    public override void OnControllerEnter(PlayerViveController currentController)
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
        if (!opened)
        {
            OpenSuitCase();
            opened = true;
        }
    }

    public override void OnUseDown()
    {
    }

    public override void OnUseUp()
    {
    }

}
