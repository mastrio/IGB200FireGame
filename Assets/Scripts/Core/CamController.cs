using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public float followDistance;

    [SerializeField] private GameObject cameraObject;

    private Camera cameraComponent;

    private Vector3 basePos;
    private Quaternion baseRot;
    private float baseFov;

    private Vector3 targetPos;
    private Quaternion targetRot;
    private float targetFov;

    void Awake()
    {
        cameraComponent = cameraObject.GetComponent<Camera>();

        basePos = cameraObject.transform.localPosition;
        baseRot = cameraObject.transform.localRotation;
        baseFov = cameraComponent.fieldOfView;
    }

    void Update()
    {
        // Set target position and rotation
        if (GameManager.instance.playerDraggingFireButton)
        {
            targetPos = new Vector3(0.0f, basePos.y + 10.0f, 0.0f);
            targetRot = Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f));
            targetFov = 50.0f;
        }
        else
        {
            targetPos = basePos;
            targetRot = baseRot;
            targetFov = baseFov;
        }

        cameraObject.transform.localPosition = Vector3.Lerp(
            cameraObject.transform.localPosition,
            targetPos,
            15.0f * Time.deltaTime
        );
        cameraObject.transform.localRotation = Quaternion.Lerp(
            cameraObject.transform.localRotation,
            targetRot,
            15.0f * Time.deltaTime
        );
        cameraComponent.fieldOfView = Mathf.Lerp(
            cameraComponent.fieldOfView,
            targetFov,
            15.0f * Time.deltaTime
        );
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = Vector3.MoveTowards(
            transform.position, player.position, moveSpeed * Time.deltaTime
        );

        transform.position = targetPosition;
        Vector3 targetPos = Vector3.Lerp(transform.position, player.position, moveSpeed * Time.deltaTime);
        transform.position = targetPos;
    }
}
