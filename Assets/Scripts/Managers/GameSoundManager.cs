using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameSoundManager : MonoBehaviour
{
    public static GameSoundManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource fireSound;
    [SerializeField] private AudioSource forestSound;
    [SerializeField] private AudioSource animalSound;
    [SerializeField] private AudioSource insectSound;
    [SerializeField] private AudioSource steamingSound;

    private List<Transform> activeFireTransforms = new List<Transform>();
    private Transform playerTransform;
    [SerializeField] private float maxHearDistance = 20f;
    [SerializeField] private float minHearDistance = 5f;
    [SerializeField] private float minVolumeMultiplier = 0.2f;
    [SerializeField] private float intensityPerFire = 0.1f;
    [SerializeField] private int highIntensityThreshold = 3;

    private bool hasPlayedScream = false;
    private bool wasHighIntensity = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        fireSound = GetComponent<AudioSource>();
        playerTransform = GameObject.FindWithTag("Player")?.transform;

        if (forestSound != null) forestSound.Play();
        if (animalSound != null) animalSound.Play();
        if (insectSound != null) insectSound.Play();

        if (animalSound != null)
        {
            animalSound.spatialBlend = 0f;
            animalSound.rolloffMode = AudioRolloffMode.Logarithmic;
            animalSound.volume = 0.2f;
        }
    }

    private void Update()
    {
        int activeCount = activeFireTransforms.Count;
        float fireIntensity = Mathf.Clamp((float)activeCount / 5f, 0f, 1f);
        bool isHighIntensity = activeCount >= highIntensityThreshold;

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
                    if (fireTrans == null) continue;
                    float dist = Vector3.Distance(playerTransform.position, fireTrans.position);
                    if (dist < minDist) minDist = dist;
                }
                proximityFactor = Mathf.Clamp(1f - ((minDist - minHearDistance) / (maxHearDistance - minHearDistance)), minVolumeMultiplier, 1f);
            }
            fireSound.volume = baseVolume * intensityFactor * proximityFactor;

            // Play screaming sound once when intensity goes high
            if (isHighIntensity && !wasHighIntensity && !hasPlayedScream && steamingSound != null)
            {
                steamingSound.volume = 0.1f; //Subtle volume
                steamingSound.loop = false;
                steamingSound.PlayOneShot(steamingSound.clip);
            }
        }
        else
        {
            if (fireSound != null && fireSound.isPlaying) fireSound.Stop();
            PlayWithVariation(forestSound);
            PlayWithVariation(animalSound);
            PlayWithVariation(insectSound);
        }

        wasHighIntensity = isHighIntensity; // Update previous state

        float ambientVolume = Mathf.Lerp(0.5f, 0.05f, fireIntensity);
        if (forestSound != null)
        {
            forestSound.volume = ambientVolume * Random.Range(0.8f, 1.0f);
            forestSound.pitch = Random.Range(0.9f, 1.0f);
        }
        if (animalSound != null)
        {
            animalSound.volume = (ambientVolume * 0.2f) * Random.Range(0.8f, 1.0f);
            animalSound.pitch = Random.Range(0.9f, 1.0f);
            if (!animalSound.isPlaying) PlayWithVariation(animalSound);
        }
        if (insectSound != null)
        {
            insectSound.volume = (ambientVolume * 0.8f) * Random.Range(0.8f, 1.0f);
            insectSound.pitch = Random.Range(0.9f, 1.0f);
        }
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
        activeFireTransforms.RemoveAll(t => t == null);
    }
}