using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropSoundClipManager : SingletonMonoBehaviour<DropSoundClipManager>
{

    [Serializable]
    public struct DropSoundKeyPair
    {
        public DropSoundHandler.DropSoundType dropSoundType;
        public AudioClip audioClip;
    }

    [SerializeField]
    DropSoundKeyPair[] m_Clips;

    /// <summary>
    /// Linear search, avoid calling this too often
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public AudioClip GetClip(DropSoundHandler.DropSoundType type)
    {
        foreach (DropSoundKeyPair kp in m_Clips)
        {
            if (kp.dropSoundType == type)
                return kp.audioClip;
        }

        return null;
    }

}