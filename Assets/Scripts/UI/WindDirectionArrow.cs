using UnityEngine;
using Random = UnityEngine.Random;

public class WindDirectionArrow : MonoBehaviour
{
    // TEMP
    private float timer = 0.0f;

    void Update()
    {
        // TEMP: Will be changed to actually show the wind direction once that is implemented
        if (Time.time > timer)
        {
            timer = Time.time + 1.0f;
            transform.localRotation = Quaternion.Euler(new Vector3(
                0.0f,
                0.0f,
                Random.Range(0.0f, 360.0f)
            ));
        }

        // For playtest
        transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
    }
}
