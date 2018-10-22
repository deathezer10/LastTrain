﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrainDoorHandler : MonoBehaviour
{

    const float m_DoorOffset = -1.1f;

    const string m_DoorNamePrefix = "PC_DoubleDr_";

    private enum DoorSide { Left, Right }

    private List<KeyValuePair<DoorSide, Transform>> m_Doors = new List<KeyValuePair<DoorSide, Transform>>();


    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform childTransform = transform.GetChild(i);

            int doorIndex = int.Parse(childTransform.name.Replace(m_DoorNamePrefix, string.Empty));

            if (doorIndex % 2 == 0)
                m_Doors.Add(new KeyValuePair<DoorSide, Transform>(DoorSide.Left, childTransform));
            else
                m_Doors.Add(new KeyValuePair<DoorSide, Transform>(DoorSide.Right, childTransform));
        }
    }

    public void ToggleDoors(bool opened, System.Action onComplete = null)
    {
        int direction = (opened) ? 1 : -1;

        for (int i = 0; i < m_Doors.Count; ++i)
        {
            var door = m_Doors[i];

            if (door.Key == DoorSide.Left)
            {
                var tweener = door.Value.DOLocalMoveZ(m_DoorOffset * direction, 2).SetRelative();

                if (i == 0)
                {
                    tweener.OnComplete(() =>
                    {
                        if (onComplete != null)
                            onComplete();
                    });
                }
            }
            else
            {
                var tweener = door.Value.DOLocalMoveZ(-m_DoorOffset * direction, 2).SetRelative();

                if (i == 0)
                {
                    tweener.OnComplete(() =>
                    {
                         if (onComplete != null)
                             onComplete();
                    });
                }
            }
        }

    }

}
