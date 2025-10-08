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

    public void BeginFireIgnition(FireObject baseFireObject)
    {
        FireObjectRef = baseFireObject;
        burning = true;
        BurnTimer = 0f;
        if (firePS == null)
        {
            GameObject fireParticle = Instantiate(FireParticlePrefab, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            GameObject positiveParticle = Instantiate(FirePostivePrefab, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            firePS = fireParticle.GetComponent<ParticleSystem>();
            postivePS = positiveParticle.GetComponent<ParticleSystem>();
        }
        else
        {
            var firePSEmission = firePS.emission;
            firePSEmission.rateOverTime = 400f;
            var positivePSEmission = postivePS.emission;
            positivePSEmission.rateOverTime = 10f;
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

        if (firePS != null && postivePS != null)
        {
            var firePSEmission = firePS.emission;
            firePSEmission.rateOverTime = 0f;
            var positivePSEmission = postivePS.emission;
            positivePSEmission.rateOverTime = 0f;
            FireManager.UpdateFireDangerLevel(false);
        }

    }


    // Update is called once per frame
    void Update()
    {
        //End if not currently Burning
        if (burning)
        {
            FireIntensifys(FireObjectRef.fireIntensity);
        }
    }

    void FireIntensifys(float CurrentIntensity)
    {
        float currentIntensity = FireObjectRef.fireIntensity;
        var FirePSShape = firePS.shape;
        var FirePSEmission = firePS.emission;
        var PositvePSShape = postivePS.shape;
        var PositivePSEmission = postivePS.emission;

        if (currentIntensity < 100f)
        {
            Vector3 SmallFireScale = Vector3.Lerp(new Vector3(0.8f, 1.3f, 0.5f), new Vector3(2f, 2.5f, 0.8f),
                (currentIntensity - 100) / 100f);

            FirePSShape.scale = SmallFireScale;
            PositvePSShape.scale = SmallFireScale;
            FirePSEmission.rateOverTime = Mathf.Lerp(300f, 400f, currentIntensity / 100f);
            PositivePSEmission.rateOverTime = Mathf.Lerp(10f, 20f, currentIntensity / 100f);
            BurnTimer = 0f;
            MaxBurnTime = 0f;
        }
        else if (currentIntensity >= 100f && currentIntensity < 200f)
        {
            BurnTimer += Time.deltaTime;
            Vector3 MedFireScale = Vector3.Lerp(new Vector3(3.1f, 3.5f, 2.7f), new Vector3(4.7f, 5f, 4.5f),
                (currentIntensity - 100) / 100f);

            FirePSShape.scale = MedFireScale;
            PositvePSShape.scale = MedFireScale;
            FirePSEmission.rateOverTime = Mathf.Lerp(400f, 500f, (currentIntensity - 100f) / 100f);
            PositivePSEmission.rateOverTime = Mathf.Lerp(20f, 40f, (currentIntensity - 100f) / 100f);
            Debug.Log("Do This Other Thing");
            ScoreManager.instance.AddScore(2);
            MaxBurnTime = 0f;

        }
        else if (currentIntensity >= 200f && BurnTimer >= 30f)
        {
            Vector3 BigFireScale = Vector3.Lerp(new Vector3(5.3f, 5.8f, 4.5f), new Vector3(9.17f, 9.823f, 3f),
                BurnTimer / 100f);

            FirePSShape.scale = BigFireScale;
            PositvePSShape.scale = BigFireScale;
            FirePSEmission.rateOverTime = Mathf.Lerp(500f, 550f, BurnTimer / 30f);
            PositivePSEmission.rateOverTime = Mathf.Lerp(40f, 50f, BurnTimer / 30f);
            ScoreManager.instance.AddScore(-3);
            MaxBurnTime += Time.deltaTime;
            Debug.Log("Burnt");

        }
        else if (currentIntensity >= 200f && MaxBurnTime > 20f)
        {
            ScoreManager.instance.AddScore(-5);
            FireManager.UpdateFireDangerLevel(false);
            Destroy(gameObject);
        }
    }
}
