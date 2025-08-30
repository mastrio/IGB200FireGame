using UnityEngine;
using UnityEngine.UI;

public class PopupUIAnimation : MonoBehaviour
{
    [SerializeField] private GameObject motionRootObject;
    [SerializeField] private GameObject bgCloseButtonObject;

    private SpringDamperVector3 movementAnimation;
    //private SpringDamperVector3 scaleAnimation;

    //private Color targetColour = Color.white;
    //private float colourLerpSpeed = 4.0f;

    private Vector3 startPos;

    void Start()
    {
        startPos = motionRootObject.transform.localPosition;
        //motionRootObject.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
        
        gameObject.SetActive(false);
    }

    void Update()
    {
        // Alpha Lerp
        //for (int i = 0; i < motionRootObject.transform.childCount; i++)
        //{
        //    Image childImage = motionRootObject.transform.GetChild(i).gameObject.GetComponent<Image>();
        //    float blend = 1 - Mathf.Pow(colourLerpSpeed, Time.deltaTime * colourLerpSpeed); // this equation makes a blend value for the lerp function that SHOULD work correctly no matter the framerate
        //    childImage.color = Color.Lerp(childImage.color, targetColour, blend);
        //}

        // Movement Animation
        if (movementAnimation == null) return;
        motionRootObject.transform.localPosition = movementAnimation.Update(motionRootObject.transform.localPosition);
        //motionRootObject.transform.localScale = scaleAnimation.Update(motionRootObject.transform.localScale);

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
        gameObject.SetActive(true);
        bgCloseButtonObject.SetActive(true);

        //targetColour = Color.white;

        movementAnimation = new SpringDamperVector3(
            35.0f,
            40.0f,
            startPos
        );
        motionRootObject.transform.localPosition = startPos - new Vector3(0.0f, 600.0f);

        //scaleAnimation = new SpringDamperVector3(
        //    35.0f,
        //    40.0f,
        //    Vector3.one
        //);
    }

    public void CloseUI()
    {
        bgCloseButtonObject.SetActive(false);

        //targetColour = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        movementAnimation = new SpringDamperVector3(
            40.0f,
            40.0f,
            startPos + new Vector3(0.0f, -600.0f)
        );

        //scaleAnimation = new SpringDamperVector3(
        //    40.0f,
        //    40.0f,
        //    new Vector3(0.0f, 0.0f, 1.0f)
        //);
    }
}
