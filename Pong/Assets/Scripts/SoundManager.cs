using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public bool musicOn;
    public bool soundOn;

    public AudioMixer audioMixer;
    public AudioMixerGroup audioMixerGroupMusic;
    public AudioMixerGroup audioMixerGroupSFX;

    public Sound[] musics;
    public Sound[] sounds;
    public AudioEvent[] audioEvents; //Experimental

    [SerializeField] Toggle toggleMusic;
    [SerializeField] Toggle toggleSound;

    public static SoundManager instance;

    private AudioSource audioSource;

    private void Awake()
    {
        #region singleton
        if (instance != null)
        {
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
        #endregion

        foreach (Sound s in musics)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = audioMixerGroupMusic;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = audioMixerGroupSFX;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        //load settings
        musicOn = (PlayerPrefs.GetInt("music", 1) == 1);
        soundOn = (PlayerPrefs.GetInt("sfx", 1) == 1);

        toggleMusic.isOn = musicOn;
        toggleSound.isOn = soundOn;

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayMusic("MainTheme");

        SetVolumeMusic(musicOn);
        SetVolumeSFX(soundOn);
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void PlayMusic(string name, float fadeTime)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        StartCoroutine(FadeIn(s.source, fadeTime));
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void StopMusic(string name, float fadeTime)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        StartCoroutine(FadeOut(s.source, fadeTime));
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    //Experimental
    public void PlayAudioEvent(string name)
    {
        AudioEvent a = Array.Find(audioEvents, audioEvent => audioEvent.name == name);
        if (a == null)
        {
            Debug.Log("Audio Event not found");
            return;
        }
        a.Play(audioSource);
    }

    public void PlaySound(string name, float fadeTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        StartCoroutine(FadeIn(s.source, fadeTime));
    }

    private IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        float startVolume = 0.2f;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < 1)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.volume = 1;
    }

    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public void SetVolumeMusic(bool on)
    {
        if (on)
        {
            audioMixerGroupMusic.audioMixer.SetFloat("VolumeMusic", 0);
            PlayerPrefs.SetInt("music", 1);
        }
        else
        {
            audioMixerGroupMusic.audioMixer.SetFloat("VolumeMusic", -80);
            PlayerPrefs.SetInt("music", 0);
        }
    }

    public void SetVolumeSFX(bool on)
    {
        if (on)
        {
            audioMixerGroupSFX.audioMixer.SetFloat("VolumeSound", 0);
            PlayerPrefs.SetInt("sfx", 1);
        }
        else
        {
            audioMixerGroupSFX.audioMixer.SetFloat("VolumeSound", -80);
            PlayerPrefs.SetInt("sfx", 0);
        }
    }
}
