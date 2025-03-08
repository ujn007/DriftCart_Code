using Hellmade.Sound;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] Sound[] musicSound, sfxSound;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void PlayMusic(string name, float sound = 1)
    {
        Sound s = Array.Find(musicSound, x => x.name == name);

        if (s == null)
            Debug.Log("Music Not Found");
        else
        {
            EazySoundManager.PlayMusic(s.clip, sound, true, false);
        }
    }

    public void PlaySFX(string name, float sound, bool loop = false)
    {
        Sound s = Array.Find(sfxSound, x => x.name == name);

        if (s == null)
            Debug.Log("SFX Not Found");
        else
        {
            EazySoundManager.PlaySound(s.clip, sound, loop, null);
        }
    }

    public void PauseAllSound(bool v)
    {
        if (v)
            EazySoundManager.PauseAll();
        else
        {
            EazySoundManager.ResumeAll();
        }

    }

    public void StopAllSound()
    {
        EazySoundManager.StopAll();
    }
}
