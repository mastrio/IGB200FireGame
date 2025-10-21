using System.Collections;
using UnityEngine;


public class CoolBurnFuelTarget : MonoBehaviour
{
    private FireObject FireObjectRef;
    [HideInInspector] public bool burning = false;
    private Coroutine fireExtinguisherCoroutine;
    private float BurnTimer = 0f;
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

    private float tempFireTimer = 1f;
    private float FireCounterTime = 1f;

    private float currentIntensity;


    public void BeginFireIgnition(FireObject baseFireObject)
    {
        FireObjectRef = baseFireObject;
        burning = true;
        if (firePS == null) //Temp Fix If fire hasnt happend before
        {
            GameObject fireParticle = Instantiate(FireParticlePrefab, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            GameObject positiveParticle = Instantiate(FirePostivePrefab, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            firePS = fireParticle.GetComponent<ParticleSystem>();
            postivePS = positiveParticle.GetComponent<ParticleSystem>();
            Vector3 FireScale = Vector3.Lerp(new Vector3(0.5f, 0.8f, 0.5f), new Vector3(4.17f, 4.823f, 1.4f),
                FireObjectRef.fireIntensity / 200f);
            Vector3 PositveScale = Vector3.Lerp(new Vector3(0.5f, 0.8f, 0.5f), new Vector3(7f, 7f, 4f),
                FireObjectRef.fireIntensity / 200f);
            var firePSScale = firePS.shape;
            var postivePSScale = postivePS.shape;
            firePSScale.scale = FireScale;
            postivePSScale.scale = PositveScale;

            var firePSEmission = firePS.emission;
            firePSEmission.rateOverTime = Mathf.Lerp(50f, 200f, FireObjectRef.fireIntensity / 200f);
            var positivePSEmission = postivePS.emission;
            positivePSEmission.rateOverTime = Mathf.Lerp(10f, 20f, FireObjectRef.fireIntensity / 200f);
            MaxBurnTime = Time.time + 10f;
        }
        else if (firePS != null)
        {
            Vector3 FireScale = Vector3.Lerp(new Vector3(0.5f, 0.8f, 0.5f), new Vector3(4.17f, 4.823f, 1.4f),
                FireObjectRef.fireIntensity / 200f);
            Vector3 PositveScale = Vector3.Lerp(new Vector3(0.5f, 0.8f, 0.5f), new Vector3(12f, 12f, 5f),
                FireObjectRef.fireIntensity / 200f);
            var firePSScale = firePS.shape;
            var postivePSScale = postivePS.shape;
            firePSScale.scale = FireScale;
            postivePSScale.scale = PositveScale;

            var firePSEmission = firePS.emission;
            firePSEmission.rateOverTime = Mathf.Lerp(100f, 300f, FireObjectRef.fireIntensity / 200f);
            var positivePSEmission = postivePS.emission;
            positivePSEmission.rateOverTime = Mathf.Lerp(10f, 20f, FireObjectRef.fireIntensity / 200f);
        }
    }

    public void StoppingBurn()
    {
        burning = false;

        if (fireExtinguisherCoroutine != null) StopCoroutine(fireExtinguisherCoroutine);

        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        fireExtinguisherCoroutine = StartCoroutine(FireExtinguisher());
    }

    private IEnumerator FireExtinguisher()
    {
        //float delay = UnityEngine.Random.Range(8f, 18f);
        yield return new WaitForSeconds(1f);

        if (firePS != null && postivePS != null) //Temp fix so no errors when destorying it
        {

            Destroy(firePS.gameObject);
            Destroy(postivePS.gameObject);
            firePS = null;
            postivePS = null;
        }

    }

    private void OnDestroy()
    {
        FireManager.instance.UpdateFireDangerLevel();
        StoppingBurn();
    }

    private void Update()
    {

        if (FireObjectRef == null)
        {
            return;
        }

        if (burning)
        {
            CoolburnIntensifying(FireObjectRef.fireIntensity);

            if (FireObjectRef.fireIntensity > 70f && FireObjectRef.fireIntensity <= 130f)
            {
                BurnTimer += Time.deltaTime;
                Debug.Log(BurnTimer);

                if (BurnTimer >= 5f)
                {
                    burning = false;
                    ScoreManager.instance.AddScore(1);
                    ScoreManager.instance.scorePositiveParticles.Play(true);
                    Destroy(gameObject);
                }
            }
            else if (FireObjectRef.fireIntensity > 130f)
            {
                BurnTimer += Time.deltaTime;
                Debug.Log(BurnTimer);
                if (BurnTimer >= 5f)
                {

                    //Ember Particles
                    burning = false;
                    Destroy(gameObject);
                }
            }
            else
            {
                BurnTimer = 0f;
            }

            AddScoreForFire(FireObjectRef.fireIntensity);

        }
        else
        {
            BurnTimer = 0f;
        }
    }


        //if (burning & tempFireTimer <= 0f)
        // {

        // tempFireTimer = 1f;

        //else if (burning)
        //       {
        //         tempFireTimer -= Time.deltaTime;
        //   }


    private void CoolburnIntensifying(float FireIntensity)
    {
        if (firePS == null || postivePS == null)
        {
            return;
        }
        else if (FireIntensity == currentIntensity)
        {
            return;
        }
        currentIntensity = FireIntensity;
        var FirePSShape = firePS.shape;
        var FirePSEmission = firePS.emission;
        var PositvePSShape = postivePS.shape;
        var PositivePSEmission = postivePS.emission;
        minFirePSScale = new Vector3(0.5f, 0.8f, 0.5f);
        maxFirePSScale = new Vector3(4.17f, 4.823f, 1.4f);
        minFireEmissionRate = 100f;
        maxFireEmissionRate = 250f;
        Vector3 UpdatingIntensityScale =
           Vector3.Lerp(minFirePSScale, maxFirePSScale, currentIntensity / 200);

        Vector3 PositveScale = Vector3.Lerp(new Vector3(0.5f, 0.8f, 0.5f), new Vector3(7f, 7f, 4f),
            FireObjectRef.fireIntensity / 200f);

        float UpdatingFireEmission =
            Mathf.Lerp(minFireEmissionRate, maxFireEmissionRate, currentIntensity / 200);

        float minPositvePSEmission = 10f;
        float maxPositvePSEmission = 20f;
        float UpdatingPositveEmission =
            Mathf.Lerp(minPositvePSEmission, maxPositvePSEmission, currentIntensity / 200);

        FirePSShape.scale = UpdatingIntensityScale;
        PositvePSShape.scale = PositveScale;
        FirePSEmission.rateOverTime = UpdatingFireEmission;
        PositivePSEmission.rateOverTime = UpdatingPositveEmission;

        if (currentIntensity <= 0)
        {
            burning = false;
            Destroy(firePS.gameObject);
            Destroy(postivePS.gameObject);
            firePS = null;
            postivePS = null;
        }
        else if (currentIntensity <= 70f)
        {
            ScoreManager.instance.scorePositiveParticles.Play(true);
        }
    }

    private void AddScoreForFire(float currentIntensity)
    {

        if (currentIntensity <= 0f)
        {
            return;
        }
        else if (FireObjectRef.fireIntensity <= 70f)
        {
            ScoreManager.instance.AddScore(0.1f);


        }
        else if (FireObjectRef.fireIntensity <= 130f)
        {
            ScoreManager.instance.AddScore(0.2f);


        }


    }
}
