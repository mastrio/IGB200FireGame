using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    [SerializeField] private TutorialDragThingy tutorialThing;

    public void ShowTutorial()
    {
        tutorialThing.StartTutorial(gameObject);
    }
}
