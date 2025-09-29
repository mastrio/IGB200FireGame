using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class FireObject : MonoBehaviour
{
    public float playerDetectionDistance = 10.0f;
    private bool hasCoolburnTag = false;
    private string coolburnTag = "Coolburn";
    private bool currentlyBurning = false;

    //FireDirection Variables
    [SerializeField] private float MoveSpeed = 4f;
    private float changeDirectionTime = 10f;
    private Vector3 FiresDirection;
    private float FireDirectionTimer;

    //Fire Intensity
    [HideInInspector] public float fireIntensity = 0f;
    [HideInInspector] public float MaxFireIntensity = 200f;
    private float fireIntensityTimer = 5f;
    private float fireIntensityTimerRest = 5f;

    //PS system
    private ParticleSystem fireObjectPS;
    private Vector3 minFirePsScale;
    private Vector3 maxFirePsScale;

    private Coroutine FireIntensityCoroutine;


    void Awake()
    {
        GameManager.instance.fireObjects.Add(gameObject);
        GameManager.instance.fireObjectScripts.Add(this);
        currentlyBurning = true;

        // Added for testing the management UI
        fireIntensity = 50.0f;

        float Firex = UnityEngine.Random.Range(-20f, 20f);
        Debug.Log(Firex);
        float Firez = UnityEngine.Random.Range(-20f, 30f);
        Debug.Log(Firez);
        FiresDirection = new Vector3(Firex, 0f, Firez).normalized;
        Vector3 StartingFiresDirection = transform.position + FiresDirection * MoveSpeed;
        Debug.Log(FiresDirection);
        fireObjectPS = GetComponentInChildren<ParticleSystem>();
        new LerpAnimationVector3(StartingFiresDirection, MoveSpeed);

    }

    void OnDestroy()
    {
        currentlyBurning = false;
        GameManager.instance.fireObjects.Remove(gameObject);
        GameManager.instance.fireObjectScripts.Remove(this);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(coolburnTag)&&fireIntensity > 100f)
        {
            Debug.Log("Called");
            CoolburnGroundItem CollidedEnviroment = other.GetComponent<CoolburnGroundItem>();
            CollidedEnviroment.FireStart();

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(coolburnTag))
        {
            CoolburnGroundItem CollidedEnviroment = other.GetComponent<CoolburnGroundItem>();
            CollidedEnviroment.FireDestory();
        }
    } 


    private IEnumerator IntensifyFire(float initalFireIntensity)
    {
        currentlyBurning = true;
        fireIntensity += initalFireIntensity;
        fireIntensityTimer = 0f;

        var FireObjectPSShape = fireObjectPS.shape;
        minFirePsScale = new Vector3(1f, 1f, 1f);
        maxFirePsScale = new Vector3(9.17f, 9.823f, 3f);
     
        

        while (currentlyBurning)
        {
            Debug.Log(fireIntensityTimer);
            Debug.Log(fireIntensity);
            fireIntensityTimer -= 1f;
            Vector3 UpdatingIntensityScale =
                Vector3.Lerp(minFirePsScale, maxFirePsScale, fireIntensity / MaxFireIntensity);
            if (fireIntensityTimer <= 0f)
            {
                if (fireIntensity > 0f && fireIntensity < 50f)
                {
                    float smallFireIncriment = Random.Range(2f, 6f);
                    fireIntensity += smallFireIncriment;
                    FireObjectPSShape.scale = UpdatingIntensityScale;
                    fireIntensityTimer = fireIntensityTimerRest;
                }
                else if (fireIntensity > 50f && fireIntensity < 150f)
                {
                    float middleFireIncriment = Random.Range(10, 20);
                    fireIntensity += middleFireIncriment;
                    FireObjectPSShape.scale = UpdatingIntensityScale;
                    fireIntensityTimer = fireIntensityTimerRest;
                }
                else if (fireIntensity > 150f && fireIntensity < MaxFireIntensity)
                {
                    float largeFireIncriment = Random.Range(15, 25);
                    fireIntensity += largeFireIncriment;
                    FireObjectPSShape.scale = UpdatingIntensityScale;
                    fireIntensityTimer = fireIntensityTimerRest;
                }
            }

            yield return new WaitForSeconds(1f);
        }

        
    }


    void ChangeDirection()
    {

    
        float Firex = UnityEngine.Random.Range(-50f, 50f);
        float Firez = UnityEngine.Random.Range(-50f, 50f);
        FiresDirection = new Vector3(Firex, 0, Firez).normalized + WindManager.instance.Direction * 0.2f;

        float DistanceFireMoved = Random.Range(1.2f, 9f);

        Vector3 TargetPosition = transform.position + FiresDirection * DistanceFireMoved;

        new LerpAnimationVector3(TargetPosition, MoveSpeed);

    }

    private void Start()
    {
        FireDirectionTimer = changeDirectionTime;
        if (FireIntensityCoroutine != null) StopCoroutine(FireIntensityCoroutine); 
        FireIntensityCoroutine = StartCoroutine(IntensifyFire(20));

    }

    private void Update()
    {
        FireDirectionTimer -= Time.deltaTime;

        //Pick new direction if the timer is met
        if (FireDirectionTimer <= 0f)
        {
            ChangeDirection();
            FireDirectionTimer = changeDirectionTime;
        }
    }

    private void FixedUpdate()
    {
        // Update with current position and then start moving
        transform.Translate(FiresDirection * Time.deltaTime, Space.World);
    }
}