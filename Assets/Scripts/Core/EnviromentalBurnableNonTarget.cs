using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class EnviromentalBurnableNonTarget : MonoBehaviour
{
    private FireObject BaseFireObjectRef;
    [HideInInspector] public bool burning = false;
    private Coroutine fireExtinguisherCoroutine;
    private float BurnTimer;
    private float MaxBurnTime = 0f;
    [SerializeField] private GameObject FireParticlePrefab;
    [SerializeField] private GameObject FireNegativePrefab;
    private ParticleSystem firePS;
    private ParticleSystem negativePS;

    public void BeginSpreadFire(FireObject baseFireObject)
    {
        BaseFireObjectRef = baseFireObject;
        burning = true;
        BurnTimer = 0f;
        if (firePS == null) //Temp Fix 
        {
            GameObject fireParticle = Instantiate(FireParticlePrefab, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            GameObject negativeParticle = Instantiate(FireNegativePrefab, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            firePS = fireParticle.GetComponent<ParticleSystem>();
            negativePS = negativeParticle.GetComponent<ParticleSystem>();
        }
        else // Temp fix
        {
            var firePSEmission = firePS.emission;
            firePSEmission.rateOverTime = 400f;
            var negPSEmission = negativePS.emission;
            negPSEmission.rateOverTime = 10f;
        }

            FireManager.UpdateFireDangerLevel(true); //Increase by one as the fire spread to something unintended
    }

    public void StoppingBurn()
    {
        burning = false;

        if (fireExtinguisherCoroutine != null) StopCoroutine(fireExtinguisherCoroutine);
        fireExtinguisherCoroutine = StartCoroutine(SpreadFireExtinguisher());
    }

    private IEnumerator SpreadFireExtinguisher()
    {
        //float delay = UnityEngine.Random.Range(15f, 20f);
        yield return new WaitForSeconds(1f);

        if (firePS != null && negativePS != null) //Temp fix to stop errors
        {
            var firePSEmission = firePS.emission;
            firePSEmission.rateOverTime = 0f;
            var negPSEmission = negativePS.emission;
            negPSEmission.rateOverTime = 0f;
            //Reduce By one
            FireManager.UpdateFireDangerLevel(false);
        }

    }


    // Update is called once per frame
    void Update()
    {
        //End if not currently Burning
        if (burning)
        {
            BurningEnviroment(BaseFireObjectRef.fireIntensity);
        }

    }

    private void BurningEnviroment(float currentIntensity)
    {
        currentIntensity = BaseFireObjectRef.fireIntensity;

        var FirePSShape = firePS.shape;
        var FirePSEmission = firePS.emission;
        var NegPSShape = negativePS.shape;
        var NegPSEmission = negativePS.emission;
        if (currentIntensity < 100f)
        {
            Vector3 SmallFireScale = Vector3.Lerp(new Vector3(0.8f, 1.3f, 0.5f), new Vector3(2f, 2.5f, 0.8f),
                (currentIntensity-100) / 100f);

            FirePSShape.scale = SmallFireScale;
            NegPSShape.scale = SmallFireScale;
            FirePSEmission.rateOverTime = Mathf.Lerp(300f, 400f, currentIntensity / 100f);
            NegPSEmission.rateOverTime = Mathf.Lerp(10f, 20f, currentIntensity / 100f);
            BurnTimer = 0f;
            MaxBurnTime = 0f;

        }
        else if (currentIntensity > 100f && currentIntensity <= 200f)
        {
            BurnTimer += Time.deltaTime;
            Vector3 MedFireScale = Vector3.Lerp(new Vector3(3.1f, 3.5f, 2.7f), new Vector3(4.7f, 5f, 4.5f),
                (currentIntensity-100) / 100f);
           
           FirePSShape.scale = MedFireScale;
               NegPSShape.scale = MedFireScale;
               FirePSEmission.rateOverTime = Mathf.Lerp(400f, 500f, (currentIntensity -100f) / 100f);
               NegPSEmission.rateOverTime = Mathf.Lerp(20f, 40f, (currentIntensity - 100f) / 100f);
            Debug.Log("Do This Other Thing");
            MaxBurnTime = 0f;
            

        }
        else if (currentIntensity >= 200f && BurnTimer >= 30f)
        {
            Vector3 BigFireScale = Vector3.Lerp(new Vector3(5.3f, 5.8f, 4.5f), new Vector3(9.17f, 9.823f, 3f),
                BurnTimer / 100f);
     
            FirePSShape.scale = BigFireScale;
            NegPSShape.scale = BigFireScale;
            FirePSEmission.rateOverTime = Mathf.Lerp(500f, 550f, BurnTimer / 30f);
            NegPSEmission.rateOverTime = Mathf.Lerp(40f, 50f, BurnTimer / 30f);
            Debug.Log("Burnt");
            ScoreManager.instance.AddScore(-10);
            MaxBurnTime += Time.deltaTime;
        }
        else if (currentIntensity >= 200f && MaxBurnTime > 20f)
        {
            ScoreManager.instance.AddScore(-15);
            //Should reduce the fire danger level by one since its being burnt (back to how it was before unintended got set on fire)
            FireManager.UpdateFireDangerLevel(false); 
            Destroy(gameObject);
        }
    }
}
