using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private TextMeshProUGUI ScoreText;
    private int score;

    //public TextMeshProUGUI highScore;

    private void Start()
    {
        AddScore(0);
    }

    public void AddScore(int points)
    {
        score += points;

        ScoreText.text = score.ToString();
    }
    public void UpdateScore(int SucessfulBurn) //Change to switch
    {
        if (SucessfulBurn == 1) //Coolburn Managed
        {
            AddScore(150);
        }
        else if (SucessfulBurn == 2) //Burnable Managed
        {
            AddScore(75);
        }
        else if (SucessfulBurn == 3)
        {
            AddScore(-10);
        }
        else if (SucessfulBurn == 4)
        {
            AddScore(90);
        }
        else if (SucessfulBurn == 5)
        {
            AddScore(50);
        }
    }

    /*
        public void AddHighScore()
        {
            if (score > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", score);
                highScore.text = score.ToString();
            }
        }

        public void Reset()
        {
            PlayerPrefs.DeleteKey("HighScore");
            highScore.text = "Highscore: None";
        }
    */
}