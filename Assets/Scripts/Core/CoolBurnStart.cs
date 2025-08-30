using System;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoolBurnStart : MonoBehaviour
{
    [SerializeField] private InputAction MouseClick;
    [SerializeField] private float targetoffsetY = 0f;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject FireParticlePrefab;
    [SerializeField] private ClicktoMove clicktoMove;

    private Camera mainCamera;
    public Coroutine fireCoroutine;
    public Coroutine buttonCoroutine;
    private int coolburnLayer;

    private bool CoolbuttonPressed = false;
    private void Awake()
    {
        mainCamera = Camera.main;
        coolburnLayer = LayerMask.NameToLayer("Coolburn");
    }
    private void OnEnable()
    {
        MouseClick.Enable();
        MouseClick.performed += mouseActionCheck;
    }

    private void OnDisable()
    {
        MouseClick.Disable();
        MouseClick.performed -= mouseActionCheck;
    }

    public void CoolButtonTrigger()
    {
        buttonCoroutine = StartCoroutine(delayCoolbuttonTrigger());
        Debug.Log("True");
    }

    private IEnumerator delayCoolbuttonTrigger()
    {
        yield return null;
        CoolbuttonPressed = true;
        Debug.Log("ITWORKED");
    }

    private void mouseActionCheck(InputAction.CallbackContext context)
    {
        //only triggers if the bool is true
        if (CoolbuttonPressed)
        {
            Debug.Log("itworked");
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (clicktoMove.MouseOverUi())
            {
                CoolbuttonPressed = false;
                return;
            }
            if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit firehit) && firehit.collider && firehit.collider.gameObject.layer.CompareTo(coolburnLayer) == 0)
            {
               // float distanceFromPlayer = Vector3.Distance(player.transform.position, firehit.point);
                //if (distanceFromPlayer < 1f)
                //{
                if (fireCoroutine != null) StopCoroutine(fireCoroutine);
                fireCoroutine = StartCoroutine(FireBegin(firehit.point));

                CoolbuttonPressed = false;
                //}
            }
        }
        
    }
    //spawns the fire sligtly below the click position
    private IEnumerator FireBegin(Vector3 target)
    {
        float offsettedtarget = targetoffsetY - target.y;
        target.y += offsettedtarget;
        Instantiate(FireParticlePrefab, target, Quaternion.identity);
        yield return null;
    }
}
