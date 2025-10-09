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

    private Coroutine coolburnIntensifycoroutine;
    public void BeginFireIgnition(FireObject baseFireObject)
    {
        FireObjectRef = baseFireObject;
        burning = true;
        BurnTimer = 0f;
        if (firePS == null) //Temp Fix If fire hasnt happend before
        {
            GameObject fireParticle = Instantiate(FireParticlePrefab, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            GameObject positiveParticle = Instantiate(FirePostivePrefab, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            firePS = fireParticle.GetComponent<ParticleSystem>();
            postivePS = positiveParticle.GetComponent<ParticleSystem>();
            Vector3 FireScale = Vector3.Lerp(new Vector3(0.8f, 1.3f, 0.5f), new Vector3(9.17f, 9.823f, 3f),
                FireObjectRef.fireIntensity/200);
            var firePSEmission = firePS.emission;
            firePSEmission.rateOverTime = 1f;
            var positivePSEmission = postivePS.emission;
            positivePSEmission.rateOverTime = 1f;
        }
     
        if (coolburnIntensifycoroutine != null)
        {
            coolburnIntensifycoroutine = StartCoroutine(CoolburnFireIntensifys(FireObjectRef.fireIntensity));
        }
        if(burning == false) StopCoroutine(coolburnIntensifycoroutine);
        

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

    private IEnumerator CoolburnFireIntensifys(float currentFireIntensity)
    {
        float currentIntensity = FireObjectRef.fireIntensity;
        var FirePSShape = firePS.shape;
        var FirePSEmission = firePS.emission;
        var PositvePSShape = postivePS.shape;
        var PositivePSEmission = postivePS.emission;

        if (currentIntensity < 100f)
        {
            float smallT = currentIntensity / 100;

            Vector3 SmallFireScale = Vector3.Lerp(new Vector3(0.8f, 1.3f, 0.5f), new Vector3(2f, 2.5f, 0.8f),
                smallT);

            FirePSShape.scale = SmallFireScale;
            PositvePSShape.scale = SmallFireScale;
            FirePSEmission.rateOverTime = Mathf.Lerp(0f, 60f, smallT);
            PositivePSEmission.rateOverTime = Mathf.Lerp(10f, 20f, smallT);
            BurnTimer = 0f;
            MaxBurnTime = 0f;
        }
        else if (currentIntensity < 200f)
        {
            float medT = (currentIntensity - 100) / 100;
            Vector3 MedFireScale = Vector3.Lerp(new Vector3(3.1f, 3.5f, 2.7f), new Vector3(4.7f, 5f, 4.5f),
                medT);

            FirePSShape.scale = MedFireScale;
            PositvePSShape.scale = MedFireScale;
            FirePSEmission.rateOverTime = Mathf.Lerp(60f, 160f, medT);
            PositivePSEmission.rateOverTime = Mathf.Lerp(20f, 50f, medT);

            ScoreManager.instance.AddScore(2);
            MaxBurnTime = 0f;

        }
        else if (currentIntensity >= 200f)
        {
            MaxBurnTime += 3;
            if (BurnTimer >= 30f)
            {
                float bigT = BurnTimer / 30f;

                Vector3 BigFireScale = Vector3.Lerp(new Vector3(5.3f, 5.8f, 4.5f), new Vector3(9.17f, 9.823f, 3f),
                    bigT);

                FirePSShape.scale = BigFireScale;
                PositvePSShape.scale = BigFireScale;
                FirePSEmission.rateOverTime = Mathf.Lerp(160f, 200f, bigT);
                PositivePSEmission.rateOverTime = Mathf.Lerp(40f, 50f, bigT);
                ScoreManager.instance.AddScore(1);
                MaxBurnTime += 3;
                Debug.Log("Burnt");
            }
            else if (MaxBurnTime >= 40f)
            {
                ScoreManager.instance.AddScore(10);
                FireManager.UpdateFireDangerLevel(false);
                Destroy(gameObject);
            }


        }
        yield return new WaitForSeconds(3f);
    }

   
}
