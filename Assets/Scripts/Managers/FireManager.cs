using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class FireManager : MonoBehaviour
{
    [SerializeField] private InputAction MouseClick;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject FireParticlePrefab;
    //[SerializeField] private Canvas gameUICanvas;

    public static FireManager instance;

    private Camera mainCamera;

    public Coroutine buttonCoroutine;
    private int burnableLayer;
    private int coolburnLayer;
    private int groundLayer;

    private bool CoolbuttonPressed = false;

    [HideInInspector] public int FireDangerLevel;

    [HideInInspector] public int CurrentNumberOfFires = 0;

    private void Awake()
    {
        mainCamera = Camera.main;
        GetPlayer();
        SetFireDangerLevel();
        burnableLayer = LayerMask.NameToLayer("Burnable");
        coolburnLayer = LayerMask.NameToLayer("Coolburn");
        groundLayer = LayerMask.NameToLayer("Ground");

        if (instance == null) 
        { 
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
    }

    public void GetPlayer()
    {
        GameObject FindPlayer = GameObject.FindWithTag("Player");

        if (FindPlayer != null)
        {
            player = FindPlayer;
        }
        else
        {
            Debug.Log("No Player in scene");
        }
    }

    public void NewSceneCamera()
    {
        

        if (!mainCamera == Camera.main)
        {
            mainCamera = Camera.main; ;
        }
        else
        {
            Debug.Log("No New Camera in scene");
        }
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
        
        ClickToMove.movedisabled = true;
        // buttonCoroutine = StartCoroutine(delayCoolbuttonTrigger());
    }

    private IEnumerator delayCoolbuttonTrigger()
    {
        yield return null;
        //  CoolbuttonPressed = true;
        // Debug.Log("ITWORKED");
    }

    public bool mouseActionCheck(GameObject fireObjectPrefab) //InputAction.CallbackContext context
    {
        bool success = false;
        if (CurrentNumberOfFires >= 2)
        {
            ClickToMove.movedisabled = false;
            CoolbuttonPressed = false;
            return success;
        }


        player.TryGetComponent<ClickToMove>(out ClickToMove clickToMove);
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
      
        Physics.Raycast(ray: ray, hitInfo: out RaycastHit rayHit);
        int hitLayer = rayHit.collider.gameObject.layer;

        if (hitLayer == groundLayer || hitLayer == coolburnLayer)
        {
            IncreaseNumberOfFires();
            Vector3 rayHitPos = rayHit.point;
            Vector3 spawnPos = new Vector3(rayHitPos.x, 0f, rayHitPos.z);
            Instantiate(fireObjectPrefab, spawnPos, Quaternion.Euler(Vector3.zero));
            success = true;
        }
        ClickToMove.movedisabled = false;
        CoolbuttonPressed = false;
        return success;
    }
    public void UpdateFireDangerLevel(bool CoolBurnFail)
    {
        if (FireDangerLevel < 6 && FireDangerLevel > 0)
        {
            if (CoolBurnFail)
            {
                FireDangerLevel += 1;
            }
            else if (!CoolBurnFail)
            {
                FireDangerLevel -= 1;
            }
        }

    }
    public int GetFireDangerLevel()
    {
        return FireDangerLevel;
    }

    public void SetFireDangerLevel()
    {
        FireDangerLevel = 6;
    }

    public void IncreaseNumberOfFires()
    {
        CurrentNumberOfFires += 1;
    }

    public void ReduceNumberOfFires()
    {
        CurrentNumberOfFires -= 1;
    }
    
    public int GetNumberOfFires()
    {
        return CurrentNumberOfFires;
    }

    public void ResetNumOfFires()
    {
        CurrentNumberOfFires = 0;
    }
}
//OLD SYSTEM
/*

public bool mouseActionCheck(GameObject fireObjectPrefab) //InputAction.CallbackContext context
{
    bool success = false;
    if (CurrentNumberOfFires >= 2)
    {
        ClickToMove.movedisabled = false;
        CoolbuttonPressed = false;
        return success;
    }




    //only triggers if the bool is true
    //if (!CoolbuttonPressed)
    //{
    //    return;
    //}

    player.TryGetComponent<ClickToMove>(out ClickToMove clickToMove);
    Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
    //if (clicktoMove.MouseOverUi())
    //{
    //    CoolbuttonPressed = false;
    //    ClicktoMove.movedisabled = false;
    //    return;
    //}

    // Spawn fire object
    Physics.Raycast(ray: ray, hitInfo: out RaycastHit rayHit);
    int hitLayer = rayHit.collider.gameObject.layer;

    if (hitLayer == groundLayer || hitLayer == coolburnLayer)
    {
        IncreaseNumberOfFires();
        Instantiate(fireObjectPrefab, rayHit.point, Quaternion.Euler(Vector3.zero));
        success = true;
    }

    // Old system
    /*
    if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit burnablehit) && burnablehit.collider &&
        burnablehit.collider.gameObject.layer.CompareTo(burnableLayer) == 0)
    {
        CoolbuttonPressed = false;
        ClicktoMove.movedisabled = false;
        return success;
        // float distanceFromPlayer = Vector3.Distance(player.transform.position, firehit.point);
        //if (distanceFromPlayer < 1f)
        //{
        //if (burnablehit.collider.TryGetComponent<BurnableObject>(out BurnableObject coolBurnable))
        //{
        //    coolBurnable.BurnableIgnition(30f);
        //}
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


    ClickToMove.movedisabled = false;
    CoolbuttonPressed = false;
    return success;
}

/*public void ShowFireSlider()
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

}*/
