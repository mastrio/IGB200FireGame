using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    void Start()
    {
        if (Global.scenarioNum != 3) gameObject.SetActive(false);
    }

    public void ShowTutorial()
    {
        TutorialManager.instance.tutorialUI.ShowTutorial(0);
    }
}
