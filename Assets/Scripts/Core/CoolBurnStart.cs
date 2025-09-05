using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoolBurnStart : MonoBehaviour
{
    [SerializeField] private InputAction MouseClick;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject FireParticlePrefab;

    private Camera mainCamera;

    public Coroutine buttonCoroutine;
    private int burnableLayer;
    private int coolburnLayer;

    private bool CoolbuttonPressed = false;
    private void Awake()
    {
        mainCamera = Camera.main;
        burnableLayer = LayerMask.NameToLayer("Burnable");
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

    private void mouseActionCheck(InputAction.CallbackContext context)
    {
        //only triggers if the bool is true
        if (!CoolbuttonPressed)
        {
            return;
        }
        
        player.TryGetComponent<ClicktoMove>(out ClicktoMove clicktoMove);
        Debug.Log("itworked");
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (clicktoMove.MouseOverUi())
        {
            CoolbuttonPressed = false;
            ClicktoMove.movedisabled = false;
            return;
        }
        if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit burnablehit) && burnablehit.collider && burnablehit.collider.gameObject.layer.CompareTo(burnableLayer) == 0)
        {
            // float distanceFromPlayer = Vector3.Distance(player.transform.position, firehit.point);
            //if (distanceFromPlayer < 1f)
            //{

            if (burnablehit.collider.TryGetComponent<BurnableObject>(out BurnableObject coolBurnable))
            {
                coolBurnable.CoolBurnIgnition(30f);
            }

            //}
        }
        else if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit firehit) && firehit.collider &&
                 firehit.collider.gameObject.layer.CompareTo(coolburnLayer) == 0)
        {
            if (firehit.collider.TryGetComponent<CoolburnGroundItem>(out CoolburnGroundItem coolburnGround))
            {
                ClicktoMove.movedisabled = true;
                coolburnGround.CoolBurnIgnition(30f);
            }

        }
        ClicktoMove.movedisabled = false;
        CoolbuttonPressed = false;
       
        Debug.Log("Cool disable Move enable");
    }

    


}
