using TMPro;
using UnityEngine;

public class AvaliableFiresText : MonoBehaviour
{
    [SerializeField] private GameObject coolburnButton;

    private TMP_Text text;

    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.text = "x" + (2 - FireManager.instance.CurrentNumberOfFires);

        if (FireManager.instance.CurrentNumberOfFires < 2)
        {
            coolburnButton.SetActive(true);
        }
    }
}
