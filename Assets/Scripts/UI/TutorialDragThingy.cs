using UnityEngine;
using UnityEngine.UI;

public class TutorialDragThingy : MonoBehaviour
{
    static readonly private float DRAG_SPEED = 400.0f;

    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private GameObject image;
    [SerializeField] private Image imageComponent;

    private TutorialDragThingyState state = TutorialDragThingyState.Disabled;
    private float timer = 0.0f;

    private GameObject sourceObject;

    private SpringDamperVector3 scaleAnimation;
    private LerpAnimationColour colourAnimation;

    void OnEnable()
    {
        StartAnimation();
    }

    void OnDisable()
    {
        sourceObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) gameObject.SetActive(false);

        if (scaleAnimation != null) image.transform.localScale = scaleAnimation.Update(image.transform.localScale);
        if (colourAnimation != null) imageComponent.color = colourAnimation.Update(imageComponent.color);

        switch (state)
        {
            case TutorialDragThingyState.Starting: StateStarting(); break;
            case TutorialDragThingyState.Moving: StateMoving(); break;
            case TutorialDragThingyState.Waiting: StateWaiting(); break;
        }
    }

    public void StartTutorial(GameObject sourceObject)
    {
        this.sourceObject = sourceObject;
        sourceObject.SetActive(false);
        gameObject.SetActive(true);
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
        state = TutorialDragThingyState.Starting;
        image.transform.localPosition = startPos;
        timer = Time.time + 1.0f;

        image.transform.localScale = new Vector3(8.0f, 8.0f, 1.0f);
        scaleAnimation = new SpringDamperVector3(15.0f, 25.0f, Vector3.one);
        imageComponent.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        colourAnimation = new LerpAnimationColour(Color.white, 10.0f);
    }
}

enum TutorialDragThingyState
{
    Disabled,
    Starting,
    Moving,
    Waiting
}
