using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;

    void Start()
    {
        settingsMenu.SetActive(false);
    }

    public void PlayButtonPressed()
    {
        //SceneManager.LoadScene("g");
        Debug.Log("nah");
    }

    public void SettingsButtonPressed()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void LeaveSettingsButtonPressed()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
