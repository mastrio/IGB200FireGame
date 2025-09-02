using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioButton : MonoBehaviour
{
    [SerializeField] private int scenarioNumber = 0;

    public void Pressed()
    {
        Global.scenarioNum = scenarioNumber;
        SceneManager.LoadScene("Game");
    }
}
