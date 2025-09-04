using UnityEngine;

// I was hoping to use generics to have a single "SpringDampener" class that
// would work with any math supporting data type (e.g. float, Vector2/3)
// but alas it does not seem to be possible, so instead we have this mess.

public class SpringDamperVector3
{
    public float spring;
    public float damp;
    public Vector3 targetVal;

    private float startDelay;
    private Vector3 velocity;

    public SpringDamperVector3(float spring, float damp, Vector3 targetVal, float startDelay = 0.0f)
    {
        this.spring = spring;
        this.damp = damp;
        this.targetVal = targetVal;
        this.startDelay = Time.time + startDelay;
    }

    // Needs to be called in the Update function of whatever script is using an instance of this class.
    // Example: value = springDampenerVar.Update(value);
    public Vector3 Update(Vector3 value)
    {
        if (Time.time < startDelay) return value;

        // Spring Damper
        // Now rewritten to ACTUALLY be frame independent
        velocity = Vector3.Lerp(
            velocity,
            (targetVal - value) * spring,
            1f - Mathf.Exp(-damp * Time.unscaledDeltaTime)
        );
        return value + (velocity * Time.unscaledDeltaTime);
    }
}

public class SpringDamperFloat
{
    public float spring;
    public float damp;
    public float targetVal;

    private float startDelay;
    private float velocity;

    public SpringDamperFloat(float spring, float damp, float targetVal, float startDelay = 0.0f)
    {
        this.spring = spring;
        this.damp = damp;
        this.targetVal = targetVal;
        this.startDelay = Time.time + startDelay;
    }

    // Needs to be called in the Update function of whatever script is using an instance of this class.
    public float Update(float value)
    {
        if (Time.unscaledTime < startDelay) return value;

        // Spring Damper
        velocity = Mathf.Lerp(
            velocity,
            (targetVal - value) * spring,
            1f - Mathf.Exp(-damp * Time.unscaledDeltaTime)
        );
        return value + (velocity * Time.unscaledDeltaTime);
    }
}