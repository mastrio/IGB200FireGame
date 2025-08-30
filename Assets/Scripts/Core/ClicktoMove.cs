using System;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
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
    [SerializeField] private GameObject FireParticlePrefab;
    private Camera mainCamera;
    private Coroutine coroutine;
    private Coroutine fireCoroutine;
    //Will be used if swap is made after prototype
    //private CharacterController cc;
    private Rigidbody rb;

    private int groundLayer;
    private int coolburnLayer;
    private void Awake()
    {
        mainCamera = Camera.main;
        //cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        groundLayer = LayerMask.NameToLayer("Ground");
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

    private void mouseActionCheck(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) && hit.collider && hit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
        {
            //Stops if player is already midway through click
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(PlayerMoveTowards(hit.point));
        }
        else if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit firehit) && firehit.collider && firehit.collider.gameObject.layer.CompareTo(coolburnLayer) == 0)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, hit.point);
           
            if (fireCoroutine != null) StopCoroutine(fireCoroutine);
            fireCoroutine = StartCoroutine(FireBegin(hit.point));
        
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
    private IEnumerator FireBegin(Vector3 target)
    {
        Instantiate(FireParticlePrefab, target, Quaternion.identity);
        yield return null;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
