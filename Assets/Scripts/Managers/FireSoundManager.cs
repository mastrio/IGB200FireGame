using System.Collections.Generic;
using UnityEngine;

public class FireSoundManager : MonoBehaviour
{
    public static FireSoundManager Instance { get; private set; }

    private AudioSource fireSound;
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
        fireSound = GetComponent<AudioSource>();
        playerTransform = GameObject.FindWithTag("Player")?.transform; // Tag your player "Player".
    }

    private void Update()
    {
        int activeCount = activeFireTransforms.Count;
        if (activeCount > 0)
        {
            if (!fireSound.isPlaying)
            {
                fireSound.Play(); // Starts/restarts single playback only when needed.
            }

            // Dynamic volume: base * intensity * proximity.
            float baseVolume = 0.5f; // Set this to your inspector's base volume.
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
            if (fireSound.isPlaying)
            {
                fireSound.Stop();
            }
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
        // Clean up any null transforms (e.g., destroyed fires).
        activeFireTransforms.RemoveAll(t => t == null);
    }
}