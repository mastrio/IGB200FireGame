using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FireSoundManager : MonoBehaviour
{
    public static FireSoundManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource fireSound;
    [SerializeField] private AudioSource forestSound;
    [SerializeField] private AudioSource animalSound;
    [SerializeField] private AudioSource insectSound;

    private List<Transform> activeFireTransforms = new List<Transform>();
    private Transform playerTransform;
    [SerializeField] private float maxHearDistance = 20f; // Fade beyond this.
    [SerializeField] private float minHearDistance = 5f;  // Full volume within this.
    [SerializeField] private float minVolumeMultiplier = 0.2f; // Min when far.
    [SerializeField] private float intensityPerFire = 0.1f; // +10% per extra fire.

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        playerTransform = GameObject.FindWithTag("Player")?.transform;

        
        if (forestSound != null)
            forestSound.Play();

        if (animalSound != null)
            animalSound.Play();

        if (insectSound != null)
            insectSound.Play();
        
        if (animalSound != null)
        {
            animalSound.spatialBlend = 0f; // No spatial effect
            animalSound.rolloffMode = AudioRolloffMode.Logarithmic; // Default rolloff
            animalSound.volume = 0.45f;
        }
    }
    private void Update()
    {
        int activeCount = activeFireTransforms.Count;
        float fireIntensity = Mathf.Clamp((float)activeCount / 5f, 0f, 1f);

        if (activeCount > 0)
        {
            PlayWithVariation(fireSound);

            float baseVolume = 0.5f;
            float intensityFactor = Mathf.Clamp(1f + (activeCount - 1) * intensityPerFire, 1f, 2f);
            float proximityFactor = minVolumeMultiplier;
            if (playerTransform != null)
            {
                float minDist = float.MaxValue;
                foreach (Transform fireTrans in activeFireTransforms)
                {
                    if (fireTrans == null) continue; // Skip destroyed.
                    float dist = Vector3.Distance(playerTransform.position, fireTrans.position);
                    if (dist < minDist) minDist = dist;
                }
                proximityFactor = Mathf.Clamp(1f - ((minDist - minHearDistance) / (maxHearDistance - minHearDistance)), minVolumeMultiplier, 1f);
            }

            fireSound.volume = baseVolume * intensityFactor * proximityFactor;
        }
        else
        {
            if (fireSound.isPlaying) fireSound.Stop();
            PlayWithVariation(forestSound);
            PlayWithVariation(animalSound);
            PlayWithVariation(insectSound);
        }

        float ambientVolume = Mathf.Lerp(0.5f, 0.05f, fireIntensity);
        if (forestSound != null) forestSound.volume = ambientVolume;
        if (animalSound != null) animalSound.volume = ambientVolume * 0.8f;
        if (insectSound != null) insectSound.volume = ambientVolume * 0.5f;

        // Ensure sounds keep playing
        if (animalSound != null && !animalSound.isPlaying) PlayWithVariation(animalSound);
    }
    private void PlayWithVariation(AudioSource source)
    {
        if (source != null && !source.isPlaying)
        {
            source.pitch = Random.Range(0.9f, 1.0f);
            source.volume = Random.Range(0.8f, 1.0f);
            source.Play();
        }
    }

    public void RegisterFireStart(Transform fireTransform)
    {
        if (!activeFireTransforms.Contains(fireTransform))
        {
            activeFireTransforms.Add(fireTransform);
        }
    }

    public void RegisterFireStop(Transform fireTransform)
    {
        activeFireTransforms.Remove(fireTransform);
    }

    private void LateUpdate()
    {
        // Destroy fire
        activeFireTransforms.RemoveAll(t => t == null);
    }
}