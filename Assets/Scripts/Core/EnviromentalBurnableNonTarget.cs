using System.Collections;
using UnityEngine;

public class EnviromentalBurnableNonTarget : MonoBehaviour
{
    private FireObject BaseFireObjectRef;
    [HideInInspector] public bool burning = false;
    private Coroutine fireExtinguisherCoroutine;
    private float BurnTimer;
    private float MaxBurnTime = 0f;
    [SerializeField] private GameObject FireParticlePrefab;
    private ParticleSystem firePS;

    public void BeginSpreadFire(FireObject baseFireObject)
    {
        BaseFireObjectRef = baseFireObject;
        burning = true;
        BurnTimer = 0f;
        GameObject fireParticle = Instantiate(FireParticlePrefab, transform.position,
            Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)), transform);
        firePS = fireParticle.GetComponent<ParticleSystem>();
    }

    public void StoppingBurn()
    {
        burning = false;

        if (fireExtinguisherCoroutine != null) StopCoroutine(fireExtinguisherCoroutine);
        fireExtinguisherCoroutine = StartCoroutine(SpreadFireExtinguisher());
    }

    private IEnumerator SpreadFireExtinguisher()
    {
        float delay = UnityEngine.Random.Range(15f, 20f);
        yield return new WaitForSeconds(delay);

        if (firePS != null)
        {
            Destroy(firePS.gameObject);
        }

    }


    // Update is called once per frame
    void Update()
    {
        //End if not currently Burning
        if (burning)
        {
            BurningEnviroment(BaseFireObjectRef.fireIntensity);
        }

    }

    private void BurningEnviroment(float currentIntensity)
    {
        currentIntensity = BaseFireObjectRef.fireIntensity;

        var FirePSShape = firePS.shape;
        if (currentIntensity < 100f)
        {

            Vector3 SmallFireMin = new Vector3(0.8f, 1.3f, 0.5f); // Untested but want to show small fire
            Vector3 SmallFireMax = new Vector3(2f, 2.5f, 0.8f); // Untested but want to show small fire
            FirePSShape.scale = Vector3.Lerp(SmallFireMin, SmallFireMax, currentIntensity / 200f);
            BurnTimer = 0f;
            MaxBurnTime = 0f;

        }
        else if (currentIntensity > 100f && currentIntensity <= 200f)
        {
            BurnTimer += Time.deltaTime;
            Vector3 MedFireMin = new Vector3(3.1f, 3.5f, 2.7f); // Untested but want to show small fire
            Vector3 MedFireMax = new Vector3(4.7f, 5f, 4.5f); // Untested but want to show small fire
            FirePSShape.scale = Vector3.Lerp(MedFireMin, MedFireMax, currentIntensity / 200f);
            Debug.Log("Do This Other Thing");
            MaxBurnTime = 0f;
            ScoreManager.instance.AddScore(-1);

        }
        else if (currentIntensity >= 200f && BurnTimer >= 30f)
        {
            Vector3 BigFireMin = new Vector3(5.3f, 5.8f, 4.5f); // Untested but want to show small fire
            Vector3 BigFireMax = new Vector3(9.17f, 9.823f, 3f); // Untested but want to show small fire
            FirePSShape.scale = Vector3.Lerp(BigFireMin, BigFireMax, currentIntensity / 200f);
            Debug.Log("Burnt");
            ScoreManager.instance.AddScore(-10);
            MaxBurnTime += Time.deltaTime;
        }
        else if (currentIntensity >= 200f && MaxBurnTime > 20f)
        {
            ScoreManager.instance.AddScore(-15);
            Destroy(this);
        }
    }
}
