using UnityEngine;

public class FireDangerArrow : MonoBehaviour
{
    //[SerializeField] private GameObject fireParticles;
    [SerializeField] private ParticleSystem fireParticles;

    public Transform needle;

    void Update()
    {
        int clampValue = Mathf.Clamp(FireManager.instance.FireDangerLevel, 0, 60);

        float angle = 90 - (clampValue*3);

        needle.localRotation = Quaternion.Euler(0f, 0f, angle);

        if (FireManager.instance.FireDangerLevel >= 90f) fireParticles.Play();
        else fireParticles.Stop();
    }
}