using UnityEngine;

public class LerpAnimationVector3
{
    public Vector3 targetVal;
    public float lerpSpeed;

    public LerpAnimationVector3(Vector3 targetVal, float lerpSpeed)
    {
        this.targetVal = targetVal;
        this.lerpSpeed = lerpSpeed;
    }

    public Vector3 Update(Vector3 value)
    {
        float blend = 1 - Mathf.Pow(0.5f, Time.deltaTime * lerpSpeed);
        return Vector3.Lerp(value, targetVal, blend);
    }
}

public class LerpAnimationColour
{
    public Color targetVal;
    public float lerpSpeed;

    public LerpAnimationColour(Color targetVal, float lerpSpeed)
    {
        this.targetVal = targetVal;
        this.lerpSpeed = lerpSpeed;
    }

    public Color Update(Color value)
    {
        float blend = 1 - Mathf.Pow(0.5f, Time.deltaTime * lerpSpeed);
        return Color.Lerp(value, targetVal, blend);
    }
}

public class LerpAnimationFloat
{
    public float targetVal;
    public float lerpSpeed;

    public LerpAnimationFloat(float targetVal, float lerpSpeed)
    {
        this.targetVal = targetVal;
        this.lerpSpeed = lerpSpeed;
    }

    public float Update(float value)
    {
        float blend = 1 - Mathf.Pow(0.5f, Time.deltaTime * lerpSpeed);
        return Mathf.Lerp(value, targetVal, blend);
    }
}