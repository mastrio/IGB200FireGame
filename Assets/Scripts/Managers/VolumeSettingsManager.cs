using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettingsManager : MonoBehaviour
{
    public static VolumeSettingsManager Instance { get; private set; }

    [SerializeField] private AudioMixer myAudioMixer;
    [SerializeField] private AudioSource sliderSoundEffect;
    private float backgroundMusic = 0.5f;
    private float soundEffectsVolume = 0.5f;

    private bool isInitializing = true; 
    private float lastSoundTime; 
    private float soundCooldown = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load saved volumes or use defaults if not set
        backgroundMusic = PlayerPrefs.GetFloat("BackgroundMusic", 0.5f);
        soundEffectsVolume = PlayerPrefs.GetFloat("SoundEffectsVolume", 0.5f);

        // Apply loaded volumes
        isInitializing = true;
        SetMusicVolume(backgroundMusic);
        SetSFXVolume(soundEffectsVolume);
        isInitializing = false;

        // Initialize slider sound effect
        if (sliderSoundEffect != null)
        {
            sliderSoundEffect.outputAudioMixerGroup = myAudioMixer.FindMatchingGroups("SoundEffectsVolume")[0]; 
            sliderSoundEffect.playOnAwake = false;
            sliderSoundEffect.loop = false;
        }
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
        
        // Play slider sound effect 
        if (!isInitializing && sliderSoundEffect != null && sliderSoundEffect.clip != null)
        {
            float currentTime = Time.time;
            if (currentTime - lastSoundTime >= soundCooldown)
            {
                sliderSoundEffect.PlayOneShot(sliderSoundEffect.clip);
                lastSoundTime = currentTime;
            }
        }
    }

    public float GetMusicVolume()
    {
        return backgroundMusic;
    }

    public float GetSFXVolume()
    {
        return soundEffectsVolume;
    }

    private void OnApplicationQuit()
    {
        // Reset to defaults when quit the game
        PlayerPrefs.SetFloat("BackgroundMusic", 0.5f);
        PlayerPrefs.SetFloat("SoundEffectsVolume", 0.5f);
        PlayerPrefs.Save(); 
    }
}
