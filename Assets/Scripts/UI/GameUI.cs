using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject phoneUIObject;
    [SerializeField] private GameObject mapUIObject;
    [SerializeField] private GameObject pauseUIObject;
    [SerializeField] private GameObject coolburnStart;
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
        coolBurnStarter = coolburnStart.GetComponent<CoolBurnStart>();
    }

    public void PauseButtonPressed()
    {
        pauseUI.Pause();
    }

    public void StartCoolburnButtonPressed()
    {
        //sets bool to true to spawn fire on next click
         coolBurnStarter.CoolButtonTrigger();
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
