using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FireManager : MonoBehaviour
{
    [SerializeField] private InputAction MouseClick;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject FireParticlePrefab;
    [SerializeField] private Canvas gameUICanvas;

    private bool fireSlidersVisible = false;

    private CoolburnGroundItem sliderTarget;

    private Camera mainCamera;

    public Coroutine buttonCoroutine;
    private int burnableLayer;
    private int coolburnLayer;
    private int groundLayer;

    private bool CoolbuttonPressed = false;

    public static int FireDangerLevel = 3;

    private void Awake()
    {
        mainCamera = Camera.main;
        burnableLayer = LayerMask.NameToLayer("Burnable");
        coolburnLayer = LayerMask.NameToLayer("Coolburn");
    }

    private void OnEnable()
    {
        MouseClick.Enable();
        //MouseClick.performed += mouseActionCheck;
    }

    private void OnDisable()
    {
        MouseClick.Disable();
        //MouseClick.performed -= mouseActionCheck;
    }

    public void CoolButtonTrigger()
    {
        CoolbuttonPressed = true;
        ClicktoMove.movedisabled = true;
        Debug.Log("Coolburn true move disabled");
        // buttonCoroutine = StartCoroutine(delayCoolbuttonTrigger());
    }

    private IEnumerator delayCoolbuttonTrigger()
    {
        yield return null;
        //  CoolbuttonPressed = true;
        // Debug.Log("ITWORKED");
    }

    public bool mouseActionCheck() //InputAction.CallbackContext context
    {
        bool success = false;

        //only triggers if the bool is true
        //if (!CoolbuttonPressed)
        //{
        //    return;
        //}

        player.TryGetComponent<ClicktoMove>(out ClicktoMove clicktoMove);
        Debug.Log("itworked");
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        //if (clicktoMove.MouseOverUi())
        //{
        //    CoolbuttonPressed = false;
        //    ClicktoMove.movedisabled = false;
        //    return;
        //}

        if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit burnablehit) && burnablehit.collider &&
            burnablehit.collider.gameObject.layer.CompareTo(burnableLayer) == 0)
        {
            CoolbuttonPressed = false;
            ClicktoMove.movedisabled = false;
            return success;
            // float distanceFromPlayer = Vector3.Distance(player.transform.position, firehit.point);
            //if (distanceFromPlayer < 1f)
            //{
            /*
            if (burnablehit.collider.TryGetComponent<BurnableObject>(out BurnableObject coolBurnable))
            {
                coolBurnable.BurnableIgnition(30f);
            }
            */
            //}
        }
        else if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit firehit) && firehit.collider &&
                 firehit.collider.gameObject.layer.CompareTo(coolburnLayer) == 0)
        {
            if (firehit.collider.TryGetComponent<CoolburnGroundItem>(out CoolburnGroundItem coolburnBrush))
            {
                ClicktoMove.movedisabled = true;
                coolburnBrush.CoolBurnIgnition(30f);
                success = true;
            }

        }
        else if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit groundhit) && groundhit.collider &&
                 groundhit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
        {
            if (groundhit.collider.TryGetComponent<CoolBurnManager>(out CoolBurnManager coolburnGround))
            {
                ClicktoMove.movedisabled = true;
                coolburnGround.FireIgnition(30f, groundhit.point);
            }
        }

        ClicktoMove.movedisabled = false;
        CoolbuttonPressed = false;

        Debug.Log("Cool disable Move enable");
        return success;
    }



    public void ShowFireSlider()
    {
        //flips is the button is pressed
        fireSlidersVisible = !fireSlidersVisible;
        var coolburnObjects = FindObjectsByType<CoolburnGroundItem>(FindObjectsSortMode.None);
        foreach (var coolburn in coolburnObjects)
        {
            if (coolburn.currentlyBurning)
            {
                coolburn.SetFireSliderVisible(fireSlidersVisible);
            }
        }

        var burnableObjects = FindObjectsByType<BurnableObject>(FindObjectsSortMode.None);
        foreach (var burnables in burnableObjects)
        {
            if (burnables.currentlyBurning)
            {
                burnables.SetFireSliderVisible(fireSlidersVisible);
            }
           
        }

    }

    public static void UpdateFireDangerLevel(bool CoolBurnSuccess)
    {
        if (FireDangerLevel <= 5 && FireDangerLevel >= 0)
        {
            if (CoolBurnSuccess)
            {
                FireDangerLevel += 1;
            }
            else if (!CoolBurnSuccess)
            {
                FireDangerLevel -= 1;
            }
        }
        
    }
    
    public static int GetFireDangerLevel()
    {
        return FireDangerLevel;
    }
}