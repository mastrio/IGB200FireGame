using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CoolburnGroundItem : MonoBehaviour
{
    //Particles
    [SerializeField] private GameObject FireParticlePrefab;
    private ParticleSystem firePS;

    //Layers
    private int BurnableLayer;
    private int CoolburnLayer;

    //Cortoutine
    private Coroutine CoolBurnGroundCoroutine;
    private Coroutine MoveCoolBurnCoroutine;

    //Bools Variables
    private bool currentlyBurning = false;
    private bool nearbyCoolburn = true;
    // private bool nearbyBurnablesExist = false;

    //Intensity Variables
    private float currentFireIntensity;
    private float fireMaxIntensity = 100f;

    //Time Variables
    private float currentFireTimer;
    private float currentMaxIntensityFireTimer;
    private int maxFireTime = 15;

    //Slider
    [SerializeField] private Slider fireSlider;

    private void Start()
    {
        if (fireSlider != null)
        {
            fireSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    private void OnSliderValueChanged(float sliderfireIntensityValue)
    {
        if (sliderfireIntensityValue < currentFireIntensity)
        {
            currentFireIntensity = sliderfireIntensityValue;

            var firePSShape = firePS.shape;
            Vector3 minUpdatedPsScale = new Vector3(1f, 1f, 1f);
            Vector3 maxUpdatedPsScale = new Vector3(9.17f, 9.823f, 3f);


            Vector3 ChangeFireIntestintyScaleManuel = Vector3.Lerp(minUpdatedPsScale, maxUpdatedPsScale,
                currentFireIntensity / fireMaxIntensity);

        }
        else
        {
            fireSlider.value = currentFireIntensity;
        }
    }

    void Update()
    {
        // Dev key, burns everything, kinda funny.
        if (Input.GetKeyDown(KeyCode.F1) && Global.devMode) CoolBurnIgnition(100.0f);
    }

    //New Method for starting cool Burn, Will allow for slider and managment
    public void CoolBurnIgnition(float startBurnIntensity)
    {
        //sets the fire intensity at start of coolburn ignition
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
        fireMaxIntensity = 150f;

        float coolBurnSpreadDelay = 5f;
        float coolBurnSpreadTimer = 0f;
        float spreadCheckRate = 3f;

        //9.17 X and 9.823 Z
        Vector3 MinFirePsScale = new Vector3(1f, 1f, 1f);
        Vector3 MaxFirePsScale = new Vector3(9.17f, 9.823f, 3f);

        /*while (currentlyBurning)
        {
            currentFireTimer += Time.deltaTime;
            yield return null;
        }*/


        //Temp version since it doesnt allow for fire slider managment
        while (currentFireIntensity < fireMaxIntensity)
        {
            yield return new WaitForSeconds(1f);
            int randomFloat = UnityEngine.Random.Range(2, 30);
            currentFireIntensity += randomFloat;
            fireSlider.value = currentFireIntensity;

            var firePsShape = firePS.shape;

            Vector3 UpdatingIntensityScale =
                Vector3.Lerp(MinFirePsScale, MaxFirePsScale, currentFireIntensity / fireMaxIntensity);
            firePsShape.scale = UpdatingIntensityScale;
            Debug.Log("intensity =" + currentFireIntensity + randomFloat);


            currentFireTimer += 1;
            coolBurnSpreadTimer += 1;
            Debug.Log("Current Times" + currentFireTimer + coolBurnSpreadTimer );
            //Spread to new coolburn area nearby if there is any
            if (currentFireIntensity >= 100f && coolBurnSpreadTimer >= coolBurnSpreadDelay)
            {
                Debug.Log("Got To Try and Spread");
                SpreadToCoolBurn();
                coolBurnSpreadTimer = -spreadCheckRate;
            }
            
            yield return null;
            
            /*
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
                        closestsBurnableObject.TryGetComponent<BurnableObject>(
                            out BurnableObject closestBurnable);
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
             */
        }


        // Once Max Fire Intensity is reached then hold it for 20 seconds (editable) before destorying

        while (currentFireIntensity >= fireMaxIntensity && currentMaxIntensityFireTimer < 120f)
        {
            currentMaxIntensityFireTimer += Time.deltaTime;
            yield return null;
        }

        if (currentFireIntensity >= fireMaxIntensity && currentMaxIntensityFireTimer >= 120f)
        {
            Debug.Log("Destoryed");
            currentlyBurning = false;
           // Destroy(this.GameObject());
        }
    }

    /* private IEnumerator moveCoolBurn(Transform coolburnTarget, CoolburnGroundItem closeCoolburnGround)
     {
        Debug.Log("Started");
        Vector3 initalPosition = firePS.transform.position;
        Vector3 targetEndPoint = coolburnTarget.position;

        float coolburnTimeElapsed = 0f;
        float timeMovemntTakes = 5f;





        //The Direction needed to stech and how far it needs to 
        //Doesnt work for y movement only x and y

        Vector3 closestBurnDirection = coolburnTarget.position - firePS.transform.position;
        float closestBurnDistance = closestBurnDirection.magnitude;
        Vector3 closestBurnDirectionNormalised = closestBurnDirection.normalized;

        //Fire Particle Scale Change Calcualtions
        Vector3 fireParticleInitalScale = firePS.transform.localScale;

        Vector3 coolburnTargetScale = new Vector3
        (
            fireParticleInitalScale.x + Mathf.Abs(closestBurnDirectionNormalised.x) * closestBurnDistance,
            fireParticleInitalScale.y + Mathf.Abs(closestBurnDirectionNormalised.y) * closestBurnDistance,
            fireParticleInitalScale.z + Mathf.Abs(closestBurnDirectionNormalised.z) * closestBurnDistance
        );

        //Fire Particle centre offset calcualtions
        Vector3 newCentreFireOffset = closestBurnDirection * 0.5f;
        firePS.transform.position = initalPosition + newCentreFireOffset;


        while (coolburnTimeElapsed < timeMovemntTakes)
        {
            Debug.Log("Tried");
            coolburnTimeElapsed += Time.deltaTime;
            float moveBurnVar = coolburnTimeElapsed / timeMovemntTakes;

            firePS.transform.localScale = Vector3.Lerp(fireParticleInitalScale, coolburnTargetScale, moveBurnVar);
            firePS.transform.position = Vector3.Lerp(initalPosition, targetEndPoint, moveBurnVar);

             yield return null;
        }

        firePS.transform.localScale = coolburnTargetScale;
        firePS.transform.position = targetEndPoint;

        if (closeCoolburnGround.currentlyBurning)
        {
            Debug.Log("Tried to set other");
            CoolBurnIgnition(20f);
        }








     }*/

    /*private void OnDrawGizmosSelected()
    {
        float radius = 15f;

        // Draw your main OverlapSphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);

        // Check if this runs in Editor safely
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, 1 << LayerMask.NameToLayer("Coolburn"));
        if (hitColliders != null)
        {
            Gizmos.color = Color.green;
            foreach (var col in hitColliders)
            {
                Gizmos.DrawSphere(col.transform.position, 0.3f);
            }
        }
    }

    */
    private void SpreadToCoolBurn()
    {
        CoolburnLayer = 1 << LayerMask.NameToLayer("Coolburn");
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 20f, CoolburnLayer);

        if (hitColliders.Length == 0 || hitColliders == null)
        {
            Debug.Log("coolburn notfound");
            nearbyCoolburn = false;
            return;
        }
        
        //Sorts Array to be the closest collider that was hit
        var orderedCoolburnables = hitColliders
            .Where(c => c && c.gameObject != this.gameObject)//Stops this object from being included
            .OrderBy(c => (c.transform.position - this.transform.position).sqrMagnitude).ToArray();//Orders the rest found by distance from object

        foreach (var collidershit in orderedCoolburnables)
        {
            Debug.Log("" + collidershit.GameObject().name);
        }

        GameObject closestCoolburnObject = orderedCoolburnables[0].gameObject;
        closestCoolburnObject.TryGetComponent<CoolburnGroundItem>(
            out CoolburnGroundItem closestCoolburn);
        if (closestCoolburn.currentlyBurning == false)
        {
            Debug.Log("Tried to set other");
            closestCoolburn.CoolBurnIgnition(20f);
        }
    }

}
