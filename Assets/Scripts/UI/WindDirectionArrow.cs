using UnityEngine;

public class WindDirectionArrow : MonoBehaviour
{
    [SerializeField] private GameObject windArrow;

    void Update()
    {
        windArrow.transform.localRotation = Quaternion.Euler(new Vector3(
            0.0f,
            0.0f,
            -WindManager.instance.directionDegrees
        ));
    }
}
