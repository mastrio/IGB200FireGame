using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;
    [SerializeField] private Slider menuMusicSlider;

    public void SetMusicVolume()
    {
        float volume = menuMusicSlider.value;
        myAudioMixer.SetFloat("MainMenuMusic", Mathf.Log10(volume)*20);
    }
}
