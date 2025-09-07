using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        ScoreText.text = "Score " + score.ToString();
    }
    public void UpdateScore(int SucessfulBurn)
    {
        if (SucessfulBurn == 1) //Coolburn Managed
        {
            AddScore(150);
        }
        else if (SucessfulBurn == 2) //Burnable Managed
        {
            AddScore(75);
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