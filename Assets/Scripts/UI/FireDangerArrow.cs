using UnityEngine;

public class FireDangerArrow : MonoBehaviour
{
    //[SerializeField] private GameObject fireParticles;
    [SerializeField] private ParticleSystem fireParticles;

    public Transform needle;

    void Update()
    {
        int clampValue = Mathf.Clamp(FireManager.instance.FireDangerLevel, 0, 90);
        //If you want to change it basically need the max to be something that will be able to = 180 max for the the clamp value so 30 * 6, 60 * 3, 90 *2 
        float angle = 90 - (clampValue*2);

        needle.localRotation = Quaternion.Euler(0f, 0f, angle);

        if (FireManager.instance.FireDangerLevel >= 90f) fireParticles.Play();
        else fireParticles.Stop();
    }
}