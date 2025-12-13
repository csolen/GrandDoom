using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            if (PlayerPrefs.GetInt("isSoundsOn") == 1)
            {
                s.source.mute = true;
                s.source.enabled = false;
                PlayerPrefs.SetInt("isSoundsOn", s.source.mute ? 1 : 0);
            }
            else
            {
                s.source.mute = false;
                s.source.enabled = true;
                PlayerPrefs.SetInt("isSoundsOn", s.source.mute ? 1 : 0);
            }

        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (s.source.enabled)
        {
            s.source.Play();
        }

        if (PlayerPrefs.GetInt("isSoundsOn") == 1)
        {
            s.source.mute = true;
            PlayerPrefs.SetInt("isSoundsOn", s.source.mute ? 1 : 0);
        }
        else
        {
            s.source.mute = false;
            PlayerPrefs.SetInt("isSoundsOn", s.source.mute ? 1 : 0);
        }
    }

    public void MuteAllMusic()
    {
        foreach (Sound s in sounds)
        {
            if (s != null)
            {
                s.source.mute = true;
                s.source.enabled = false;
            }
        }
    }

    public void PlayAllMusic()
    {
        foreach (Sound s in sounds)
        {
            if (s != null)
            {
                s.source.playOnAwake = false;
                s.source.enabled = true;
                s.source.mute = false;
                s.source.playOnAwake = true;
            }
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.enabled = false;
    }

}