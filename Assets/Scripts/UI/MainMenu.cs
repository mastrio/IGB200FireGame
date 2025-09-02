using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject scenarioMenu;
    [SerializeField] private GameObject settingsMenu;

    void Start()
    {
        settingsMenu.SetActive(false);
        scenarioMenu.SetActive(false);

        Global.ResetData();
    }

    public void PlayButtonPressed()
    {
        mainMenu.SetActive(false);
        scenarioMenu.SetActive(true);
    }

    public void SettingsButtonPressed()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void QuitButtonPressed()
    {
        Application.Quit();
    }

    public void ReturnToMainButtonPressed()
    {
        scenarioMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
