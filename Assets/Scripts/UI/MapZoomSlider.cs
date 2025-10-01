using UnityEngine;
using UnityEngine.UI;

public class MapZoomSlider : MonoBehaviour
{
    private Slider slider;
    private LerpAnimationFloat zoomAnimation;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();

        zoomAnimation = new LerpAnimationFloat(slider.value, 8.0f);
    }

    void Update()
    {
        if (zoomAnimation == null) return;
        GameManager.instance.mapZoomLevel = zoomAnimation.Update(GameManager.instance.mapZoomLevel);
    }

    public void ZoomSliderValueChanged()
    {
        zoomAnimation = new LerpAnimationFloat(slider.value, 8.0f);
    }
}
