using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class HighFuel : MonoBehaviour
{
    private FireObject BaseFireObjectRef;
    [HideInInspector] public bool burning = false;
    private Coroutine fireExtinguisherCoroutine;
    private float BurnTimer;
    private float MaxBurnTime = 0f;
    [SerializeField] private GameObject FireParticlePrefab;
    [SerializeField] private GameObject FireNegativePrefab;
    [SerializeField] private GameObject AshPileObject;
    [SerializeField] private GameObject ashDestructionParticleGameObject;

    private ParticleSystem firePS;
    private ParticleSystem negativePS;
    private ParticleSystem ashPS;

    private float fireIntensity;

    private Coroutine HighFuelBurningCoroutine;
    public void BeginHighFuelFire(FireObject baseFireObject)
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

        /*if (enviroIntensiftyCoroutine != null)
        {
            enviroIntensiftyCoroutine = StartCoroutine(EnviroFireIntensifys(BaseFireObjectRef.fireIntensity));
        }
        if (burning == false) StopCoroutine(enviroIntensiftyCoroutine);*/

        // FireManager.instance.Update
        // (true); //Increase by one as the fire spread to something unintended
    }

    public void StoppingHighFuelBurn()
    {
        burning = false;

        if (fireExtinguisherCoroutine != null) StopCoroutine(fireExtinguisherCoroutine);

        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        fireExtinguisherCoroutine = StartCoroutine(SpreadFireExtinguisher());
    }

    private IEnumerator SpreadFireExtinguisher()
    {
        //float delay = UnityEngine.Random.Range(15f, 20f);
        yield return new WaitForSeconds(4f);

        if (firePS != null && negativePS != null) //Temp fix to stop errors
        {
            Destroy(firePS.gameObject);
            Destroy(negativePS.gameObject);
            firePS = null;
            negativePS = null;
        }

    }

    private void Update()
    {
        if (BaseFireObjectRef != null)
        {
            if (burning)
            {
                BurnTimer += Time.deltaTime;
                HighFuelBurning(BaseFireObjectRef.fireIntensity);
            }
            else
            {
                BurnTimer = 0f;
            }

        }

    }

    private void HighFuelBurning(float currentFireIntensity)
    {
        if (firePS == null || negativePS == null)
        {
            return;
        }
        else if (fireIntensity == currentFireIntensity)
        {
            return;
        }

        fireIntensity = currentFireIntensity;

        var FirePSShape = firePS.shape;
        var FirePSEmission = firePS.emission;
        var NegPSShape = negativePS.shape;
        var NegPSEmission = negativePS.emission;
        Vector3 minFirePsScale = new Vector3(1.8f, 2.3f, 0.7f);
        Vector3 maxFirePsScale = new Vector3(8.17f, 8.823f, 2.1f);
        float minFirePSEmission = 200f;
        float maxFirePSEmission = 550f;
        Vector3 UpdatingIntensityScale =
            Vector3.Lerp(minFirePsScale, maxFirePsScale, fireIntensity / 200);
        float UpdatingFireEmission =
            Mathf.Lerp(minFirePSEmission, maxFirePSEmission, fireIntensity / 200);


        float minNegPSEmission = 20f;
        float maxNegPSEmission = 60f;
        float UpdatingNegEmission =
            Mathf.Lerp(minNegPSEmission, maxNegPSEmission, fireIntensity / 200);

        FirePSShape.scale = UpdatingIntensityScale;
        NegPSShape.scale = UpdatingIntensityScale;
        FirePSEmission.rateOverTime = UpdatingFireEmission;
        NegPSEmission.rateOverTime = UpdatingNegEmission;

        if (fireIntensity <= 0f)
        {
            burning = false;
            BurnTimer = 0f;
            Destroy(firePS.gameObject);
            Destroy(negativePS.gameObject);
            firePS = null;
            negativePS = null;
        }
        else if (fireIntensity <= 130f)
        {
            BurnTimer = 0f;
        }
        else if (fireIntensity > 130f && fireIntensity < 200f)
        {

            if (BurnTimer >= 10f)
            {
                Debug.Log(BurnTimer);
                //Ember Particles
                burning = false;
                Instantiate(ashDestructionParticleGameObject,
                    new Vector3(transform.position.x, 1f, transform.position.z), transform.rotation);
                Instantiate(AshPileObject, new Vector3(transform.position.x, 0f, transform.position.z),
                    transform.rotation);
                Destroy(gameObject);
            }
        }
        else if (fireIntensity >= 200f) //Testing
        {
            Debug.Log(BurnTimer);
            //Ember Particles
            burning = false;

            Instantiate(ashDestructionParticleGameObject, new Vector3(transform.position.x, 1f, transform.position.z),
                transform.rotation);
            Instantiate(AshPileObject, new Vector3(transform.position.x, 0f, transform.position.z), transform.rotation);
            Destroy(gameObject);
        }
    }
}