using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject phoneUIObject;
    [SerializeField] private GameObject mapUIObject;
    [SerializeField] private CoolBurnStart coolburnStart;
    [SerializeField] private ClicktoMove clicktoMove;

    private PopupUIAnimation phoneUIAnim;
    private PopupUIAnimation mapUIAnim;
   

    void Start()
    {
        phoneUIAnim = phoneUIObject.GetComponent<PopupUIAnimation>();
        mapUIAnim = mapUIObject.GetComponent<PopupUIAnimation>();
    }

    public void PauseButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
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
