using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Adapts Tutorial "Click to Move in 3d 2/ Input System - Unity Tutorial" by SamYam
// https://youtu.be/zZDiC0aOXDY?si=nLyKQO07EBWIuBTb
//Full refrence present in refrence list within ReadMe file

//[RequireComponent(typeof(CharacterController))]

//Might Change from rb to character controller
[RequireComponent(typeof(Rigidbody))]
public class ClicktoMove : MonoBehaviour
{
    [SerializeField] private InputAction MouseClick;
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float rotationSpeed = 3f;

    private Camera mainCamera;
    private Coroutine coroutine;

    //Will be used if swap is made after prototype
    //private CharacterController cc;
    private Rigidbody rb;

    private int groundLayer;
    private int uiLayer;
    private bool movedisabledButtonPress = false;
    private void Awake()
    {
        mainCamera = Camera.main;
        //cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        groundLayer = LayerMask.NameToLayer("Ground");
        uiLayer = LayerMask.NameToLayer("UI");
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
    //not done yet
    public void disablemoveButtonPress()
    {
        movedisabledButtonPress = true;
    }

    //Checks if the mouse click is over ui 
    public bool MouseOverUi()
    {

        PointerEventData mousepointInfo = new PointerEventData(EventSystem.current);
        //Checks the mouses current position as a value
        mousepointInfo.position = Mouse.current.position.ReadValue();

        //list the graphics raycasts results and if it hit a ui element then it will be >0
        List<RaycastResult> listofrays = new List<RaycastResult>();
        EventSystem.current.RaycastAll(mousepointInfo, listofrays);
        if (listofrays.Count > 0)
        {
            Debug.Log("true");
            return true;
        }
        else
        {
            Debug.Log("false");
            return false;
        }

    }
    private void mouseActionCheck(InputAction.CallbackContext context)
    {
        if (!movedisabledButtonPress)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (MouseOverUi())
            {
                return;
            }
            else if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) && hit.collider &&
                     hit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
            {
                //Stops if player is already midway through click
                if (coroutine != null) StopCoroutine(coroutine);
                coroutine = StartCoroutine(PlayerMoveTowards(hit.point));
            }
        }
    }


    private IEnumerator PlayerMoveTowards(Vector3 target)
    {
        float playerDistanceToFloor = transform.position.y - target.y;
        target.y += playerDistanceToFloor;
        while (Vector3.Distance(transform.position, target) > 1f)
        {
            Vector3 destination = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);
            //transform.position = destination;

            Vector3 direction = target - transform.position;
            Vector3 movement = direction.normalized * (playerSpeed * Time.deltaTime);

            //Want to look at changing to character controller and navmesh for pathfinding after prototype
            //CharacterController.Move(movement);

            rb.linearVelocity = direction.normalized * playerSpeed;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized),
                rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
