using UnityEngine;

public class CamController : MonoBehaviour
{

    public Transform player;

    public float moveSpeed = 5f;


    public float followDistance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {

        Vector3 targetPosition = Vector3.MoveTowards(
            transform.position, player.position, moveSpeed * Time.deltaTime);
        transform.position = targetPosition;

        Vector3 targetPos = Vector3.Lerp(transform.position, player.position, moveSpeed * Time.deltaTime);

        transform.position = targetPos;

        //transform.rotation = rotation;
    }
}
