using Unity.VisualScripting;
using UnityEngine;

public class PopupUIAnimation : MonoBehaviour
{
    [SerializeField] private GameObject motionRootObject;
    [SerializeField] private GameObject bgCloseButtonObject;

    [DoNotSerialize] public bool open = false;

    private SpringDamperVector3 movementAnimation;
    private Vector3 startPos;

    void Start()
    {
        startPos = motionRootObject.transform.localPosition;

        gameObject.SetActive(false);
    }

    void Update()
    {
        // Movement Animation
        if (movementAnimation == null) return;
        motionRootObject.transform.localPosition = movementAnimation.Update(motionRootObject.transform.localPosition);

        if (motionRootObject.transform.localPosition.y < -600.0f)
        {
            gameObject.SetActive(false);
            movementAnimation = null;
        }
    }

    public void BgCloseButtonPressed()
    {
        CloseUI();
    }

    public void OpenUI()
    {
        open = true;
        gameObject.SetActive(true);
        if (bgCloseButtonObject != null) bgCloseButtonObject.SetActive(true);

        movementAnimation = new SpringDamperVector3(
            35.0f,
            40.0f,
            startPos
        );
        motionRootObject.transform.localPosition = startPos - new Vector3(0.0f, 600.0f);
    }

    public void CloseUI()
    {
        open = false;
        if (bgCloseButtonObject != null) bgCloseButtonObject.SetActive(false);

        movementAnimation = new SpringDamperVector3(
            40.0f,
            40.0f,
            startPos + new Vector3(0.0f, -600.0f)
        );
    }
}
