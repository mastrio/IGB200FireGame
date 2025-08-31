using UnityEngine;

public class PhoneClouds : MonoBehaviour
{
    [SerializeField] private float animationSpeed = 1.0f;
    [SerializeField] private float animationHeight = 1.0f;

    void Update()
    {
        transform.localPosition = new Vector3(
            0.0f,
            Mathf.Sin(Time.time * animationSpeed) * animationHeight,
            0.0f
        );
    }
}
