using UnityEngine;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (VolumeSettingsManager.Instance == null) return;

        if (musicSlider != null)
        {
            musicSlider.value = VolumeSettingsManager.Instance.GetMusicVolume();
            musicSlider.onValueChanged.AddListener(VolumeSettingsManager.Instance.SetMusicVolume);
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = VolumeSettingsManager.Instance.GetSFXVolume();
            sfxSlider.onValueChanged.AddListener(VolumeSettingsManager.Instance.SetSFXVolume);
        }
    }

    public void SyncSliders()
    {
        if (musicSlider != null) musicSlider.value = VolumeSettingsManager.Instance.GetMusicVolume();
        if (sfxSlider != null) sfxSlider.value = VolumeSettingsManager.Instance.GetSFXVolume();
    }
}
