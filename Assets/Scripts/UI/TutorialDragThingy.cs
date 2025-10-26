using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDragThingy : MonoBehaviour
{
    static readonly private float DRAG_SPEED = 400.0f;

    [SerializeField] private bool manuallyDisabled = false;
    [SerializeField] private float startWaitTime;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private GameObject image;
    [SerializeField] private Image imageComponent;

    private TutorialDragThingyState state = TutorialDragThingyState.Disabled;
    private float timer = 0.0f;
    private bool hasDoneDelay = false;

    private SpringDamperVector3 scaleAnimation;
    private LerpAnimationColour colourAnimation;

    void Start()
    {
        if (Global.scenarioNum != 1) Destroy(gameObject);

        if (gameObject.activeSelf) StartAnimation();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !manuallyDisabled && Time.timeScale != 0.0f) Destroy(gameObject);

        if (state == TutorialDragThingyState.DelayedStart)
        {
            if (Time.time >= timer)
            {
                state = TutorialDragThingyState.Starting;
                timer = Time.time + 1.0f;
            }
            else
            {
                image.transform.localScale = Vector3.zero;
            }
        }

        if (scaleAnimation != null) image.transform.localScale = scaleAnimation.Update(image.transform.localScale);
        if (colourAnimation != null) imageComponent.color = colourAnimation.Update(imageComponent.color);

        switch (state)
        {
            case TutorialDragThingyState.Starting: StateStarting(); break;
            case TutorialDragThingyState.Moving: StateMoving(); break;
            case TutorialDragThingyState.Waiting: StateWaiting(); break;
        }
    }

    void FixedUpdate()
    {
    }

    public void StartTutorial()
    {
        StartAnimation();
    }

    private void StateStarting()
    {
        if (Time.time >= timer)
        {
            scaleAnimation = null;
            state = TutorialDragThingyState.Moving;
        }
    }

    private void StateMoving()
    {
        image.transform.localPosition = Vector3.MoveTowards(image.transform.localPosition, endPos, DRAG_SPEED * Time.deltaTime);

        if (image.transform.localPosition == endPos)
        {
            timer = Time.time + 1.0f;
            state = TutorialDragThingyState.Waiting;
        }
    }

    private void StateWaiting()
    {
        if (Time.time >= timer)
        {
            StartAnimation();
        }
    }

    private void StartAnimation()
    {
        if (startWaitTime > 0.0f && !hasDoneDelay)
        {
            state = TutorialDragThingyState.DelayedStart;
            timer = Time.time + startWaitTime;
            hasDoneDelay = true;
        }
        else
        {
            state = TutorialDragThingyState.Starting;
            timer = Time.time + 1.0f;
        }

        gameObject.SetActive(true);
        image.transform.localPosition = startPos;
        image.transform.localScale = new Vector3(8.0f, 8.0f, 1.0f);
        scaleAnimation = new SpringDamperVector3(15.0f, 25.0f, Vector3.one);
        imageComponent.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        colourAnimation = new LerpAnimationColour(Color.white, 10.0f);
    }
}

enum TutorialDragThingyState
{
    Disabled,
    DelayedStart,
    Starting,
    Moving,
    Waiting
}
