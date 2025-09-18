using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class FireObject : MonoBehaviour
{
    public float playerDetectionDistance = 10.0f;
    private bool hasCoolburnTag = false;
    private string coolburnTag = "Coolburn";
    private bool currentlyBurning = false;

    [SerializeField] private float MoveSpeed = 10f;
    [SerializeField] private float DirectionTime = 10f;
    private Vector3 FiresDirection;
    private float FireDirectionTimer;
    private float fireIntensity = 0f;

    void Awake()
    {
        GameManager.instance.fireObjects.Add(gameObject);
        GameManager.instance.fireObjectScripts.Add(this);
        currentlyBurning = true;
    }

    void OnDestroy()
    {
        currentlyBurning = false;
        GameManager.instance.fireObjects.Remove(gameObject);
        GameManager.instance.fireObjectScripts.Remove(this);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("FUCCCCCCCCCCCCCCCCCCCK");
        if (other.CompareTag(coolburnTag))
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
    void UpdateFireIntensity(float currentIntensity)
    {
        if (fireIntensity >= 200f) ;
        {
            fireIntensity += currentIntensity;
            ParticleSystem FireObjectsPS = GetComponentInChildren<ParticleSystem>();
            Vector3 MinFirePsScale = new Vector3(1f, 1f, 1f);
            Vector3 MaxFirePsScale = new Vector3(15.17f, 15.823f, 4f);

            var FireObjectPSShape = FireObjectsPS.shape;
            //Make Null Exception
            Vector3 UpdatingIntensityScale =
                Vector3.Lerp(MinFirePsScale, MaxFirePsScale, fireIntensity / 200f);
            transform.localScale = UpdatingIntensityScale;
        }
    }


    void ChangeDirection()
    {
        float Firex = UnityEngine.Random.Range(-20f, 30f);
        float Firez = UnityEngine.Random.Range(-20f, 30f);
        FiresDirection = new Vector3(Firex, 0, Firez).normalized;
    }

    private void Start()
    {
        ChangeDirection();
        FireDirectionTimer = DirectionTime;

    }

    private void Update()
    {
        transform.position += FiresDirection * MoveSpeed * Time.deltaTime;

        FireDirectionTimer -= Time.deltaTime;

        if (FireDirectionTimer <= DirectionTime)
        {
            ChangeDirection();
            FireDirectionTimer = DirectionTime;
        }
    }
}
