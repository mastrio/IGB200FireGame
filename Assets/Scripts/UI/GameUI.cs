using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public FireManager coolburnStart;
    [SerializeField] private GameObject mapUIObject;
    [SerializeField] private GameObject pauseUIObject;
    [SerializeField] private TextMeshProUGUI FireDangerLevelText;

    private PopupUIAnimation phoneUIAnim;
    private PopupUIAnimation mapUIAnim;
    private PauseUI pauseUI;

    private int UiFireDangerLevel;


    void Start()
    {
        mapUIAnim = mapUIObject.GetComponent<PopupUIAnimation>();
        pauseUI = pauseUIObject.GetComponent<PauseUI>();
        UiFireDangerLevel = FireManager.instance.GetFireDangerLevel();
        UpdateTextForDangerLevel(FireDangerLevelText);
    }

    private void Update()
    {
        if (UiFireDangerLevel != FireManager.instance.GetFireDangerLevel())
        {
            UpdateTextForDangerLevel(FireDangerLevelText);
        }
    }

    public void UpdateTextForDangerLevel(TextMeshProUGUI FireDangerText)
    {
        FireDangerText.text = "Danger Level: " + FireManager.instance.GetFireDangerLevel();
    }

    public void PauseButtonPressed()
    {
        pauseUI.Pause();
    }

    //Unsure if this actually does anything
    public void StartCoolburnButtonPressed()
    {
        FireManager.instance.CoolButtonTrigger();
    }

    public void OpenMapButtonPressed()
    {
        if (!GameManager.instance.hasPannedMap)
        {
            TutorialManager.instance.tutorialUI.ShowTutorial("MapOpened");
        }

        mapUIAnim.OpenUI();
    }

    public void OpenPhoneButtonPressed()
    {
        phoneUIAnim.OpenUI();
    }
}
