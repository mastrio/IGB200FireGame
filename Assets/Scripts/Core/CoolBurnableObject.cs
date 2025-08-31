using System;
using UnityEngine;

public class CoolBurnableObject : MonoBehaviour
{
    private float fireIntensity = 0f;
    [SerializeField] private GameObject FireParticlePrefab;
    private ParticleSystem firePS;

    //New Method for starting cool Burn, Will allow for slider and managment
    public void CoolBurnIgnition(float startBurnIntensity)
    {
        //sets the fire intensity at start of coolburn ignition
        fireIntensity = startBurnIntensity;
        Debug.Log("Couldnt Get Past");
        //Checks if object already on fire
        if (firePS == null)
        {
            GameObject fireinstance = Instantiate(FireParticlePrefab, transform.position, Quaternion.identity,transform);
           
            firePS = fireinstance.GetComponent<ParticleSystem>();
        }
            

    }
    

    //Need to link to slider but allows the size to be edited
    public void UpdateCoolBurnIntensity(float newIntensity)
    {
        fireIntensity = newIntensity;

        if (firePS != null)
        {
            //variable for main settings of particle systems 
            var psSize = firePS.main;
            psSize.startSize = Mathf.Lerp(0.4f, 4f, fireIntensity / 100f);
        }
    }

    //Checks if object is still burning
    public bool CurrentlyBurning()
    {
        if (fireIntensity > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
