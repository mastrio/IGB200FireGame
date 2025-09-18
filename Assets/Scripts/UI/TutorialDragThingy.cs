using UnityEngine;

public class TutorialDragThingy : MonoBehaviour
{
    static readonly private float DRAG_SPEED = 200.0f;

    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    private TutorialDragThingyState state = TutorialDragThingyState.Disabled;
    private float timer = 0.0f;

    void OnEnable()
    {
        transform.localPosition = startPos;
        state = TutorialDragThingyState.Moving;
    }

    void OnDisable()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) gameObject.SetActive(false);

        switch (state)
        {
            case TutorialDragThingyState.Moving: StateMoving(); break;
            case TutorialDragThingyState.Waiting: StateWaiting(); break;
        }
    }

    private void StateMoving()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, DRAG_SPEED * Time.deltaTime);

        if (transform.localPosition == endPos)
        {
            timer = Time.time + 1.0f;
            state = TutorialDragThingyState.Waiting;
        }
    }

    private void StateWaiting()
    {
        if (Time.time >= timer)
        {
            transform.localPosition = startPos;
            state = TutorialDragThingyState.Moving;
        }
    }
}

enum TutorialDragThingyState
{
    Disabled,
    Moving,
    Waiting
}
