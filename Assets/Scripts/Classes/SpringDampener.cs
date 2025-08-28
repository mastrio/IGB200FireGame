using System;
using UnityEngine;

public class SpringDampenerVector3
{
    public float spring;
    public float damp;
    public Vector3 targetVal;

    private float startTime;
    private Vector3 velocity;

    public SpringDampenerVector3(float spring, float damp, Vector3 targetVal, float startDelay = 0.0f)
    {
        this.spring = spring;
        this.damp = damp;
        this.targetVal = targetVal;
        this.startTime = Time.time + startDelay;
    }

    public Vector3 Update(Vector3 value)
    {
        if (Time.time < startTime) return value;

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

    private float startTime;
    private float velocity;

    public SpringDampenerFloat(float spring, float damp, float targetVal, float startDelay = 0.0f)
    {
        this.spring = spring;
        this.damp = damp;
        this.targetVal = targetVal;
        this.startTime = Time.time + startDelay;
    }

    public float Update(float value)
    {
        if (Time.time < startTime) return value;

        // Spring Dampener
        velocity += (targetVal - value) * spring;
        velocity -= velocity * damp;
        return value + velocity;
    }
}

