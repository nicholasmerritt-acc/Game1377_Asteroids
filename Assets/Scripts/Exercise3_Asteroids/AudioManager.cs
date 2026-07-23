using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    public AudioSource MusicSource;
    [SerializeField] private AudioClip backgroundMusic;

    [Header("SFX")]
    public AudioSource SfxSource;
    public AudioSource EngineSfxSource;
    [SerializeField] private AudioClip thrustClip;
    [SerializeField] private AudioClip spaceshipExplodeClip;
    [SerializeField] private AudioClip asteroidExplodeClip;
    [SerializeField] private AudioClip bulletClip;
    [SerializeField] private AudioClip spaceshipTeleportClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic();
        }
        EngineSfxSource.clip = thrustClip;
    }

    private void PlayBackgroundMusic()
    {
        MusicSource.clip = backgroundMusic;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    public void PlayAsteroidExplodeClip()
    {
        SfxSource.PlayOneShot(asteroidExplodeClip);
    }

    public void PlayBulletClip()
    {
        SfxSource.PlayOneShot(bulletClip);
    }

    public void PlayEngineAudio()
    {
        if (!EngineSfxSource.isPlaying)
        {
            EngineSfxSource.Play();
        }
    }

    public void PauseEngineAudio()
    {
        EngineSfxSource.Pause();
    }

    public void PlaySpaceshipTeleportClip()
    {
        SfxSource.PlayOneShot(spaceshipTeleportClip);
    }

    public void PlaySpaceshipExplodeAudio()
    {
        SfxSource.PlayOneShot(spaceshipExplodeClip);
    }
}
