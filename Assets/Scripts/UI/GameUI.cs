using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject phoneUIObject;

    private PhoneUI phoneUI;

    void Start()
    {
        phoneUI = phoneUIObject.GetComponent<PhoneUI>();
    }

    public void PauseButtonPressed()
    {
        Debug.Log("NOT paused!!!");
    }

    public void StartCoolburnButtonPressed()
    {
        Debug.Log("tbh I dont think the burn is very cool im pretty sure fire is hot actually but hey thats just my opinion maybe im wrong who knows.");
    }

    public void SprayWaterButtonPressed()
    {
        Debug.Log("psssssssssssss");
    }

    public void OpenMapButtonPressed()
    {
        Debug.Log("no, use your eyes.");
    }

    public void OpenPhoneButtonPressed()
    {
        phoneUI.OpenPhone();
    }
}
