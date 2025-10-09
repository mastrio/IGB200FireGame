using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class FireObject : MonoBehaviour
{
    public float playerDetectionDistance = 10.0f;
    private string coolburnTag = "Coolburn";
    private string burnableTag = "Burnable";
    private bool currentlyBurning = false;
    private float updateIntensity;

    private BoxCollider thisBoxCollider;
    private Vector3 initalColliderSize = new Vector3(5.6f,3.2f,5.6f);
    private Vector3 initalPSScale = new Vector3(5f,5f,1f);
    private Vector3 currentPSScale;
    

    //FireDirection Variables
    [SerializeField] private float MoveSpeed = 0.1f;
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
    private float MaxFireIntensityTimer = 0f;
    private float FireWeakTimer = 0f;

    //PS system
    private ParticleSystem fireObjectPS;
    private Vector3 minFirePsScale;
    private Vector3 maxFirePsScale;

    private Coroutine FireIntensityCoroutine;

    private float minFirePSEmission = 200f;
    private float maxFirePSEmission = 500f;


    void Awake()
    {
        GameManager.instance.fireObjects.Add(gameObject);
        GameManager.instance.fireObjectScripts.Add(this);
        currentlyBurning = true;

        thisBoxCollider = GetComponent<BoxCollider>();

        
        // Added for testing the management UI
        fireIntensity = 50.0f;

        float Firex = UnityEngine.Random.Range(-30f, 20f);
        Debug.Log(Firex);
        float Firez = UnityEngine.Random.Range(-30f, 30f);
        Debug.Log(Firez);
        FiresDirection = new Vector3(Firex, 0f, Firez).normalized;
        Vector3 StartingFiresDirection = transform.position + FiresDirection * MoveSpeed;
       
        fireObjectPS = GetComponentInChildren<ParticleSystem>();
        new LerpAnimationVector3(StartingFiresDirection, MoveSpeed); 


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
        else if (other.CompareTag(burnableTag)) //&& fireIntensity >= 120f)
        {
          
            other.TryGetComponent<EnviromentalBurnableNonTarget>(out var CollidedEnviromentNonTarget);
            if (!CollidedEnviromentNonTarget.burning)
            {
                CollidedEnviromentNonTarget.BeginSpreadFire(this);
            }

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
        minFirePsScale = new Vector3(0.5f, 0.8f, 0.5f);
        maxFirePsScale = new Vector3(7.17f, 7.823f, 2f);
        minFirePSEmission = 100f;
        maxFirePSEmission = 500f;

        



        while (currentlyBurning)
        {
            Debug.Log(fireIntensity);

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
            float UpdatingEmission = Mathf.Lerp(minFirePSEmission, maxFirePSEmission, fireIntensity / MaxFireIntensity);
            if (fireIntensityTimer <= 0f)
            {
                if (fireIntensity < 50f)
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
                    if (FireWeakTimer > 20f)
                    {
                        FireManager.instance.ReduceNumberOfFires();
                        Destroy(gameObject);
                    }
                    else
                    {
                        FireWeakTimer += 3f;
                    }
                }
                else if (fireIntensity > 50f && fireIntensity < 150f)
                {
                    float middleFireIncriment = Random.Range(10, 20);
                    fireIntensity += middleFireIncriment;
                    FireObjectPSShape.scale = UpdatingIntensityScale;
                    FireObjectPSEmission.rateOverTime = UpdatingEmission;
                    fireIntensityTimer = fireIntensityTimerRest;
                    fireIntensityTimer = fireIntensityTimerRest;
                    MaxFireIntensityTimer = 0f;
                    FireWeakTimer = 0f;
                }
                else if (fireIntensity > 150f)
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
                    FireWeakTimer = 0f;
                }
                else if (fireIntensity >= MaxFireIntensity)
                {
                    FireObjectPSShape.scale = new Vector3(9.5f, 10f, 3.1f);
                    FireObjectPSEmission.rateOverTime = 550f;
                    ScoreManager.instance.AddScore(-2);
                    MaxFireIntensityTimer += 1f;
                    FireWeakTimer = 0f;

                    //Temporary just for now will change later but is the extreme case 

                    if (MaxFireIntensityTimer >= 35f)
                    {
                        FireObjectPSShape.scale = new Vector3(15f, 16f, 4f);
                        FireObjectPSEmission.rateOverTime = 600f;
                        ScoreManager.instance.AddScore(-10);
                    }
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

                    thisBoxCollider.size = Vector3.Scale(initalColliderSize,ScaleRatio);
                }
            }

            yield return new WaitForSeconds(3f);
        }


    }


    void ChangeDirection(float movespeed)
    {


        float Firex = UnityEngine.Random.Range(-50f, 50f);
        float Firez = UnityEngine.Random.Range(-50f, 50f);
        FiresDirection = new Vector3(Firex, 0, Firez).normalized + WindManager.instance.Direction * movespeed;

        float DistanceFireMoved = Random.Range(1.2f, 9f);

        Vector3 TargetPosition = transform.position + FiresDirection * DistanceFireMoved;

        new LerpAnimationVector3(TargetPosition, MoveSpeed);

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

        if (fireIntensity < 75f)
        {
            if (FireDirectionTime <= 0f)
            {
                ChangeDirection(0.1f);
                FireDirectionTime = lowIntensityDirectionTimer;
            }
        }
        else if (fireIntensity < 150f)
        {
            if (FireDirectionTime <= 0f)
            {
                ChangeDirection(0.25f);
                FireDirectionTime = medIntensityDirectrionTimer;
            }
        }
        else if (fireIntensity > 150f)
        {
            if (FireDirectionTime <= 0f)
            {
                ChangeDirection(0.4f);
                FireDirectionTime = highDirectionTimer;
            }
        }
    }

    private void FixedUpdate()
    {
        // Update with current position and then start moving
        transform.Translate(FiresDirection * Time.deltaTime, Space.World);
    }
}