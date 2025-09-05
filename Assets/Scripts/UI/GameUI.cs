using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject phoneUIObject;
    [SerializeField] private GameObject mapUIObject;
    [SerializeField] private GameObject pauseUIObject;
    [SerializeField] private CoolBurnStart coolburnStart;
    //[SerializeField] private ClicktoMove clicktoMove;

    private PopupUIAnimation phoneUIAnim;
    private PopupUIAnimation mapUIAnim;
    private PauseUI pauseUI;
    private CoolBurnStart coolBurnStarter;

    void Start()
    {
        phoneUIAnim = phoneUIObject.GetComponent<PopupUIAnimation>();
        mapUIAnim = mapUIObject.GetComponent<PopupUIAnimation>();
        pauseUI = pauseUIObject.GetComponent<PauseUI>();
    }

    public void PauseButtonPressed()
    {
        pauseUI.Pause();
    }

    public void StartCoolburnButtonPressed()
    {
     
        //sets bool to true to spawn fire on next click
        coolburnStart.CoolButtonTrigger();
        //  clicktoMove.disablemoveButtonPress();
        Debug.Log("pressed");
    }

    public void SprayWaterButtonPressed()
    {
        Debug.Log("psssssssssssss");
    }

    public void OpenMapButtonPressed()
    {
        mapUIAnim.OpenUI();
    }

    public void OpenPhoneButtonPressed()
    {
        phoneUIAnim.OpenUI();
    }
}
