using UnityEngine;

public class MapPanTutorialButton : MonoBehaviour
{
    void Update()
    {
        if (GameManager.instance.hasPannedMap) gameObject.SetActive(false);
    }
}
