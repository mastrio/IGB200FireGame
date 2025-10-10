using System;
using System.Collections;
using System.Data.Common;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.Rendering.DebugUI;
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

    private float fireIntensity;

    private Coroutine enviroIntensiftyCoroutine;
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
  
        /*if (enviroIntensiftyCoroutine != null)
        {
            enviroIntensiftyCoroutine = StartCoroutine(EnviroFireIntensifys(BaseFireObjectRef.fireIntensity));
        }
        if (burning == false) StopCoroutine(enviroIntensiftyCoroutine);*/

        FireManager.UpdateFireDangerLevel(true); //Increase by one as the fire spread to something unintended
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

    private void Update()
    {
        if (!burning && BaseFireObjectRef == null)
        {
            return;
        }
        float tempIntensity = BaseFireObjectRef.fireIntensity;
        if (Mathf.Approximately(fireIntensity,tempIntensity))
        {
            return;
        }

        EnviromentIntensifying(BaseFireObjectRef.fireIntensity);
        //tempFireTimer = 1f;
    }

    private void EnviromentIntensifying(float currentFireIntensity)
    {
        if (firePS == null || negativePS == null)
        {
            return;
        }
        fireIntensity = currentFireIntensity;

        var FirePSShape = firePS.shape;
        var FirePSEmission = firePS.emission;
        var NegPSShape = negativePS.shape;
        var NegPSEmission = negativePS.emission;
        Vector3 minFirePsScale = new Vector3(0.8f, 1.3f, 0.5f);
        Vector3 maxFirePsScale = new Vector3(7.17f, 7.823f, 2f);
        float minFirePSEmission = 100f;
        float maxFirePSEmission = 500f;
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
        if (fireIntensity <= 0)
        {
            burning = false;
            Destroy(firePS.gameObject);
            Destroy(negativePS.gameObject);
            firePS = null;
            negativePS = null;
        }
        //Ask if we want it to burn after x amount of burning at max
        /*else if (fireIntensity >= 200f && BurnTimer >= 30f)
        {
            Debug.Log("Burnt");
            // ScoreManager.instance.AddScore(-10);
            MaxBurnTime += 3;
        }
        else if (fireIntensity >= 200f && MaxBurnTime > 20f)
        {
            // ScoreManager.instance.AddScore(-15);
            //Should reduce the fire danger level by one since its being burnt (back to how it was before unintended got set on fire)
            Destroy(gameObject);
        }*/
    }
    /*private IEnumerator EnviroFireIntensifys(float currentIntensity)
    {
        currentIntensity = BaseFireObjectRef.fireIntensity;

        var FirePSShape = firePS.shape;
        var FirePSEmission = firePS.emission;
        var NegPSShape = negativePS.shape;
        var NegPSEmission = negativePS.emission;
        Vector3 minFirePsScale = new Vector3(0.8f, 1.3f, 0.5f);
        Vector3 maxFirePsScale = new Vector3(7.17f, 7.823f, 2f);
        float minFirePSEmission = 100f;
        float maxFirePSEmission = 500f;
        Vector3 UpdatingIntensityScale =
            Vector3.Lerp(minFirePsScale, maxFirePsScale, currentIntensity / 200);
        float UpdatingFireEmission =
            Mathf.Lerp(minFirePSEmission, maxFirePSEmission, currentIntensity / 200);


        float minNegPSEmission = 20f;
        float maxNegPSEmission = 100f;
        float UpdatingNegEmission =
            Mathf.Lerp(minNegPSEmission, maxNegPSEmission, currentIntensity / 200);

        FirePSShape.scale = UpdatingIntensityScale;
        NegPSShape.scale = UpdatingIntensityScale;
        FirePSEmission.rateOverTime = UpdatingFireEmission;
        NegPSEmission.rateOverTime = UpdatingNegEmission;
        
        
        yield return new WaitForSeconds(3f);
    }*/
    
}
