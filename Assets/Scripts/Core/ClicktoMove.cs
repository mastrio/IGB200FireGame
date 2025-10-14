using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ClickToMove : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private InputAction MouseClick;
    [SerializeField] private GameObject clickParticlePrefab;
    [SerializeField] private Animator animator;

    private Camera mainCamera;

    private NavMeshAgent navAgent;
    private NavMeshPath navPath;
    private int pathCounter;
    private LerpAnimationQuaternion rotAnim;
    private Vector3 previousPosition;

    private int groundLayer;
    public static bool movedisabled = false;

    private void Awake()
    {
        mainCamera = Camera.main;
        groundLayer = LayerMask.NameToLayer("Ground");

        navAgent = gameObject.GetComponent<NavMeshAgent>();
        navPath = new NavMeshPath();

        rotAnim = new LerpAnimationQuaternion(Quaternion.Euler(Vector3.zero), 20.0f);
        previousPosition = transform.position;
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

    void FixedUpdate()
    {
        // Calculate move speed
        float distanceMoved = Vector3.Distance(transform.position, previousPosition);
        float speed = distanceMoved / Time.deltaTime;

        // Clamp small or negative values
        if (speed < 0.05f)
            speed = 0f;
        else
            speed = Mathf.Max(speed, 0f);

        // Snap to 0 when stopped
        if (animator != null)
        {
            if (speed == 0f)
                animator.SetFloat("Speed", 0f);
            else
                animator.SetFloat("Speed", speed);
        }

        previousPosition = transform.position;

        // Path movement
        if (navPath.corners.Length == 0 || pathCounter == navPath.corners.Length) return;

        Vector3 vector = -(transform.position - navPath.corners[pathCounter]);
        Vector3 direction = vector.normalized;

        // Move
        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);

        // Rotate
        float rotDirection = Vector3.Angle(direction, Vector3.forward);
        if (direction.x < 0.0f)
        {
            rotDirection = Vector3.Angle(
                new Vector3(direction.x, -direction.y, direction.z),
                Vector3.back
            ) + 180.0f;
        }
        rotAnim.targetVal = Quaternion.Euler(new Vector3(0.0f, rotDirection, 0.0f));
        transform.rotation = rotAnim.Update(transform.rotation);

        // Go to next path point if you at the current path point
        if (vector.magnitude < 0.4f) pathCounter++;
    }

    // Checks if the mouse click is over ui 
    public bool MouseOverUi()
    {

        PointerEventData mousepointInfo = new PointerEventData(EventSystem.current);
        // Checks the mouses current position as a value
        mousepointInfo.position = Mouse.current.position.ReadValue();

        // list the graphics raycasts results and if it hit a ui element then it will be >0
        List<RaycastResult> listofrays = new List<RaycastResult>();
        EventSystem.current.RaycastAll(mousepointInfo, listofrays);

        if (listofrays.Count > 0) return true;
        else return false;

    }
    private void mouseActionCheck(InputAction.CallbackContext context)
    {
        // Checks if movement is disabled if so stop loop
        if (movedisabled) return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (MouseOverUi()) return;
        else if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) && hit.collider && hit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
        {
            if (!GameManager.instance.hasMoved)
            {
                bool result = TutorialManager.instance.tutorialUI.ShowTutorial("HowToPlaceFireTutorial");
                if (result)
                {
                    GameManager.instance.hasMoved = true;
                    return;
                }
            }

            // Set target destination
            pathCounter = 1;
            navAgent.CalculatePath(hit.point, navPath);

            Instantiate(clickParticlePrefab, hit.point + Vector3.up * 0.05f, transform.rotation);
        }
    }
}