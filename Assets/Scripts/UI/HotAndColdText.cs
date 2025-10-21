using UnityEngine;

public class HotAndColdText : MonoBehaviour
{
    [SerializeField] private float sharpShakeAmount;
    [SerializeField] private float softShakeAmount;

    private ObjectShaker objectShaker;

    void Start()
    {
        objectShaker = GetComponent<ObjectShaker>();
    }

    void Update()
    {
        objectShaker.SetSharpShake(sharpShakeAmount);
        objectShaker.SetSoftShake(softShakeAmount);

        transform.localScale = new Vector3(
            1.0f + Mathf.Sin(Time.time * 3.0f) * 0.1f,
            1.0f + Mathf.Sin(Time.time * 3.0f) * 0.1f,
            1.0f
        );
    }
}
