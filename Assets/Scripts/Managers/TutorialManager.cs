using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public static List<TutorialClip> seenClips = new List<TutorialClip>();

    public TutorialUI tutorialUI;

    public TutorialClip[] tutorialClips;

    void Awake()
    {
        instance = this;
    }
}

[Serializable]
public struct TutorialClip
{
    public string name;
    public string description;
    public VideoClip videoClip;
}