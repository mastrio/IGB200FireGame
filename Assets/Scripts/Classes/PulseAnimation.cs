using UnityEngine;

public class PulseAnimationVector3
{
    public Vector3 baseValue;
    public Vector3 pulsedValue;
    public Vector3 maxPulseValue;
    public float pulseSpeed;

    private LerpAnimationVector3 scaleAnim;
    private LerpAnimationVector3 targetAnim;
    private Vector3 targetPulsedValue;

    public PulseAnimationVector3(Vector3 baseValue, Vector3 pulsedValue, Vector3 maxPulseValue, float pulseSpeed)
    {
        this.baseValue = baseValue;
        this.pulsedValue = pulsedValue;
        this.maxPulseValue = maxPulseValue;
        this.pulseSpeed = pulseSpeed;

        targetPulsedValue = baseValue;
        scaleAnim = new LerpAnimationVector3(baseValue, pulseSpeed);
        targetAnim = new LerpAnimationVector3(baseValue, pulseSpeed * 0.1f);
    }

    public Vector3 Update(Vector3 value)
    {
        if (scaleAnim == null) return value;

        targetPulsedValue = targetAnim.Update(targetPulsedValue);
        scaleAnim.targetVal = targetPulsedValue;

        return scaleAnim.Update(value);
    }

    public void Pulse()
    {
        targetPulsedValue += pulsedValue;

        if (targetPulsedValue.x > maxPulseValue.x) targetPulsedValue.x = maxPulseValue.x;
        if (targetPulsedValue.y > maxPulseValue.y) targetPulsedValue.y = maxPulseValue.y;
        if (targetPulsedValue.z > maxPulseValue.z) targetPulsedValue.z = maxPulseValue.z;

        scaleAnim = new LerpAnimationVector3(targetPulsedValue, pulseSpeed);
    }
}
