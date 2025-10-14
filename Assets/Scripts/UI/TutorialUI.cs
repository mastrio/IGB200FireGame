using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private PopupUIAnimation popupAnimation;

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private TMP_Text counterText;

    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject forwardButton;

    private int currentTutIndex = 0;

    void Update()
    {
        counterText.text = (currentTutIndex + 1) + "/" + TutorialManager.seenClips.Count;
    }

    public void ShowTutorial(int index)
    {
        if (TutorialManager.seenClips.Count == 0) return;

        for (int i = 0; i < TutorialManager.instance.tutorialClips.Length; i++)
        {
            TutorialClip clip = TutorialManager.instance.tutorialClips[i];

            if (clip.name == TutorialManager.seenClips[index].name)
            {
                currentTutIndex = index;
                ShowTutorial(clip.name, true);
                return;
            }
        }
    }

    public void ShowTutorial(string tutorialName, bool force = false)
    {
        for (int i = 0; i < TutorialManager.instance.tutorialClips.Length; i++)
        {
            TutorialClip clip = TutorialManager.instance.tutorialClips[i];

            if (clip.name == tutorialName)
            {
                if (!force && TutorialManager.seenClips.Contains(clip)) return;

                if (!TutorialManager.seenClips.Contains(clip)) TutorialManager.seenClips.Add(clip);
                if (!force) currentTutIndex = TutorialManager.seenClips.Count - 1;

                // Hide and show buttons based on tutorial index
                if (currentTutIndex == 0) backButton.SetActive(false);
                else backButton.SetActive(true);

                if (currentTutIndex == TutorialManager.seenClips.Count - 1) forwardButton.SetActive(false);
                else forwardButton.SetActive(true);

                // Set ui shit
                videoPlayer.clip = clip.videoClip;
                tutorialText.text = clip.description;

                popupAnimation.OpenUI();

                Debug.Log("Showing tutorial \"" + tutorialName + "\"");
                return;
            }
        }

        Debug.Log("Tutorial \"" + tutorialName + "\" does not exist :(");
    }

    public void HideTutorial()
    {
        popupAnimation.CloseUI();
    }

    // Buttons

    public void BackButtonPressed()
    {
        if (currentTutIndex == 0) return;
        ShowTutorial(currentTutIndex - 1);
    }

    public void ForwardButtonPressed()
    {
        if (currentTutIndex == TutorialManager.seenClips.Count - 1) return;
        ShowTutorial(currentTutIndex + 1);
    }
}
