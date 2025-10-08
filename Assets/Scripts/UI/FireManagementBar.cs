using UnityEngine;

public class FireManagementBar : MonoBehaviour
{
    [SerializeField] private GameObject infoPosObj;
    [SerializeField] private GameObject minigamePosObj;
    [SerializeField] private GameObject bgButton;
    [SerializeField] private GameObject fireLevelObject;
    [SerializeField] private ObjectShaker objectShaker;

    [HideInInspector] public FireObject fireObject;

    private LerpAnimationVector3 posAnimation;
    private LerpAnimationQuaternion rotAnimation;
    private LerpAnimationVector3 scaleAnimation;
    private PulseAnimationVector3 pulseAnimation;

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

    void OnEnable()
    {
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

        switch (State)
        {
            case FireBarState.Info: StateInfo(); break;
            case FireBarState.Minigame: StateMinigame(); break;
        }

        fireLevelObject.transform.localPosition = new Vector3(
            0.0f,
            (fireObject.fireIntensity - 100.0f) * 2.3f
        );
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
                if (fireObject.fireIntensity > 0.0f) fireObject.fireIntensity -= 5.0f;
                break;
        }
    }

    private void StateInfo()
    {
        if (fireObject.fireIntensity > 100.0f)
        {
            objectShaker.SetSharpShake(Mathf.Clamp(fireObject.fireIntensity - 100.0f, 0.0f, 100.0f) * 0.15f);
        }

        transform.localScale = new Vector3(
            Mathf.Sin(Time.time * 2.0f) * 0.02f,
            Mathf.Sin(Time.time * 2.0f) * 0.02f
        ) + Vector3.one;
    }

    private void StateMinigame()
    {
        //if (fireObject.fireIntensity > 100.0f)
        //{
        //    objectShaker.SetSharpShake(Mathf.Clamp(fireObject.fireIntensity - 100.0f, 0.0f, 100.0f) * 0.01f);
        //}
    }
}

public enum FireBarState
{
    Info,
    Minigame
}