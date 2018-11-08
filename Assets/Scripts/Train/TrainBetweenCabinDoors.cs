using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class TrainBetweenCabinDoors : MonoBehaviour
{
    float offset = -1.1f;

    List<GameObject> betweenDoors = new List<GameObject>();

    BoxCollider betweenDoorsCollider;

    AudioPlayer betweenDoorsAudio;

    bool open;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (child.name.Contains("BtwnCabinDr"))
            {
                betweenDoors.Add(child);
            }
        }

        betweenDoorsCollider = gameObject.AddComponent<BoxCollider>();
        betweenDoorsCollider.center = new Vector3(0f, 1.1f, -9.62f);
        betweenDoorsCollider.size = new Vector3(1.25f, 2f, 1.25f);
    }

    public void OpenBetweenDoors()
    {
        if (!open)
        {
            foreach (GameObject door in betweenDoors)
            {
                var tweener = door.transform.DOLocalMoveX(offset, 1.5f).SetRelative();
            }

            //betweenDoorsAudio.Play();

            StartCoroutine(OnDoorOpenFinish());

            open = true;
        }
    }

    public void CloseBetweenDoors()
    {
        foreach (GameObject door in betweenDoors)
        {
            var tweener = door.transform.DOLocalMoveX(-offset, 1.5f).SetRelative();
        }

        //betweenDoorsAudio.Play();

        betweenDoorsCollider.enabled = true;

        open = false;
    }

    IEnumerator OnDoorOpenFinish()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("Play Announcement");
        // Call for the next announcement

        yield return new WaitForSeconds(1.05f);

        betweenDoorsCollider.enabled = false;
    }
}
