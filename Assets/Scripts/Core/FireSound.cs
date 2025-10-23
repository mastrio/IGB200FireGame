using UnityEngine;

public class FireSound : MonoBehaviour
{
    private ParticleSystem firePsSystem;
    private bool wasAlive = false;

    private void Awake()
    {
        firePsSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        bool isAlive = firePsSystem.IsAlive(true);

        if (isAlive && !wasAlive)
        {
            GameSoundManager.Instance?.FireStart(transform);
            wasAlive = true;
        }
        else if (!isAlive && wasAlive)
        {
            GameSoundManager.Instance?.FireStop(transform);
            wasAlive = false;
        }
    }

    private void OnDestroy()
    {
        if (wasAlive)
        {
            GameSoundManager.Instance?.FireStop(transform);
        }
    }
}