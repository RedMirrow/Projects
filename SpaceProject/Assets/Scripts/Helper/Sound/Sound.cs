using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// A custom class to hold the data of a sound asset
/// </summary>
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f,1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch=1f;
    public bool loops;

    [HideInInspector]
    public AudioSource source;

}
