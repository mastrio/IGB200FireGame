using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettingsManager : MonoBehaviour
{
    public static VolumeSettingsManager Instance { get; private set; }

    [SerializeField] private AudioMixer myAudioMixer;
    private float backgroundMusic = 0.5f; 
    private float soundEffectsVolume = 0.5f;   

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load saved volumes or use defaults if not set.
        backgroundMusic = PlayerPrefs.GetFloat("BackgroundMusic", 0.5f);
        soundEffectsVolume = PlayerPrefs.GetFloat("SoundEffetcsVolume", 0.5f);

        // Apply loaded volumes.
        SetMusicVolume(backgroundMusic);
        SetSFXVolume(soundEffectsVolume);
    }

    public void SetMusicVolume(float volume)
    {
        backgroundMusic = Mathf.Clamp01(volume);
        myAudioMixer.SetFloat("BackgroundMusic", Mathf.Log10(backgroundMusic) * 20); 
        PlayerPrefs.SetFloat("BackgroundMusic", backgroundMusic); 
    }

    public void SetSFXVolume(float volume)
    {
        soundEffectsVolume = Mathf.Clamp01(volume);
        myAudioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(soundEffectsVolume) * 20); 
        PlayerPrefs.SetFloat("SoundEffectsVolume", soundEffectsVolume);
    }

    public float GetMusicVolume() => backgroundMusic;
    public float GetSFXVolume() => soundEffectsVolume;

    private void OnApplicationQuit()
    {
        // Reset to defaults on quit.
        PlayerPrefs.SetFloat("BackgroundMusic", 0.5f);
        PlayerPrefs.SetFloat("SoundEffectsVolume", 0.5f);
        PlayerPrefs.Save(); 
    }
}
