using UnityEngine;

public class FireManagementBar : MonoBehaviour
{
    [SerializeField] private GameObject infoPosObj;
    [SerializeField] private GameObject minigamePosObj;
    [SerializeField] private GameObject bgButton;
    [SerializeField] private GameObject fireLevelObject;
    [SerializeField] private GameObject tooColdText;
    [SerializeField] private GameObject tooHotText;
    [SerializeField] private ObjectShaker objectShaker;

    [HideInInspector] public FireObject fireObject;

    private LerpAnimationVector3 posAnimation;
    private LerpAnimationQuaternion rotAnimation;
    private LerpAnimationVector3 scaleAnimation;
    private PulseAnimationVector3 pulseAnimation;

    private LerpAnimationVector3 fireLevelAnimation;

    private bool canDoTheThingNowOkayYeah = false;

    private FireBarState state = FireBarState.Info;
    [HideInInspector]
    public FireBarState State
    {
        get { return state; }
        set
        {
            state = value;

            switch (state)
            {
                case FireBarState.Info:
                    bgButton.SetActive(false);
                    posAnimation = new LerpAnimationVector3(infoPosObj.transform.position, 20.0f);
                    rotAnimation = new LerpAnimationQuaternion(infoPosObj.transform.rotation, 20.0f);
                    scaleAnimation = new LerpAnimationVector3(infoPosObj.transform.localScale, 20.0f);
                    pulseAnimation = null;
                    break;

                case FireBarState.Minigame:
                    bgButton.SetActive(true);
                    posAnimation = new LerpAnimationVector3(minigamePosObj.transform.position, 20.0f);
                    rotAnimation = new LerpAnimationQuaternion(minigamePosObj.transform.rotation, 20.0f);
                    scaleAnimation = null;
                    pulseAnimation = new PulseAnimationVector3(
                        new Vector3(1.5f, 1.5f, 1.0f),
                        new Vector3(0.2f, 0.2f, 0.0f),
                        new Vector3(2.2f, 2.2f, 1.0f),
                        25.0f
                    );
                    break;
            }
        }
    }

    void Start()
    {
        fireLevelAnimation = new LerpAnimationVector3(Vector3.zero, 30.0f);
    }

    void OnEnable()
    {
        if (TutorialManager.instance != null && GameManager.instance != null)
        {
            if (!GameManager.instance.hasManagedFire)
            {
                if (canDoTheThingNowOkayYeah) TutorialManager.instance.tutorialUI.QueueTutorial("FireManagement");
                else canDoTheThingNowOkayYeah = true;
            }
        }

        bgButton.SetActive(false);
        State = FireBarState.Info;

        transform.position = infoPosObj.transform.position;
        transform.rotation = infoPosObj.transform.rotation;
        transform.localScale = infoPosObj.transform.localScale;
    }

    void Update()
    {
        if (posAnimation != null) transform.position = posAnimation.Update(transform.position);
        if (rotAnimation != null) transform.rotation = rotAnimation.Update(transform.rotation);
        if (scaleAnimation != null) transform.localScale = scaleAnimation.Update(transform.localScale);
        if (pulseAnimation != null) transform.localScale = pulseAnimation.Update(transform.localScale);

        fireLevelAnimation.targetVal = new Vector3(0.0f, -230.0f, 0.0f) + new Vector3(
            0.0f,
            Mathf.Clamp((fireObject.fireIntensity / fireObject.MaxFireIntensity) * 460.0f, 0.0f, 460.0f)
        );
        fireLevelObject.transform.localPosition = fireLevelAnimation.Update(fireLevelObject.transform.localPosition);

        if (fireObject.fireIntensity <= 60.0f) tooColdText.SetActive(true);
        else tooColdText.SetActive(false);

        if (fireObject.fireIntensity >= 140.0f) tooHotText.SetActive(true);
        else tooHotText.SetActive(false);

        switch (State)
        {
            case FireBarState.Info: StateInfo(); break;
            case FireBarState.Minigame: StateMinigame(); break;
        }
    }

    public void ClickyClicked()
    {
        switch (state)
        {
            case FireBarState.Info:
                State = FireBarState.Minigame;
                GameManager.instance.hasManagedFire = true;
                break;
            case FireBarState.Minigame:
                pulseAnimation.Pulse();
                objectShaker.ApplySharpShake(4.0f);
                if (fireObject.fireIntensity > 0.0f) fireObject.fireIntensity -= 10.0f;
                break;
        }
    }

    private void StateInfo()
    {
        if (fireObject.fireIntensity > 135.0f)
        {
            objectShaker.SetSharpShake(Mathf.Clamp(fireObject.fireIntensity - 135.0f, 0.0f, 100.0f) * 0.15f);
        }

        transform.localScale = new Vector3(
            Mathf.Sin(Time.time * 2.0f) * 0.02f,
            Mathf.Sin(Time.time * 2.0f) * 0.02f
        ) + Vector3.one;
    }

    private void StateMinigame()
    {
        tooColdText.SetActive(false);
        tooHotText.SetActive(false);
    }
}

public enum FireBarState
{
    Info,
    Minigame
}