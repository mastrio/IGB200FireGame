using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;

public class BurnableObject : MonoBehaviour
{
    private float fireLifeSpan;
    [SerializeField] private GameObject FireParticlePrefab;
    private ParticleSystem firePS;
    private int BurnableLayer;

    //Cortoutine variables
    private Coroutine BurnableCoroutine;
    public bool currentlyBurning = false;
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
    [SerializeField] private Slider fireSlider;
    [SerializeField] private Canvas gameWorldCanvas;

    void Update()
    {
        // Dev key, burns everything, kinda funny.
        if (Input.GetKeyDown(KeyCode.F1) && Global.devMode) BurnableIgnition(100.0f);
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
                int randomFloat = UnityEngine.Random.Range(2, 12);
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

            if (currentFireIntensity >= 120f)
            {
                burnableSpreadTimer += 1f;
            }


            //Spread to new coolburn area nearby if there is any
            if (nearbyBurnable || burnableSpreadDelayTime > BurnableSpreadCheckRate)
            {
                if (currentFireIntensity >= 140f && burnableSpreadTimer >= burnableSpreadDelayTime)
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
                Destroy(firePS.gameObject); //Change too stop particle emission later
                currentlyBurning = false;
            }

            yield return new WaitForSeconds(1f);
        }
        
    }

    private bool SpreadToBurnables()
    {
        bool targetIgnitable = false;

        BurnableLayer = 1 << LayerMask.NameToLayer("Burnable");
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 20f, BurnableLayer);

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
