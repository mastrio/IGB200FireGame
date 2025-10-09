using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    [SerializeField] private ObjectiveUI objectiveDisplay;

    void Awake()
    {
        instance = this;
        SetObjective("You shouldn't be here.");
    }

    public void SetObjective(string text, bool important = false)
    {
        if (objectiveDisplay != null) objectiveDisplay.ObjectiveUpdated(text, important);
    }
}
