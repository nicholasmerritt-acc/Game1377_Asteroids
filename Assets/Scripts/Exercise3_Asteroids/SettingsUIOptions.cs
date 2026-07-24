using UnityEngine;

public class SettingsUIOptions : MonoBehaviour
{

    public void SetMusicVolume(float amount)
    {
        AudioManager.Instance.MusicSource.volume = amount;
    }

    public void SetSFXVolume(float amount)
    {
        AudioManager.Instance.SfxSource.volume = amount;
        AudioManager.Instance.EngineSfxSource.volume = amount;
    }
}
