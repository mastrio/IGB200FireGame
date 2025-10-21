using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] public ParticleSystem scorePositiveParticles;
    [SerializeField] public ParticleSystem EmberParticles;


    [HideInInspector] public float score;
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
        //if (Time.time >= particleTimer)
        //{
        //  scorePositiveParticles.Stop();
        //  scoreNegativeParticles.Stop();
        //}
    }

    public void AddScore(float points)
    {
        score += points;
        ScoreText.text = Mathf.RoundToInt(score).ToString();

        //particleTimer = Time.time + 1.0f;
        //if (points > 0) scorePositiveParticles.gameObject.SetActive(true);
        // else if (points < 0) scoreNegativeParticles.gameObject.SetActive(true);
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