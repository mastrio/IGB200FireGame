using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject gameUIObject;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        gameObject.SetActive(true);
        gameUIObject.SetActive(false);
    }

    public void Unpause()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
        gameUIObject.SetActive(true);
    }

    public void UnpauseButtonPressed()
    {
        Unpause();
    }

    public void MainMenuButtonPressed()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
