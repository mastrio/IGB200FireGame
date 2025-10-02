using UnityEngine;

public class WindManager : MonoBehaviour
{
    [HideInInspector] public static WindManager instance;

    [SerializeField] private GameObject windParticles;

    public float directionDegrees;
    [HideInInspector]
    public Vector3 Direction
    {
        get
        {
            return new Vector3(
                Mathf.Cos((-directionDegrees + 90.0f) * Mathf.Deg2Rad),
                0.0f,
                Mathf.Sin((-directionDegrees + 90.0f) * Mathf.Deg2Rad)
            );
        }
    }

    void Awake()
    {
        instance = this;

        directionDegrees = UnityEngine.Random.Range(0.0f, 360.0f);
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(
            0.0f,
            directionDegrees
        ));
    }
}