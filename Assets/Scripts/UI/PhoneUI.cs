using UnityEngine;
using UnityEngine.UI;

public class PhoneUI : MonoBehaviour
{
    [SerializeField] private GameObject motionRootObject;
    [SerializeField] private GameObject bgCloseButtonObject;

    private SpringDampenerVector3 movementAnimation;

    private Color targetColour = Color.white;
    private float colourLerpSpeed = 8.0f;

    private Vector3 startPos;

    void Start()
    {
        startPos = motionRootObject.transform.localPosition;
        
        gameObject.SetActive(false);
    }

    void Update()
    {
        // Alpha Lerp
        for (int i = 0; i < motionRootObject.transform.childCount; i++)
        {
            Image childImage = motionRootObject.transform.GetChild(i).gameObject.GetComponent<Image>();
            childImage.color = Color.Lerp(childImage.color, targetColour, colourLerpSpeed * Time.deltaTime);
        }

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
        ClosePhone();
    }

    public void OpenPhone()
    {
        gameObject.SetActive(true);
        bgCloseButtonObject.SetActive(true);

        targetColour = Color.white;

        movementAnimation = new SpringDampenerVector3(
            2.5f,
            14.0f,
            startPos
        );
        motionRootObject.transform.localPosition = startPos - new Vector3(0.0f, 600.0f);
    }

    public void ClosePhone()
    {
        bgCloseButtonObject.SetActive(false);

        targetColour = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        movementAnimation = new SpringDampenerVector3(
            3.0f,
            14.0f,
            startPos + new Vector3(0.0f, -600.0f)
        );
    }
}
