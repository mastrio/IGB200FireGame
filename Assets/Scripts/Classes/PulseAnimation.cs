using UnityEngine;

public class PulseAnimationVector3
{
    public Vector3 baseValue;
    public Vector3 pulsedValue;
    public float pulseSpeed;

    private LerpAnimationVector3 scaleAnim;
    private LerpAnimationVector3 targetAnim;
    private Vector3 targetPulsedValue;

    public PulseAnimationVector3(Vector3 baseValue, Vector3 pulsedValue, float pulseSpeed)
    {
        this.baseValue = baseValue;
        this.pulsedValue = pulsedValue;
        this.pulseSpeed = pulseSpeed;

        targetPulsedValue = baseValue;
        scaleAnim = new LerpAnimationVector3(baseValue, pulseSpeed);
        targetAnim = new LerpAnimationVector3(baseValue, pulseSpeed * 0.25f);
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
        targetPulsedValue = pulsedValue;
        scaleAnim = new LerpAnimationVector3(pulsedValue, pulseSpeed);
    }
}
