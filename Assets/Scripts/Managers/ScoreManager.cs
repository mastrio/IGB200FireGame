using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private ParticleSystem scorePositiveParticles;
    [SerializeField] private ParticleSystem scoreNegativeParticles;

    private int score;
    private float particleTimer = 0.0f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

    }

    private void Start()
    {
        AddScore(0);
    }

    private void Update()
    {
        if (Time.time >= particleTimer)
        {
            scorePositiveParticles.Stop();
            scoreNegativeParticles.Stop();
        }
    }

    public void AddScore(int points)
    {
        score += points;
        ScoreText.text = score.ToString();

        particleTimer = Time.time + 1.0f;

        if (points > 0) scorePositiveParticles.gameObject.SetActive(true);
        else if (points < 0) scoreNegativeParticles.gameObject.SetActive(true);
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