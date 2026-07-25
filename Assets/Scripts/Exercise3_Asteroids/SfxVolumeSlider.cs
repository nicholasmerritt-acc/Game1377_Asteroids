using UnityEngine;
using UnityEngine.UI;

public class SfxVolumeSlider : MonoBehaviour
{
    private void Start()
    {
        Slider slider = GetComponent<Slider>();
        slider.value = AudioManager.Instance.SfxSource.volume;
    }

    public void SetSFXVolume(float amount)
    {
        AudioManager.Instance.SfxSource.volume = amount;
        AudioManager.Instance.EngineSfxSource.volume = amount;
    }
}
