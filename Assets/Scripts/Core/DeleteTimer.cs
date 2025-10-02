using UnityEngine;

public class DeleteTimer : MonoBehaviour
{
    [SerializeField] private float timer = 1.0f;

    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= (startTime + timer)) Destroy(gameObject);
    }
}
