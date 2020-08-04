using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void SetPitch (float newPitch, string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.pitch = newPitch;
    }

    public void SetVolume (float newVolume, string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = newVolume;
    }

    public float ReturnPitch(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.pitch;
    }

    public float ReturnVolume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.volume;
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
}
