using UnityEngine;
using UnityEngine.UI;

public class SfxVolumeSlider : MonoBehaviour
{
    private void Start()
    {
        Slider slider = GetComponent<Slider>();
        //set the value so we remember the volume setting between settings pages e.g. main menu vs in game
        slider.value = AudioManager.Instance.SfxSource.volume;
    }

    public void SetSFXVolume(float amount)
    {
        AudioManager.Instance.SfxSource.volume = amount;
        AudioManager.Instance.EngineSfxSource.volume = amount;
    }
}
