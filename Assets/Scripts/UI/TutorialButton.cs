using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public void ShowTutorial()
    {
        TutorialManager.instance.tutorialUI.ShowTutorial(0);
    }
}
