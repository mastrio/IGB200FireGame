using UnityEngine;

public class WindParticleUI : MonoBehaviour
{
    [SerializeField] private GameObject particlesObject;

    void Update()
    {
        particlesObject.transform.localRotation = Quaternion.Euler(new Vector3(
            0.0f,
            0.0f,
            WindManager.instance.directionDegrees - 90.0f
        ));
    }
}
