using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioButton : MonoBehaviour
{
    [SerializeField] private int scenarioNumber = 0;

    void Start()
    {
        if (Global.playtestMode)
        {
            if (scenarioNumber == 1)
            {
                transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            }
            else if (scenarioNumber == 2)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Pressed()
    {
        Global.scenarioNum = scenarioNumber;
        SceneManager.LoadScene("Game");
    }
}
