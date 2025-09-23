using UnityEngine;

public class StartFireTutorialButton : MonoBehaviour
{
    void Update()
    {
        if (GameManager.instance.hasPlacedFire) gameObject.SetActive(false);
    }
}
