using UnityEngine;

public class WindManager : MonoBehaviour
{
    private static int DIRECTION_CHANGE_DELAY = 30; // Time in seconds

    [HideInInspector] public static WindManager instance;

    [SerializeField] private GameObject windParticles;

    public float directionDegrees;

    private float targetDirection;
    private int counter;

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
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(
            0.0f,
            directionDegrees
        ));
    }

    void FixedUpdate()
    {
        counter--;
        if (counter <= 0)
        {
            counter = DIRECTION_CHANGE_DELAY * 60;

            ChangeTargetDirection();
        }

        directionDegrees = Mathf.MoveTowards(directionDegrees, targetDirection, 0.2f);
    }

    private void ChangeTargetDirection()
    {
        targetDirection = Random.Range(0.0f, 360.0f);
    }
}