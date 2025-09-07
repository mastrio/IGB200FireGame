using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;

//Probarably will merge with CoolBurnGround item
public class BurnableObject : MonoBehaviour
{
    private float fireLifeSpan;
    [SerializeField] private GameObject FireParticlePrefab;
    private ParticleSystem firePS;
    private int BurnableLayer;
    private int PlayerLayer;

    //Cortoutine variables
    private Coroutine BurnableCoroutine;
    [HideInInspector] public bool currentlyBurning = false;
    private float currentFireIntensity;
    private float fireMaxIntensity = 100f;
    private float currentFireTimer;
    

    //Time Variables
    
    private float weakFireTime;
    private float currentMaxIntensityFireTimer;

    //Bools
    private bool nearbyBurnable = true;

    //Slider
    [Header("Ui Slider")]
    [SerializeField] private Canvas gameWorldCanvas;
    [SerializeField] private Slider fireSlider;
    private string playerTag = "Player";
    private bool playerWithinOnTrigger = false;

    private void Awake()
    {
        if (gameWorldCanvas != null)
        {
            gameWorldCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Dev key, burns everything, kinda funny.
        if (Input.GetKeyDown(KeyCode.F1) && Global.devMode) BurnableIgnition(100.0f);

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

    //New Method for starting cool Burn, Will allow for slider and managment
    public void BurnableIgnition(float startBurnIntensity)
    {
        //sets the fire intensity at start of coolburn ignition
        //Checks if object already on fire
        if (firePS == null)
        {
            GameObject fireinstance = Instantiate(FireParticlePrefab, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            firePS = fireinstance.GetComponent<ParticleSystem>();
            currentlyBurning = true;

            if (BurnableCoroutine != null) StopCoroutine(BurnableCoroutine);
            BurnableCoroutine = StartCoroutine(BurningIntensifys(startBurnIntensity));
        }
    }

    private IEnumerator BurningIntensifys(float StartingFireIntensity)
    {
        currentlyBurning = true;
        currentFireIntensity = StartingFireIntensity;
        currentFireTimer = 0f;
        weakFireTime = 0f;
        fireMaxIntensity = 150f;

       

        float burnableSpreadTimer = 0f;
        float burnableSpreadDelayTime = 10f;
        float BurnableSpreadCheckRate = 15f;

        //9.17 X and 9.823 Z
        Vector3 MinFirePsScale = new Vector3(1f, 1f, 1f);
        Vector3 MaxFirePsScale = new Vector3(3f, 3f, 5f);

        while (currentlyBurning)
        {
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

            if (currentFireIntensity >= 125f)
            {
                burnableSpreadTimer += 1f;
            }
            else if (currentFireIntensity < 125f)
            {
                burnableSpreadDelayTime = 0f;
            }



            //Spread to new coolburn area nearby if there is any
            if (nearbyBurnable || burnableSpreadDelayTime > BurnableSpreadCheckRate)
            {
                if (currentFireIntensity >= 125f && burnableSpreadTimer >= burnableSpreadDelayTime)
                {
                    nearbyBurnable = SpreadToBurnables();
                    burnableSpreadTimer = 0;
                }
            }

            //Incriments the timer when below the weak intensity threshold
            if (currentFireIntensity < 30f)
            {
                weakFireTime += 1f;
            }
            else if (currentFireIntensity >= 30f)
            {
                weakFireTime = 0f;
            }

            if (currentFireIntensity <= 30f && weakFireTime >= 15f)
            {
                FireManager.UpdateFireDangerLevel(false);
                Destroy(firePS.gameObject); //Change too stop particle emission later
                currentlyBurning = false;
                ScoreManager.instance.UpdateScore(2);
            }


            // Once Max Fire Intensity is reached then hold it for 60 seconds (editable) before destorying

            //Only Really needed for wildfire burnables or if above the max intensity
            if (currentFireIntensity >= fireMaxIntensity)
            {
                currentMaxIntensityFireTimer += 1f;
                Debug.Log("Time: " + currentMaxIntensityFireTimer);

                if (currentMaxIntensityFireTimer >= 60f)
                {
                    //destory object if timer is over ~60 seconds
                    Debug.Log("Destoryed");
                    FireManager.UpdateFireDangerLevel(false);
                    currentlyBurning = false;
                    ScoreManager.instance.UpdateScore(4);
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
