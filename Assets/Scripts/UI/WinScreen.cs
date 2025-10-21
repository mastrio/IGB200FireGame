using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    void OnEnable()
    {
        Time.timeScale = 0.0f;
        scoreText.text = "<sprite index=0> " + Mathf.RoundToInt(ScoreManager.instance.score) + "!";
    }

    public void HomeButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
