using TMPro;
using UnityEngine;

public class MapZoomText : MonoBehaviour
{
    private TMP_Text text;
    private LerpAnimationFloat alphaAnim;
    private float previousZoomLevel = 1.0f;

    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
        text.alpha = 0.0f;
    }

    void Update()
    {
        float roundedZoomLevel = Mathf.Round(GameManager.instance.mapZoomLevel * 1000.0f) / 1000.0f;
        float evenMoreRoundedZoomLevel = Mathf.Round((GameManager.instance.mapZoomLevel - 0.4f) * 10.0f) / 10.0f;
        text.text = "x" + evenMoreRoundedZoomLevel;

        if (alphaAnim != null) text.alpha = alphaAnim.Update(text.alpha);

        if (previousZoomLevel == roundedZoomLevel) alphaAnim = new LerpAnimationFloat(0.0f, 5.0f);
        else alphaAnim = new LerpAnimationFloat(1.0f, 15.0f);
        previousZoomLevel = roundedZoomLevel;
    }
}
