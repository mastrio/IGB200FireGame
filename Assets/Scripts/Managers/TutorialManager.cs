using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public static List<TutorialClip> seenClips = new List<TutorialClip>();
    public static List<string> tutorialQueue = new List<string>();

    public TutorialUI tutorialUI;

    public TutorialClip[] tutorialClips;

    void Awake()
    {
        instance = this;
        seenClips.Clear();
    }

    public void DisplayTutorial(string tutorialName, bool force = false, float delay = 0.0f)
    {
        StartCoroutine(DisplayTutorialThingy(tutorialName, force, delay));
    }

    private IEnumerator DisplayTutorialThingy(string tutorialName, bool force = false, float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);

        tutorialUI.ShowTutorial(tutorialName, force);

        yield return null;
    }
}

[Serializable]
public struct TutorialClip
{
    public string name;
    public string description;
    public VideoClip videoClip;
}