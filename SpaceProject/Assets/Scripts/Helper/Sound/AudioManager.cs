using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

/// <summary>
/// Manager script for sounds - ships flying, objects exploding, level-ups etc
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;
    public void Awake() {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loops;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }
    void Start() {
        Play("BackgroundMusic");
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) { Debug.LogWarning("Music track named "+name+" not found!");  return; }
        s.source.Play();
    }

}
