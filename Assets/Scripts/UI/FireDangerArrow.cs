using UnityEngine;

public class FireDangerArrow : MonoBehaviour
{
    void Update()
    {
        // TEMP
        transform.localRotation = Quaternion.Euler(new Vector3(
            0.0f,
            0.0f,
            Mathf.Sin(Time.time * 2.0f) * 88.0f // TODO: Replace this with a value that actually represents the fire danger level, once we have a way to calculate that
        ));
    }
}
