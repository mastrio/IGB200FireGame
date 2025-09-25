using UnityEngine;

public class FireManagementUI : MonoBehaviour
{
    [SerializeField] private PopupUIAnimation popupAnimation;
    [SerializeField] private FireManagementBar fireManagementBar;

    void Update()
    {
        if (GameManager.instance.fireObjects.Count == 0) return;

        float detectionRange = GameManager.instance.fireObjectScripts[0].playerDetectionDistance;

        GameObject closestFireObject = gameObject;
        float closestDistance = 999999999.0f;

        foreach (GameObject fireObject in GameManager.instance.fireObjects)
        {
            Vector3 playerPos = GameManager.instance.playerObject.transform.position;
            Vector3 firePos = fireObject.transform.position;
            float distance = (playerPos - firePos).magnitude;
            if (distance <= closestDistance)
            {
                closestFireObject = fireObject;
                closestDistance = distance;
            }
        }

        if (closestDistance <= detectionRange && !popupAnimation.open)
        {
            fireManagementBar.fireObject = closestFireObject.GetComponent<FireObject>();
            popupAnimation.OpenUI();
        }
        else if (closestDistance > detectionRange && popupAnimation.open)
        {
            fireManagementBar.State = FireBarState.Info;
            popupAnimation.CloseUI();
        }
    }

    public void BackgroundButtonPressed()
    {
        fireManagementBar.State = FireBarState.Info;
    }
}
