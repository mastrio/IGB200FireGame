using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    public float RotateSpeed = 1.0f;
    private Material skyboxInstance;

    void Start()
    {
        skyboxInstance = new Material(RenderSettings.skybox); // Clone skybox material so edits are not made to the original asset.
        RenderSettings.skybox = skyboxInstance;
    }

    void Update()
    {
        skyboxInstance.SetFloat("_Rotation", Time.time * RotateSpeed); // Rotate the cloned instance only.
    }
}