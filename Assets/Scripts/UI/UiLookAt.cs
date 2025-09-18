using UnityEngine;

public class UiLookAt : MonoBehaviour
{

    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 lookAtCamera = mainCamera.transform.position;
        lookAtCamera.y = transform.position.y;
        transform.LookAt(lookAtCamera);
    }


}
