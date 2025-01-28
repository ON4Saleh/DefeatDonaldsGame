using System;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioClip[] musicSounds;
    public AudioClip[] sfxSounds;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("bg");
       
    }

    public void PlayMusic(string name)
    {
        AudioClip clip = Array.Find(musicSounds, x => x.name == name);
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
            musicSource.loop = true;
        }
        else
        {
            Debug.Log("Music not found: " + name);
        }
    }

    public void PlaySFX(string name)
    {
        AudioClip clip = Array.Find(sfxSounds, x => x.name == name);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("SFX not found: " + name);
        }
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void ToggleMusic()
    {
        musicSource.mute = !sfxSource.mute;
    }
    public void musicVolume(float Volume)
    {
        musicSource.volume = Volume;
    }
    public void sfxVolume(float Volume)
    {
        sfxSource.volume = Volume;
    }
}
