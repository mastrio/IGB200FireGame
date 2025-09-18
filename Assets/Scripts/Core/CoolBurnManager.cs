using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class CoolBurnManager : MonoBehaviour
{
    [SerializeField] private GameObject FireParticlePrefab;
    private ParticleSystem firePS;

    //Cortoutine variables
    private Coroutine FireCoroutine;
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

    public void FireIgnition(float startBurnIntensity, Vector3 ClickPoint)
    {
        //sets the fire intensity at start of coolburn ignition
        //Checks if object already on fire
        if (firePS == null)
        {
            GameObject fireinstance = Instantiate(FireParticlePrefab, ClickPoint,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
            firePS = fireinstance.GetComponent<ParticleSystem>();
            currentlyBurning = true;

            if (FireCoroutine != null) StopCoroutine(FireCoroutine);
            FireCoroutine = StartCoroutine(FireIntensifys(startBurnIntensity));
        }
    }

    private IEnumerator FireIntensifys(float StartingFireIntensity)
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
                    //  nearbyBurnable = SpreadToBurnables();
                    Debug.Log("Bzzt Not Done");
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
                    ScoreManager.instance.UpdateScore(5);
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

    /*private bool SpreadToBurnables()
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
    }*/
}
