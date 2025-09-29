using UnityEngine;

public class ManageFireTutorialButton : MonoBehaviour
{
    void Update()
    {
        if (GameManager.instance.hasManagedFire) gameObject.SetActive(false);
    }
}
