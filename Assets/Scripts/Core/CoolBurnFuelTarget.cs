using System;
using System.Collections;
using UnityEngine;


public class CoolBurnFuelTarget : MonoBehaviour
{
    private FireObject FireObjectRef;
    [HideInInspector] public bool burning = false;
    private Coroutine fireExtinguisherCoroutine;
    private float BurnTimer;
    private float MaxBurnTime;
    [SerializeField] private GameObject FireParticlePrefab;
    [SerializeField] private GameObject FirePostivePrefab;
    private ParticleSystem firePS;
    private ParticleSystem postivePS;

    private Vector3 minFirePSScale;
    private Vector3 maxFirePSScale;

    private float minFireEmissionRate;
    private float maxFireEmissionRate;

    private Coroutine coolburnIntensifycoroutine;

    private float FireTimer;

    public void BeginFireIgnition(FireObject baseFireObject)
    {
        FireObjectRef = baseFireObject;
        burning = true;
        BurnTimer = 0f;
        FireTimer = Time.time + 3f;
        if (firePS == null) //Temp Fix If fire hasnt happend before
        {
            GameObject fireParticle = Instantiate(FireParticlePrefab, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            GameObject positiveParticle = Instantiate(FirePostivePrefab, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            firePS = fireParticle.GetComponent<ParticleSystem>();
            postivePS = positiveParticle.GetComponent<ParticleSystem>();
            Vector3 FireScale = Vector3.Lerp(new Vector3(0.8f, 1.3f, 0.5f), new Vector3(9.17f, 9.823f, 3f),
                FireObjectRef.fireIntensity / 200);
            var firePSEmission = firePS.emission;
            firePSEmission.rateOverTime = 200f;
            var positivePSEmission = postivePS.emission;
            positivePSEmission.rateOverTime = 15f;
            MaxBurnTime = Time.time;
        }
    }

    public void StoppingBurn()
    {
        burning = false;

        if (fireExtinguisherCoroutine != null) StopCoroutine(fireExtinguisherCoroutine);
        fireExtinguisherCoroutine = StartCoroutine(FireExtinguisher());
    }

    private IEnumerator FireExtinguisher()
    {
        //float delay = UnityEngine.Random.Range(8f, 18f);
        yield return new WaitForSeconds(1f);

        if (firePS != null && postivePS != null) //Temp fix so no errors when destorying it
        {
            FireManager.UpdateFireDangerLevel(false);
            Destroy(firePS.gameObject);
            Destroy(postivePS.gameObject);
        }

    }

    private void OnDestroy()
    {
        if (coolburnIntensifycoroutine != null)
        {
            StopCoroutine(coolburnIntensifycoroutine);
        }
    }

    private void Update()
    {
        FireTimer -= 1;
        if (FireTimer <= 0f)
        {
            if (burning)
            {
                CoolburnIntensifying(FireObjectRef.fireIntensity);
                AddScoreForFire(FireObjectRef.fireIntensity);
            }
        }
        
    }

    private void CoolburnIntensifying(float currentFireIntensity)
    {
        float currentIntensity = FireObjectRef.fireIntensity;
        var FirePSShape = firePS.shape;
        var FirePSEmission = firePS.emission;
        var PositvePSShape = postivePS.shape;
        var PositivePSEmission = postivePS.emission;
        minFirePSScale = new Vector3(0.5f, 0.8f, 0.5f);
        maxFirePSScale = new Vector3(7.17f, 7.823f, 2f);
        minFireEmissionRate = 100f;
        maxFireEmissionRate = 500f;
        Vector3 UpdatingIntensityScale =
           Vector3.Lerp(minFirePSScale, maxFirePSScale, currentIntensity / 200);

        float UpdatingFireEmission =
            Mathf.Lerp(minFireEmissionRate, maxFireEmissionRate, currentIntensity / 200);

        float minPositvePSEmission = 20f;
        float maxPositvePSEmission = 100f;
        float UpdatingPositveEmission =
            Mathf.Lerp(minPositvePSEmission, maxPositvePSEmission, currentIntensity / 200);
        if (BurnTimer < 0f)
        {
            if (currentIntensity < 100f)
            {

                Debug.Log("called");

                FirePSShape.scale = UpdatingIntensityScale;
                PositvePSShape.scale = UpdatingIntensityScale;
                FirePSEmission.rateOverTime = UpdatingFireEmission;
                PositivePSEmission.rateOverTime = UpdatingPositveEmission;
                //ScoreManager.instance.AddScore(1);
                MaxBurnTime = Time.time + 15f;
            }
            else if (currentIntensity < 200f)
            {


                FirePSShape.scale = UpdatingIntensityScale;
                PositvePSShape.scale = UpdatingIntensityScale;
                FirePSEmission.rateOverTime = UpdatingFireEmission;
                PositivePSEmission.rateOverTime = UpdatingPositveEmission;

               // ScoreManager.instance.AddScore(2);
                MaxBurnTime = Time.time + 15f;
            }
            else if (currentIntensity >= 200f)
            {
                MaxBurnTime -= 3f;
                if (MaxBurnTime <= 0f)
                {
                    ScoreManager.instance.AddScore(10);
                    FireManager.UpdateFireDangerLevel(false);
                    Destroy(gameObject);
                }
            }
        }
        
    }

    private void AddScoreForFire(float currentIntensity)
    {
        if (FireObjectRef.fireIntensity <= 100f)
        {
            ScoreManager.instance.AddScore(2);
        }
        else if (FireObjectRef.fireIntensity >= 200f)
        {
            ScoreManager.instance.AddScore(4);
        }
    }
}
