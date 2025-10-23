using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class EnviromentalBurnableNonTarget : MonoBehaviour
{
    private FireObject BaseFireObjectRef;
    [HideInInspector] public bool burning = false;
    private Coroutine fireExtinguisherCoroutine;
    private float BurnTimer;
    [SerializeField] private GameObject FireParticlePrefab;
    [SerializeField] private GameObject FireNegativePrefab;
    [SerializeField] private GameObject AshPileObject;
    [SerializeField] private GameObject ashDestructionParticleGameObject;

    private ParticleSystem firePS;
    private ParticleSystem negativePS;

    private float fireIntensity;

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
    }

    public void StoppingBurn()
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
        yield return new WaitForSeconds(1f);

        if (firePS != null && negativePS != null) //Temp fix to stop errors
        {
            Destroy(firePS.gameObject);
            Destroy(negativePS.gameObject);
            firePS = null;
            negativePS = null;
        }

    }

    private void OnDestroy()
    {
        RuntimeNavMesh.doRebuildNavmesh = true;
    }

    private void Update()
    {
        if (BaseFireObjectRef == null)
        {
            return;
        }

        if (burning)
        {
            EnviromentIntensifying(BaseFireObjectRef.fireIntensity);
            if (BaseFireObjectRef.fireIntensity > 130f && BaseFireObjectRef.fireIntensity < 200f)
            {
                BurnTimer += Time.deltaTime;
                if (BurnTimer >= 10f)
                {
                    burning = false;
                    Instantiate(ashDestructionParticleGameObject, new Vector3(transform.position.x, 1f, transform.position.z), transform.rotation);
                    Instantiate(AshPileObject, new Vector3(transform.position.x, 0f, transform.position.z), transform.rotation);
                    Destroy(gameObject);
                }
            }
            else if (BaseFireObjectRef.fireIntensity >= 200f)
            {
                BurnTimer += Time.deltaTime;
                if (BurnTimer >= 3f)
                {
                    burning = false;
                    Instantiate(ashDestructionParticleGameObject, new Vector3(transform.position.x, 1f, transform.position.z), transform.rotation);
                    Instantiate(AshPileObject, new Vector3(transform.position.x, 0f, transform.position.z), transform.rotation);
                    Destroy(gameObject);
                }
            }
            else
            {
                BurnTimer = 0f;
            }
        }
        else
        {
            BurnTimer = 0f;
        }


    }

    private void EnviromentIntensifying(float currentFireIntensity)
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

        //Change the min and max fires scales here (lerps across from min at low intensity up to max at high) 
        Vector3 minFirePsScale = new Vector3(0.8f, 1.3f, 0.5f);
        Vector3 maxFirePsScale = new Vector3(5.17f, 5.823f, 1.5f);
        //Fires rate over time valyes change here 
        float minFirePSEmission = 50f;
        float maxFirePSEmission = 200f;
        Vector3 UpdatingIntensityScale =
            Vector3.Lerp(minFirePsScale, maxFirePsScale, fireIntensity / 200);
        float UpdatingFireEmission =
            Mathf.Lerp(minFirePSEmission, maxFirePSEmission, fireIntensity / 200);

        //Negative particles rate overtime variables change here 
        float minNegPSEmission = 1f;
        float maxNegPSEmission = 10f;
        float UpdatingNegEmission =
            Mathf.Lerp(minNegPSEmission, maxNegPSEmission, fireIntensity / 200);

        FirePSShape.scale = UpdatingIntensityScale;
        NegPSShape.scale = UpdatingIntensityScale;
        FirePSEmission.rateOverTime = UpdatingFireEmission;
        NegPSEmission.rateOverTime = UpdatingNegEmission;

        if (fireIntensity <= 0f)
        {
            burning = false;
            Destroy(firePS.gameObject);
            Destroy(negativePS.gameObject);
            firePS = null;
            negativePS = null;
        }
    }
}
