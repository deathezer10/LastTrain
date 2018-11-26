using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollDeathAnnouncements : MonoBehaviour
{
    string[] dollDeathClips = { "doll_death1", "doll_death2", "doll_death3" };

    int clipIndex = -1;

    public string nextClip()
    {
        clipIndex++;

        if (clipIndex >= dollDeathClips.Length)
        {
            clipIndex = 0;
        }

        return dollDeathClips[clipIndex];
    }
}
