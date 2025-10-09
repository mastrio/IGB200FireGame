using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveText;

    public void ObjectiveUpdated(string newText, bool isImportant)
    {
        objectiveText.text = newText;
    }
}
