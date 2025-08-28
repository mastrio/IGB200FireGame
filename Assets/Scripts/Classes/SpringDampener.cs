using System;
using UnityEngine;

// I was hoping to use generics to have a single "SpringDampener" class that
// would work with any math supporting data type (e.g. float, Vector2/3)
// but alas it does not seem to be possible, so instead we have this mess.

public class SpringDampenerVector3
{
    public float spring;
    public float damp;
    public Vector3 targetVal;

    private float startDelay;
    private Vector3 velocity;

    public SpringDampenerVector3(float spring, float damp, Vector3 targetVal, float startDelay = 0.0f)
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

        // Spring Dampener
        velocity += (targetVal - value) * (spring * Time.deltaTime);
        velocity -= velocity * (damp * Time.deltaTime);
        return value + velocity;
    }
}

public class SpringDampenerFloat
{
    public float spring;
    public float damp;
    public float targetVal;

    private float startDelay;
    private float velocity;

    public SpringDampenerFloat(float spring, float damp, float targetVal, float startDelay = 0.0f)
    {
        this.spring = spring;
        this.damp = damp;
        this.targetVal = targetVal;
        this.startDelay = Time.time + startDelay;
    }

    // Needs to be called in the Update function of whatever script is using an instance of this class.
    public float Update(float value)
    {
        if (Time.time < startDelay) return value;

        // Spring Dampener
        velocity += (targetVal - value) * (spring * Time.deltaTime);
        velocity -= velocity * (damp * Time.deltaTime);
        return value + velocity;
    }
}

