using UnityEngine;

/// <summary>
/// Hold thrust audio source separately.
/// We want to play it constantly in the background while the player is accelerating,
/// and have the OneShots play over the top of it.
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class EngineAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip engineThrustAudio;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = engineThrustAudio;
    }

    public void Play()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void Pause() {
        audioSource.Pause();
    }
}
