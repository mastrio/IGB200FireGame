using System;
using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;
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
    private int PlayerLayer;

    //Cortoutine
    private Coroutine CoolBurnGroundCoroutine;
    private Coroutine MoveCoolBurnCoroutine;

    //Bools Variables
    [HideInInspector] public bool currentlyBurning = false;
    private bool nearbyCoolburn = true;
    private bool nearbyBurnable = true;
   

    //Intensity Variables
    private float currentFireIntensity;
    private float fireMaxIntensity = 100f;

    //Time Variables
    private float currentFireTimer;
    private float weakFireTime;
    private float currentMaxIntensityFireTimer;

   
       
    //Slider
    [Header("Ui Slider")]
    [SerializeField] private Canvas gameWorldCanvas;
    [SerializeField] private Slider fireSlider;
    private bool playerWithinOnTrigger = false;
    private string playerTag = "Player";

    private ScoreManager scoreManager;
    private GameObject spawnFire;

    private void Awake()
    {
        if (gameWorldCanvas != null)
        {
            gameWorldCanvas.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        if (fireSlider != null)
        {
            fireSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
      
    }

    public void FireStart()
    {
        if (spawnFire == null)
        {
            GameObject spawnFire =
                Instantiate(FireParticlePrefab, transform.position, Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)),transform);
        }
    }
    public void FireDestory()
    {
        if (spawnFire != null)
        {
            int destroyRandomNum = UnityEngine.Random.Range(2, 10);
            Destroy(spawnFire.gameObject, destroyRandomNum);
        }
    }

    public void OnSliderValueChanged(float sliderfireIntensityValue)
    {
        if (sliderfireIntensityValue < currentFireIntensity)
        {
            currentFireIntensity = sliderfireIntensityValue;
            if (firePS != null)
            {
                var firePSShape = firePS.shape;
                Vector3 minUpdatedPsScale = new Vector3(1f, 1f, 1f);
                Vector3 maxUpdatedPsScale = new Vector3(9.17f, 9.823f, 3f);


                Vector3 ChangeFireIntestintyScaleManuel = Vector3.Lerp(minUpdatedPsScale, maxUpdatedPsScale,
                    currentFireIntensity / fireMaxIntensity);
                firePSShape.scale = ChangeFireIntestintyScaleManuel;
            }
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

        if (currentlyBurning && fireSlider != null)
        {
            if (fireSlider.value != currentFireIntensity)
            {
                fireSlider.value = currentFireIntensity;
            }
        }
    }

    public void OnTriggerEnterRelayFromChild(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerWithinOnTrigger = true;
        }
    }

    public void OnTriggerExitRelayFromChild(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerWithinOnTrigger = false;
            gameWorldCanvas.gameObject.SetActive(false);
        }
    }


    public void SetFireSliderVisible(bool fireSliderVisibility)
    {

        //If prefab has canvas then set it to active 
        if (playerWithinOnTrigger && gameWorldCanvas != null)
        {
            gameWorldCanvas.gameObject.SetActive(fireSliderVisibility);

        }
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
        /*else if (firePS != null)
        {
            
        }*/


    }

    private IEnumerator CoolBurnGroundIntensifys(float StartingFireIntensity)
    {
        currentlyBurning = true;
        currentFireIntensity = StartingFireIntensity;
        currentFireTimer = 0f;
        weakFireTime = 0f;
        fireMaxIntensity = 150f;

        float coolBurnSpreadDelay = 10f;
        float coolBurnSpreadTimer = 0f;
        float burnableSpreadTimer = 0f;
        float burnableSpreadDelayTime = 15f;
        float CoolSpreadCheckRate = 5f;
        float BurnableSpreadCheckRate = 5f;

        //9.17 X and 9.823 Z
        Vector3 MinFirePsScale = new Vector3(1f, 1f, 1f);
        Vector3 MaxFirePsScale = new Vector3(9.17f, 9.823f, 3f);

   
        while (currentlyBurning)
        {
            //This Only Grows if its below the max value
            if (currentFireIntensity < fireMaxIntensity)
            {
                int randomFloat = UnityEngine.Random.Range(2, 15);
                currentFireIntensity += randomFloat;
            }



            if (fireSlider != null)
            {
                fireSlider.value = currentFireIntensity;
            }

            if (firePS != null)
            {
                var firePsShape = firePS.shape;
                //Make Null Exception
                Vector3 UpdatingIntensityScale =
                    Vector3.Lerp(MinFirePsScale, MaxFirePsScale, currentFireIntensity / fireMaxIntensity);
                firePsShape.scale = UpdatingIntensityScale;

            }

            //FireLifetime 
            currentFireTimer += 1f;

            if (currentFireIntensity >= 100f)
            {

                Debug.Log("Called");
                coolBurnSpreadTimer += 1f;
            }
            else if (currentFireIntensity < 100f)
            {
                Debug.Log(""+coolBurnSpreadTimer);
                coolBurnSpreadTimer = 0; //Could Change to -1 instead
            }


            if (currentFireIntensity >= 125f)
            {
                burnableSpreadTimer += 1f;
            }
            else if (currentFireIntensity < 125f)
            {
                burnableSpreadTimer = 0f;
            }



            //Spread to new coolburn area nearby if there is any
            if (nearbyCoolburn || coolBurnSpreadTimer > CoolSpreadCheckRate)
            {
                if (currentFireIntensity >= 100f && coolBurnSpreadTimer >= coolBurnSpreadDelay)
                {
                    nearbyCoolburn = SpreadToCoolBurn();
                    coolBurnSpreadTimer = 0;
                }
            }

            if (nearbyBurnable || burnableSpreadDelayTime > BurnableSpreadCheckRate)
            {
                if (currentFireIntensity >= 125f && burnableSpreadTimer >= burnableSpreadDelayTime)
                {
                    
                    nearbyBurnable = SpreadToBurnables();
                    burnableSpreadTimer = 0;
                }
            }
            

            //Incriments the timer when below the weak intensity threshold
            if (currentFireIntensity <= 50f)
            {
                weakFireTime += 1f;
            }
            else if (currentFireIntensity >= 50f)
            {
                weakFireTime = 0f;
            }

            if (currentFireIntensity <= 50f && weakFireTime >= 10f)
            {
                FireManager.UpdateFireDangerLevel(false);
                Destroy(firePS.gameObject); //Change too stop particle emission later
                currentlyBurning = false;
                ScoreManager.instance.UpdateScore(1);
            }



            // Once Max Fire Intensity is reached then hold it for 60 seconds (editable) before destorying

            //Only Really needed for wildfire burnables or if above the max intensity
            if (currentFireIntensity >= fireMaxIntensity)
            {
                currentMaxIntensityFireTimer += 1f;
                Debug.Log("Time: "+ currentMaxIntensityFireTimer);

                if (currentMaxIntensityFireTimer >= 60f)
                {
                    //destory object if timer is over ~60 seconds
                    Debug.Log("Destoryed");
                    FireManager.UpdateFireDangerLevel(false);
                    ScoreManager.instance.UpdateScore(4);
                    currentlyBurning = false;
                    Destroy(this.GameObject());
                }
            }
            else
            {
                //Reset maxintensity firetimer
                currentMaxIntensityFireTimer = 0f;
            }

            yield return new WaitForSeconds(1f);

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
    private bool SpreadToCoolBurn()
    {
        bool targetIgnitable = false;

        CoolburnLayer = 1 << LayerMask.NameToLayer("Coolburn");
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 10f, CoolburnLayer,QueryTriggerInteraction.Ignore);

        if (hitColliders == null || hitColliders.Length == 0)
        {
            Debug.Log("coolburn notfound");
            nearbyCoolburn = false;
            return nearbyCoolburn;
        }

        //Sorts Array to be the closest collider that was hit
        var orderedCoolburnables = hitColliders
            .Where(c => c && c.gameObject != this.gameObject)//Stops this object from being included
            .OrderBy(c => (c.transform.position - this.transform.position).sqrMagnitude).ToArray();//Orders the rest found by distance from object

        foreach (var colliderHit in orderedCoolburnables)
        {
            GameObject closestCoolburnObject = colliderHit.gameObject;
            if (closestCoolburnObject.TryGetComponent<CoolburnGroundItem>(out CoolburnGroundItem closestCoolburn))
            {
                if (!closestCoolburn.currentlyBurning)
                {
                    Debug.Log("Tried to set other");
                    closestCoolburn.CoolBurnIgnition(20f);
                    FireManager.UpdateFireDangerLevel(true);

                    targetIgnitable = true;
                    break;
                }
            }
        }

        if (!targetIgnitable)
        {
            //All closests werent ignitable
            nearbyBurnable = false;
            return nearbyBurnable;
        }
        else
        {
            nearbyBurnable = true;
            return nearbyBurnable;
        }
    }

    private bool SpreadToBurnables()
    {
        bool targetIgnitable = false;

        BurnableLayer = 1 << LayerMask.NameToLayer("Burnable");
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 10f, BurnableLayer, QueryTriggerInteraction.Ignore);

        if (hitColliders == null || hitColliders.Length == 0)
        {
            Debug.Log("coolburn notfound");
            nearbyBurnable = false;
            return nearbyBurnable;
        }

        //Sorts Array to be the closest collider that was hit
        var orderedBurnables = hitColliders
            .Where(c => c && c.gameObject != this.gameObject)//Stops this object from being included
            .OrderBy(c => (c.transform.position - this.transform.position).sqrMagnitude).ToArray();//Orders the rest found by distance from object

        foreach (var colliderHit in orderedBurnables)
        {
            GameObject closestBurnableObject = colliderHit.gameObject;
            if (closestBurnableObject.TryGetComponent<BurnableObject>(out BurnableObject closestBurnable))
            {
                if (!closestBurnable.currentlyBurning)
                {
                    Debug.Log("Tried to set other");
                    closestBurnable.BurnableIgnition(20f);
                    FireManager.UpdateFireDangerLevel(true);
                    ScoreManager.instance.UpdateScore(3);
                    targetIgnitable = true;
                    break;
                }
            }
        }

        if (!targetIgnitable)
        {
            //All closests werent ignitable
            nearbyBurnable = false;
            return nearbyBurnable;
        }
        else
        {
            nearbyBurnable = true;
            return nearbyBurnable;
        }
    }
}
