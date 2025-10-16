using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;
    [SerializeField] private Slider backgroundMusicSlider;
    [SerializeField] private Slider sfxMusicSlider;

    public void SetMusicVolume()
    {
        float volume = backgroundMusicSlider.value;
        myAudioMixer.SetFloat("BackgroundMusic", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume()
    {
        float volume = sfxMusicSlider.value;
        myAudioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(volume) * 20);
    }
}
