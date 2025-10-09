using UnityEngine;

public class Scenario : MonoBehaviour
{
    [SerializeField] private string objectiveText;

    void Start()
    {
        ObjectiveManager.instance.SetObjective(objectiveText);
    }
}
