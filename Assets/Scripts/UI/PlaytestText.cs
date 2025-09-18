using UnityEngine;

public class PlaytestText : MonoBehaviour
{
    Vector3 basePos;

    void Start()
    {
        basePos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = basePos + new Vector3(
            0.0f,
            Mathf.Abs(Mathf.Sin(Time.time * 4.0f) * 60.0f),
            0.0f
        );
    }
}
