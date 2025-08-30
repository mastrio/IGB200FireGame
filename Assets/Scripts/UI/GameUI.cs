using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject phoneUIObject;
    [SerializeField] private GameObject mapUIObject;

    private PopupUIAnimation phoneUIAnim;
    private PopupUIAnimation mapUIAnim;

    void Start()
    {
        phoneUIAnim = phoneUIObject.GetComponent<PopupUIAnimation>();
        mapUIAnim = mapUIObject.GetComponent<PopupUIAnimation>();
    }

    public void PauseButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
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
        mapUIAnim.OpenUI();
    }

    public void OpenPhoneButtonPressed()
    {
        phoneUIAnim.OpenUI();
    }
}
