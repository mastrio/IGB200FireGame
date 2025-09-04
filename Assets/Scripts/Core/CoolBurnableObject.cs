using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class CoolBurnableObject : MonoBehaviour
{
    private float fireLifeSpan;
    [SerializeField] private GameObject FireParticlePrefab;
    private ParticleSystem firePS;
    private int BurnableLayer;

    //Cortoutine variables
    private Coroutine BurnabbleCoroutine;
    public bool currentlyBurning = false;
    private float currentFireIntensity;
    private float fireMaxIntensity = 100f;
    private float currentFireTimer;
    private int maxFireTime = 15;
    private int nearbyBurnablesExist = 0;


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
            GameObject fireinstance = Instantiate(FireParticlePrefab, transform.position, Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            firePS = fireinstance.GetComponent<ParticleSystem>();
            currentlyBurning = true;

            if (BurnabbleCoroutine != null) StopCoroutine(BurnabbleCoroutine);
            BurnabbleCoroutine = StartCoroutine(BurningIntensifys(startBurnIntensity));
        }
            

    }

    private IEnumerator BurningIntensifys(float StartingFireIntensity)
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
            //Not Working Yet
            if (currentFireIntensity >= 90f)
            {
                if (nearbyBurnablesExist == 0 || nearbyBurnablesExist == 1)
                {
                    //TEMP RAIDUS SETTING
                    BurnableLayer = 1 << LayerMask.NameToLayer("Burnable");
                    Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 9f, BurnableLayer);
                    Debug.Log("Collider list" + hitColliders.Length);
                    if (hitColliders.Length > 0)
                    {
                        nearbyBurnablesExist = 1;

                        foreach (var collidershit in hitColliders)
                        {
                            Debug.Log("" + collidershit.GameObject().name);
                        }
                        //Sorts Array to be the closest collider that was hit
                        var orderedBurnables = hitColliders
                            .OrderBy(c => (c.transform.position - this.transform.position).sqrMagnitude).ToArray();
                        //Breaks if radius is lowered fix this
                        GameObject closestsBurnableObject = orderedBurnables[1].gameObject;
                        closestsBurnableObject.TryGetComponent<CoolBurnableObject>(out CoolBurnableObject closestBurnable);
                        if (closestBurnable.currentlyBurning == false)
                        {
                            Debug.Log("Expensive Please Fix");
                            closestBurnable.CoolBurnIgnition(20f);
                        }
                    }
                    else if (hitColliders.Length == 0)
                    {
                        nearbyBurnablesExist = 2;
                    }
                }
                else if (nearbyBurnablesExist == 2)
                {
                    yield return new WaitForSeconds(30f);
                    nearbyBurnablesExist = 0;
                }
            }

            yield return new WaitForSeconds(5f);
           
       }

       // Once Max Fire Intensity is reached then hold it for 20 seconds (editable) before destorying

       while (currentFireIntensity >= fireMaxIntensity && currentFireTimer < maxFireTime)
       {
           currentFireTimer += Time.deltaTime;
           yield return null;
       }

       if (currentFireIntensity >= fireMaxIntensity && currentFireTimer >= maxFireTime)
       {
           currentlyBurning = false;
           Destroy(this.GameObject());
       }
    }
}
