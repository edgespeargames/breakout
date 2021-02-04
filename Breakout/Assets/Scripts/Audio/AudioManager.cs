using System;
using System.Collections;
using UnityEngine;

//Taken from Brackeys Audio in Unity introduction
//https://www.youtube.com/watch?v=6OT43pvUyfY

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public bool mute;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.mute = s.mute;
        }
    }

    private void Start()
    {
        SaveSystem.LoadMute();
        GetComponentInChildren<ToggleMuteImage>().OnToggleMute(mute);
        SetMute();
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return false;

        return s.source.isPlaying;
    }

    public void FadeAll()
    {
        foreach (Sound s in sounds)
        {
            StartCoroutine(Fade(s.source));
        }
    }

    public void SetMute()
    {
        foreach (Sound s in sounds)
        {
            if (mute)
            {
                s.source.volume = 0;
            }
            else
            {
                s.source.volume = 1;
            }
        }
    }

    public void MuteToggle()
    {
        mute = !mute;
        SetMute();
        GetComponentInChildren<ToggleMuteImage>().OnToggleMute(mute);
        SaveSystem.SaveMute();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        s.source.Play();
    }
    public void PlayOnce(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        s.source.PlayOneShot(s.clip);
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        s.source.Stop();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        s.source.Pause();
    }

    public void Resume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        s.source.UnPause();
    }

    public void FadeOut(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        StartCoroutine(Fade(s.source));
    }

    IEnumerator Fade(AudioSource source)
    {
        while (source.volume > 0)
        {
            source.volume -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        source.Stop();
        source.volume = 1;
    }
}
