using UnityEngine;

public class SettingsUIOptions : MonoBehaviour
{
    public GameObject SettingsPanel;

    public void DeactivateSettingsPanel()
    {
        SettingsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            DeactivateSettingsPanel();
        }
    }

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
