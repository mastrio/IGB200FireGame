using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public FireManager coolburnStart;
    [SerializeField] private GameObject phoneUIObject;
    [SerializeField] private GameObject mapUIObject;
    [SerializeField] private GameObject pauseUIObject;
    [SerializeField] private TextMeshProUGUI FireDangerLevelText;
    [SerializeField] private GameObject coolburnButtonTutorial;
    //[SerializeField] private ClicktoMove clicktoMove;

    private PopupUIAnimation phoneUIAnim;
    private PopupUIAnimation mapUIAnim;
    private PauseUI pauseUI;
    private FireManager coolBurnStarter;

    private int UiFireDangerLevel;


    void Start()
    {
        phoneUIAnim = phoneUIObject.GetComponent<PopupUIAnimation>();
        mapUIAnim = mapUIObject.GetComponent<PopupUIAnimation>();
        pauseUI = pauseUIObject.GetComponent<PauseUI>();
        UiFireDangerLevel = FireManager.GetFireDangerLevel();
        UpdateTextForDangerLevel(FireDangerLevelText);
    }

    private void Update()
    {
        if (UiFireDangerLevel != FireManager.GetFireDangerLevel())
        {
            UpdateTextForDangerLevel(FireDangerLevelText);
        }
    }

    public void UpdateTextForDangerLevel(TextMeshProUGUI FireDangerText)
    {
        FireDangerText.text = "Danger Level: " + FireManager.GetFireDangerLevel();
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

    public void HelpButtonPressed()
    {
        coolburnButtonTutorial.SetActive(true);
    }

    public void SprayWaterButtonPressed()
    {
        coolburnStart.ShowFireSlider();
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
