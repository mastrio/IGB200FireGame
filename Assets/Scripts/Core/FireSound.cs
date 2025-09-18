using UnityEngine;

public class FireSound : MonoBehaviour
{
    private ParticleSystem firePsSystem;
    private AudioSource fireSound;

    private void Awake()
    {
        firePsSystem = GetComponent<ParticleSystem>();
        fireSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (firePsSystem.IsAlive(true))
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
}
