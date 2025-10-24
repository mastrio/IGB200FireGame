using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] public ParticleSystem scorePositiveParticles;
    [SerializeField] public ParticleSystem EmberParticles;

    [HideInInspector] public float score;

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

    public void AddScore(float points)
    {
        if (GameManager.instance.winScreenObject.activeInHierarchy)
        {
            return;
        }
        score += points;
        ScoreText.text = "<sprite index=0> " + Mathf.RoundToInt(score);
    }
}