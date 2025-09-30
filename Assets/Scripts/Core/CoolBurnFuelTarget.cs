using System;
using System.Collections;
using UnityEngine;


public class CoolBurnFuelTarget : MonoBehaviour
{
    private FireObject FireObjectRef;
    private bool burning = false;
    private Coroutine fireExtinguisherCoroutine;
    private float BurnTimer; 
    [SerializeField] private GameObject FireParticlePrefab;
    private ParticleSystem firePS;

    public void BeginFireIgnition(FireObject baseFireObject)
    {
        FireObjectRef = baseFireObject;
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
        fireExtinguisherCoroutine = StartCoroutine(FireExtinguisher());
    }

    private IEnumerator FireExtinguisher()
    {
        float delay = UnityEngine.Random.Range(8f, 18f);
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
        if (!burning)
        {
            return;
        }
        float currentIntensity = FireObjectRef.fireIntensity;
        

        if (currentIntensity < 100f)
        {
            Debug.Log("Do This");
        }
        else if (currentIntensity > 100f && currentIntensity < 200f)
        {
            BurnTimer += Time.deltaTime;
            Debug.Log("Do This Other Thing");
        }
        else if (currentIntensity >= 200f && BurnTimer >= 30f)
        {
            Debug.Log("Burnt");
            //burning = false;
        }
    
    }
}
