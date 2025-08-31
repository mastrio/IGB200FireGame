using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void UnpauseButtonPressed()
    {
        gameObject.SetActive(false);
    }

    public void MainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
