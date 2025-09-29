using UnityEngine;

public class ObjectShaker : MonoBehaviour
{
    [Header("Sharp Shake")]
    [SerializeField] private float sharpShakeReductionSpeed = 10.0f;
    [SerializeField] private float sharpShakeTimeDelay = 0.0f;

    [Header("Soft Shake")]
    [SerializeField] private float softShakeReductionSpeed = 5.0f;

    private float sharpShakeAmount = 0.0f;
    private float softShakeAmount = 0.0f;

    private Vector3 sharpShakeOffset = Vector3.zero;
    private Vector3 softShakeOffset = Vector3.zero;

    private float sharpShakeTimer = 0.0f;

    void Update()
    {
        HandleSharpShake();
        HandleSoftShake();

        transform.localPosition = sharpShakeOffset + softShakeOffset;
    }

    void HandleSharpShake()
    {
        float blend = 1 - Mathf.Pow(0.5f, Time.deltaTime * sharpShakeReductionSpeed);
        sharpShakeAmount = Mathf.Lerp(sharpShakeAmount, 0.0f, blend);

        if (Time.time >= sharpShakeTimer)
        {
            sharpShakeTimer = Time.time + sharpShakeTimeDelay;

            sharpShakeOffset = new Vector3(
                Random.Range(-sharpShakeAmount, sharpShakeAmount),
                Random.Range(-sharpShakeAmount, sharpShakeAmount),
                0.0f
            );
        }
    }

    void HandleSoftShake()
    {
        float blend = 1 - Mathf.Pow(0.5f, Time.deltaTime * softShakeReductionSpeed);
        softShakeAmount = Mathf.Lerp(softShakeAmount, 0.0f, blend);

        Vector3 offsetTarget = new Vector3(
            Random.Range(-softShakeAmount, softShakeAmount),
            Random.Range(-softShakeAmount, softShakeAmount),
            0.0f
        );

        softShakeOffset = Vector3.Lerp(softShakeOffset, offsetTarget, blend);
    }

    public void ApplySharpShake(float amount)
    {
        sharpShakeAmount += amount;
    }

    public void SetSharpShake(float amount)
    {
        sharpShakeAmount = amount;
    }

    public void ApplySoftShake(float amount)
    {
        softShakeAmount += amount * 2.0f;
    }

    public void SetSoftShake(float amount)
    {
        softShakeAmount = amount * 2.0f;
    }
}