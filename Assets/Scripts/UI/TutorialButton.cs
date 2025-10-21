using System.Linq;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    void Start()
    {
        if (Global.scenarioNum != 1) // If not in tutorial scenario
        {
            foreach (TutorialClip clip in TutorialManager.instance.tutorialClips)
            {
                TutorialManager.seenClips.Add(clip);
            }
        }
    }

    public void ShowTutorial()
    {
        TutorialManager.instance.tutorialUI.ShowTutorial(0);
    }
}
