using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private AudioMixerGroup soundEffectMixer;

    [SerializeField]
    private AudioMixerGroup musicMixer;

    [SerializeField]
    private Sound[] sounds;

    [SerializeField]
    private Sound[] music;

    private Sound currentPlayingMusic;
    private const int SoundAdjustFactor = 20;
    private const int LogFactor = 10;
    private const string SoundEffectKey = "SoundEffect";
    private const string MusicKey = "Music";

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
        }

        InitializeSounds(sounds, soundEffectMixer);
        InitializeSounds(music, musicMixer);

        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusic(scene.name);
    }

    public void InitializeSounds(Sound[] soundArray, AudioMixerGroup mixerGroup)
    {
        foreach (Sound sound in soundArray)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;

            sound.source.outputAudioMixerGroup = mixerGroup;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void PlayMusic(string name)
    {
        if (currentPlayingMusic != null)
        {
            currentPlayingMusic.source.Stop();
        }

        Sound song = Array.Find(music, sound => sound.name == name);
        currentPlayingMusic = song;
        
        song.source.Play();
    }

    public void SetMusicVolumeLevel(float sliderValue)
    {
        audioMixer.SetFloat(MusicKey, Mathf.Log10(sliderValue) * SoundAdjustFactor);
    }

    public void SetSoundEffectVolumeLevel(float sliderValue)
    {
        audioMixer.SetFloat(SoundEffectKey, Mathf.Log10(sliderValue) * SoundAdjustFactor);
    }

    public float GetMusicVolumeLevel()
    {
        float value = 0.0f;
        audioMixer.GetFloat(MusicKey, out value);

        float newValue = Mathf.Pow(LogFactor, value / SoundAdjustFactor);

        return newValue;
    }

    public float GetSoundEffectVolumeLevel()
    {
        float value = 0.0f;
        audioMixer.GetFloat(SoundEffectKey, out value);

        float newValue = Mathf.Pow(LogFactor, value / SoundAdjustFactor);

        return newValue;
    }
}
