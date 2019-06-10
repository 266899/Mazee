using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range (0f, 1f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;
    public bool loop;
    [Range(0f, 1f)]
    public float spatialSound;
    public float minDistance;
}
