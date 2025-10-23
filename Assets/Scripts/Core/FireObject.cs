using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class FireObject : MonoBehaviour
{
    public float playerDetectionDistance = 10.0f;
    private string coolburnTag = "Coolburn";
    private string burnableTag = "Burnable";
    private string boundaryTag = "Boundary";
    private string highFuelTag = "HighFuel";
    private bool BurningHighFuel = false;
    private bool currentlyBurning = false;
    private float updateIntensity;
    private bool goingBackToCentre = false;

    private BoxCollider thisBoxCollider;
    private Vector3 initalColliderSize = new Vector3(5.6f, 3.2f, 5.6f);
    private Vector3 initalPSScale = new Vector3(5f, 5f, 1f);
    private Vector3 currentPSScale;


    //FireDirection Variables
    private float changeDirectionTime = 10f;
    private float highDirectionTimer = 5f;
    private float lowIntensityDirectionTimer = 12f;
    private float medIntensityDirectrionTimer = 20f;
    private Vector3 FiresDirection;
    private float FireDirectionTime;


    //Fire Intensity
    [HideInInspector] public float fireIntensity = 0f;
    [HideInInspector] public float MaxFireIntensity = 200f;
    private float fireIntensityTimer = 3f;
    private float fireIntensityTimerRest = 3f;

    //PS system
    private ParticleSystem fireObjectPS;
    private Vector3 minFirePsScale;
    private Vector3 maxFirePsScale;
    private Vector3 minHighFuelFirePsScale;
    private Vector3 maxHighFuelFirePsScale;

    private Coroutine FireIntensityCoroutine;

    private float minFirePSEmission = 200f;
    private float maxFirePSEmission = 500f;


    void Awake()
    {
        GameManager.instance.fireObjects.Add(gameObject);
        GameManager.instance.fireObjectScripts.Add(this);
        currentlyBurning = true;

        thisBoxCollider = GetComponent<BoxCollider>();

        ChangeDirection();

        fireObjectPS = GetComponentInChildren<ParticleSystem>();

        fireIntensity = 80.0f;
    }

    void OnDestroy()
    {
        currentlyBurning = false;
        if (GameManager.instance != null) //Quick Fix for error
        {
            if (GameManager.instance.fireObjects != null)
            {
                GameManager.instance.fireObjects.Remove(gameObject);
            }

            if (GameManager.instance.fireObjects != null)
            {
                GameManager.instance.fireObjectScripts.Remove(this);
            }

        }
        if (FireManager.instance != null) //Quick Fix for error
        {
            if (FireManager.instance.Fire1RefGameObject == gameObject)
            {
                FireManager.instance.Fire1RefGameObject = null;
            }

            if (FireManager.instance.Fire2RefGameObject == gameObject)
            {
                FireManager.instance.Fire2RefGameObject = null;
            }
        }
        FireManager.instance.ReduceNumberOfFires();

    }

    //Will need to make work with either Child Hitbox or scale mains hitbox
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(coolburnTag))
        {

            other.TryGetComponent<CoolBurnFuelTarget>(out var CollidedCoolburnable);
            if (!CollidedCoolburnable.burning)
            {
                CollidedCoolburnable.BeginFireIgnition(this);
                //CoolburnGroundItem CollidedEnviroment = other.GetComponent<CoolburnGroundItem>();
                //CollidedEnviroment.FireStart();
            }

        }
        else if (other.CompareTag(burnableTag) && fireIntensity >= 133f)
        {

            other.TryGetComponent<EnviromentalBurnableNonTarget>(out var CollidedEnviromentNonTarget);
            if (!CollidedEnviromentNonTarget.burning)
            {
                CollidedEnviromentNonTarget.BeginSpreadFire(this);
            }
        }
        else if (other.CompareTag(highFuelTag))
        {
            other.TryGetComponent<HighFuel>(out var CollidedHighFuel);
            if (!CollidedHighFuel.burning)
            {
                CollidedHighFuel.BeginHighFuelFire(this);
            }
            if (!BurningHighFuel)
            {
                BurningHighFuel = true;
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(boundaryTag))
        {
            Vector3 backToCentre = (Vector3.zero - transform.position).normalized;
            backToCentre.y = 0f;

            FiresDirection = backToCentre;
            goingBackToCentre = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(coolburnTag))
        {
            var CollidedCoolburnable = other.GetComponent<CoolBurnFuelTarget>();
            if (CollidedCoolburnable.burning)
            {
                CollidedCoolburnable.StoppingBurn();
                // CoolburnGroundItem CollidedEnviroment = other.GetComponent<CoolburnGroundItem>();
                // CollidedEnviroment.FireDestory();
            }
        }
        else if (other.CompareTag(burnableTag))
        {

            other.TryGetComponent<EnviromentalBurnableNonTarget>(out var CollidedEnviromentNonTarget);
            if (CollidedEnviromentNonTarget.burning)
            {
                CollidedEnviromentNonTarget.StoppingBurn();
            }

        }
        else if (other.CompareTag(highFuelTag))
        {
            other.TryGetComponent<HighFuel>(out var CollidedHighFuel);
            if (CollidedHighFuel.burning)
            {
                CollidedHighFuel.StoppingHighFuelBurn();
                BurningHighFuel = false;
            }
        }

    }

    private IEnumerator IntensifyFire(float initalFireIntensity)
    {
        currentlyBurning = true;
        updateIntensity = fireIntensity;
        updateIntensity += initalFireIntensity;
        if (updateIntensity <= 0)
        {
            updateIntensity = 1;
        }
        else if (updateIntensity >= 200)
        {
            updateIntensity = 200;
        }

        fireIntensity = updateIntensity;
        fireIntensityTimer = 0f;

        var FireObjectPSShape = fireObjectPS.shape;
        var FireObjectPSEmission = fireObjectPS.emission;

        //Change this for the main Fire Objects Scale x and y are the length and width, z is the height (cause of the rotation)
        minFirePsScale = new Vector3(0.5f, 0.8f, 0.5f);
        maxFirePsScale = new Vector3(5.17f, 5.823f, 1.5f);
        //Change this for the max fuels scales
        minHighFuelFirePsScale = new Vector3(2f, 2.5f, 0.6f);
        maxHighFuelFirePsScale = new Vector3(7.17f, 7.823f, 2f);
        //Fire Objects emission values (lerps across the emission change here for the main object)
        minFirePSEmission = 100f;
        maxFirePSEmission = 800f;

        while (currentlyBurning)
        {
            if (fireIntensity <= 0)
            {
                fireIntensity = 1;
            }
            else if (fireIntensity > 200)
            {
                fireIntensity = 200;
            }
            fireIntensityTimer -= 3f;
            Vector3 UpdatingIntensityScale =
                Vector3.Lerp(minFirePsScale, maxFirePsScale, fireIntensity / MaxFireIntensity);

            Vector3 HighFuelIntensityScale = Vector3.Lerp(minHighFuelFirePsScale, maxHighFuelFirePsScale,
                fireIntensity / MaxFireIntensity);

            float UpdatingEmission = Mathf.Lerp(minFirePSEmission, maxFirePSEmission, fireIntensity / MaxFireIntensity);

            float UpdatingHighFuelEmission = Mathf.Lerp(200f, 600f, fireIntensity / MaxFireIntensity);

            if (fireIntensityTimer <= 0f)
            {
                if (fireIntensity <= 1f)
                {
                    Destroy(gameObject);

                }
                else if (BurningHighFuel)
                {
                    float HighFuelIncriment = Random.Range(15f, 30f);
                    updateIntensity = fireIntensity + HighFuelIncriment;
                    if (updateIntensity <= 0f)
                    {
                        updateIntensity = 1f;
                    }
                    else if (updateIntensity > 200f)
                    {
                        updateIntensity = 200f;
                    }

                    fireIntensity = updateIntensity;
                    FireObjectPSShape.scale = HighFuelIntensityScale;
                    FireObjectPSEmission.rateOverTime = UpdatingHighFuelEmission;


                }
                else if (fireIntensity < 70f)
                {
                    float smallFireIncriment = Random.Range(2f, 6f);
                    updateIntensity = fireIntensity + smallFireIncriment;
                    if (updateIntensity <= 0f)
                    {
                        updateIntensity = 1f;
                    }

                    fireIntensity = updateIntensity;
                    FireObjectPSShape.scale = UpdatingIntensityScale;
                    // FireObjectPSEmission.rateOverTime = Mathf.Lerp(300f, 400f, fireIntensity / 100f);
                    FireObjectPSEmission.rateOverTime = UpdatingEmission;
                    fireIntensityTimer = fireIntensityTimerRest;
                }
                else if (fireIntensity >= 70f && fireIntensity <= 130f)
                {
                    float middleFireIncriment = Random.Range(10, 20);
                    fireIntensity += middleFireIncriment;
                    FireObjectPSShape.scale = UpdatingIntensityScale;
                    FireObjectPSEmission.rateOverTime = UpdatingEmission;
                    fireIntensityTimer = fireIntensityTimerRest;
                    fireIntensityTimer = fireIntensityTimerRest;
                    //Trying to fix for whrn theres two

                }
                else if (fireIntensity > 130f)
                {
                    float largeFireIncriment = Random.Range(15, 25);
                    float smallFireIncriment = Random.Range(2f, 6f);
                    updateIntensity = fireIntensity + largeFireIncriment;
                    if (updateIntensity > 200f)
                    {
                        updateIntensity = 200f;
                    }

                    fireIntensity = updateIntensity;
                    FireObjectPSShape.scale = UpdatingIntensityScale;
                    FireObjectPSEmission.rateOverTime = UpdatingEmission;
                }


                //Goes long on z cause of rotation
                currentPSScale = FireObjectPSShape.scale;
                if (currentPSScale != initalPSScale)
                {
                    Vector3 ScaleRatio =
                        new Vector3(
                            currentPSScale.x / initalPSScale.x,
                            currentPSScale.y / initalPSScale.y,
                            currentPSScale.z / initalPSScale.z
                            );

                    thisBoxCollider.size = Vector3.Scale(initalColliderSize, ScaleRatio);
                }

            }

            FireManager.instance.UpdateEmberParticles();
            yield return new WaitForSeconds(3f);
        }
    }

    void ChangeDirection()
    {
        float Firex = UnityEngine.Random.Range(-1f, 1f);
        float Firez = UnityEngine.Random.Range(-1f, 1f);
        FiresDirection = new Vector3(Firex, 0, Firez).normalized;
        goingBackToCentre = false;
    }

    private void Start()
    {
        FireDirectionTime = changeDirectionTime;
        if (FireIntensityCoroutine != null) StopCoroutine(FireIntensityCoroutine);
        FireIntensityCoroutine = StartCoroutine(IntensifyFire(20));

    }

    private void Update()
    {
        FireDirectionTime -= Time.deltaTime;

        if (fireIntensity <= 70f)
        {
            if (FireDirectionTime <= 0f)
            {
                ChangeDirection();
                FireDirectionTime = lowIntensityDirectionTimer;
            }
        }
        else if (fireIntensity <= 130f)
        {
            if (FireDirectionTime <= 0f)
            {
                ChangeDirection();
                FireDirectionTime = medIntensityDirectrionTimer;
            }
        }
        else if (fireIntensity > 130f)
        {
            if (FireDirectionTime <= 0f)
            {
                ChangeDirection();
                FireDirectionTime = highDirectionTimer;
            }
        }
    }

    private void FixedUpdate()
    {
        //Clamp for the t to prevent it from being outside the fire intensitys min/max
        float t = Math.Clamp(fireIntensity, 0f, 200f);
        float fireMoveSpeed;

        //Checks if under 130 intensity - maybe change to stop using slow speed value at a lower intensity
        if (t <= 130f)
        {
            //Edit the 0.1f (min speed), and 0.6f (max speed) to alter what speed it used at low intensity. Could also change the exponent (3f) 
            //fireMoveSpeed = Mathf.Lerp(0.1f, 0.6f, Mathf.Pow(t / 130f, 3f));
            fireMoveSpeed = Mathf.Lerp(0.2f, 0.8f, Mathf.Pow(t / 130f, 3f));
        }
        else //If above 130 (in red) use fast speed
        {
            //Edit the 0.6f (min) and 2.5f (max) to alter the speed it uses across intensity or change the exponent (2f)
            fireMoveSpeed = Mathf.Lerp(0.8f, 2.5f, Mathf.Pow((t - 130f) / 70f, 2f));
        }
        //float fireMoveSpeed = Mathf.Lerp(0.1f, 1f, fireIntensity / 200f); - Old System
        // Update with current position and then start moving


        //The actual movement with the direction (Direction affected by wind already in ChangeDirection) multplied by the speed multiplied by Time.deltaTime.
        Vector3 windInfluence = WindManager.instance.Direction * 2.0f;
        if (goingBackToCentre) windInfluence = Vector3.zero;
        Vector3 velocity = (FiresDirection + windInfluence).normalized * fireMoveSpeed * Time.deltaTime;
        transform.Translate(velocity, Space.World);


    }
}