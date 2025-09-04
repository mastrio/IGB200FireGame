using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CoolburnGroundItem : MonoBehaviour
{
    private float fireLifeSpan;
    [SerializeField] private GameObject FireParticlePrefab;
    private ParticleSystem firePS;
    private int BurnableLayer;
    private int CoolburnLayer;

    //Cortoutine variables
    private Coroutine CoolBurnGroundCoroutine;
    private bool currentlyBurning = false;
    private float currentFireIntensity;
    private float fireMaxIntensity = 100f;
    private float currentFireTimer;
    private float currentMaxIntensityFireTimer;
    private int maxFireTime = 15;
    private bool nearbyBurnablesExist = false;
    private bool nearbyCoolburn = true;


    void Update()
    {
        // Dev key, burns everything, kinda funny.
        if (Input.GetKeyDown(KeyCode.F1) && Global.devMode) CoolBurnIgnition(100.0f);
    }

    //New Method for starting cool Burn, Will allow for slider and managment
    public void CoolBurnIgnition(float startBurnIntensity)
    {
        //sets the fire intensity at start of coolburn ignition
        Debug.Log("Couldnt Get Past");
        //Checks if object already on fire
        if (firePS == null)
        {
            GameObject fireinstance = Instantiate(FireParticlePrefab, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            firePS = fireinstance.GetComponent<ParticleSystem>();
            currentlyBurning = true;

            if (CoolBurnGroundCoroutine != null) StopCoroutine(CoolBurnGroundCoroutine);
            CoolBurnGroundCoroutine = StartCoroutine(CoolBurnGroundIntensifys(startBurnIntensity));
        }


    }

    private IEnumerator CoolBurnGroundIntensifys(float StartingFireIntensity)
    {
        currentlyBurning = true;
        currentFireIntensity = StartingFireIntensity;
        currentFireTimer = 0f;

        //Temp version since it doesnt allow for fire slider managment
        while (currentFireIntensity < fireMaxIntensity)
        {
            int randomFloat = UnityEngine.Random.Range(2, 30);
            currentFireIntensity += randomFloat;
            var psSize = firePS.main;
            psSize.startSize = Mathf.Lerp(0.1f, 3f, currentFireIntensity / 100f);
            Debug.Log("intensity =" + currentFireIntensity + randomFloat);

            //Spread to new coolburn area nearby if there is any
            if (currentFireIntensity <= 100f)
            {
                currentFireTimer += Time.deltaTime;
                if (currentFireIntensity <= 100f & currentFireTimer >= 15f)
                {
                    if (nearbyCoolburn == true)
                    {
                        CoolburnLayer = 1 << LayerMask.NameToLayer("Coolburn");
                        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 9f, CoolburnLayer);
                        Debug.Log("Collider list" + hitColliders.Length);
                        if (hitColliders.Length > 0)
                        {
                            nearbyCoolburn = true;

                            foreach (var collidershit in hitColliders)
                            {
                                Debug.Log("" + collidershit.GameObject().name);
                            }

                            //Sorts Array to be the closest collider that was hit
                            var orderedBurnables = hitColliders
                                .OrderBy(c => (c.transform.position - this.transform.position).sqrMagnitude).ToArray();
                            //Breaks if radius is lowered fix this
                            GameObject closestCoolBurnObject = orderedBurnables[1].gameObject;
                            closestCoolBurnObject.TryGetComponent<CoolburnGroundItem>(
                                out CoolburnGroundItem closestCoolburn);
                            if (closestCoolburn.currentlyBurning == false)
                            {
                                Debug.Log("Expensive Please Fix");
                                closestCoolburn.CoolBurnIgnition(20f);
                            }
                        }
                        else if (hitColliders.Length == 0)
                        {
                            nearbyCoolburn = false;
                        }


                    }

                }
            }

            //Spread to nearby object
            if (currentFireIntensity >= 150f)
            {
                if (nearbyBurnablesExist == true)
                {
                    //TEMP RAIDUS SETTING
                    BurnableLayer = 1 << LayerMask.NameToLayer("Burnable");
                    Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 9f, BurnableLayer);
                    Debug.Log("Collider list" + hitColliders.Length);
                    if (hitColliders.Length > 0)
                    {
                        nearbyBurnablesExist = true;

                        foreach (var collidershit in hitColliders)
                        {
                            Debug.Log("" + collidershit.GameObject().name);
                        }

                        //Sorts Array to be the closest collider that was hit
                        var orderedBurnables = hitColliders
                            .OrderBy(c => (c.transform.position - this.transform.position).sqrMagnitude).ToArray();
                        //Breaks if radius is lowered fix this
                        GameObject closestsBurnableObject = orderedBurnables[1].gameObject;
                        closestsBurnableObject.TryGetComponent<CoolBurnableObject>(
                            out CoolBurnableObject closestBurnable);
                        if (closestBurnable.currentlyBurning == false)
                        {
                            Debug.Log("Expensive Please Fix");
                            closestBurnable.CoolBurnIgnition(20f);
                        }
                    }
                    else if (hitColliders.Length == 0)
                    {
                        nearbyBurnablesExist = false;
                    }
                }
            }

            yield return new WaitForSeconds(5f);

        }

        // Once Max Fire Intensity is reached then hold it for 20 seconds (editable) before destorying

        while (currentFireIntensity >= fireMaxIntensity && currentMaxIntensityFireTimer < maxFireTime)
        {
            currentMaxIntensityFireTimer += Time.deltaTime;
            yield return null;
        }

        if (currentFireIntensity >= fireMaxIntensity && currentMaxIntensityFireTimer >= maxFireTime)
        {
            currentlyBurning = false;
            Destroy(this.GameObject());
        }
    }

    //private IEnumerator moveCoolBurn(Transform coolburnTarget)
   // {
     //   Vector3 initalPosition = firePS.transform.position;
       // Vector3 targetEndPoint = coolburnTarget.position;

      //  float coolburnTimeElapsed = Time.deltaTime;

    //}


}
