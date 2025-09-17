using UnityEngine;

public class FireManagementUI : MonoBehaviour
{
    [SerializeField] private PopupUIAnimation popupAnimation;

    void Update()
    {
        if (GameManager.instance.fireObjects.Count == 0) return;

        float detectionRange = GameManager.instance.fireObjectScripts[0].playerDetectionDistance;

        GameObject closestFireObject;
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

        if (closestDistance <= detectionRange && !popupAnimation.open) popupAnimation.OpenUI();
        else if (closestDistance > detectionRange && popupAnimation.open) popupAnimation.CloseUI();
    }
}
