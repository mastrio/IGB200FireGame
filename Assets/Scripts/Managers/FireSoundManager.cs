using UnityEngine;

public class FireSoundManager : MonoBehaviour
{
    public static FireSoundManager Instance { get; private set; }

    private AudioSource fireSound;
    private int activeFires = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        fireSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (activeFires > 0)
        {
            if (!fireSound.isPlaying)
            {
                fireSound.Play();
            }
        }
        else
        {
            if (fireSound.isPlaying)
            {
                fireSound.Stop();
            }
        }
    }

    public void RegisterFireStart()
    {
        activeFires++;
    }

    public void RegisterFireStop()
    {
        activeFires--;
        if (activeFires < 0) activeFires = 0;
    }
}