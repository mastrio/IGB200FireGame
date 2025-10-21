using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject[] hideableObjects;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        gameObject.SetActive(true);

        foreach (GameObject thing in hideableObjects)
        {
            thing.SetActive(false);
        }
    }

    public void Unpause()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);

        foreach (GameObject thing in hideableObjects)
        {
            thing.SetActive(true);
        }
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

    public void RestartButtonPressed()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Game");
    }
}
