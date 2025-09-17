using UnityEngine;

public class FireManagementUI : MonoBehaviour
{
    [SerializeField] private PopupUIAnimation popupAnimation;

    void Update()
    {
        if (GameManager.instance.fireObject == null) return;

        float detectionRange = GameManager.instance.fireObjectScript.playerDetectionDistance;

        Vector3 playerPos = GameManager.instance.playerObject.transform.position;
        Vector3 firePos = GameManager.instance.fireObject.transform.position;

        float distance = (playerPos - firePos).magnitude;

        Debug.Log(popupAnimation == null);

        if (distance <= detectionRange && !popupAnimation.open) popupAnimation.OpenUI();
        else if (distance > detectionRange && popupAnimation.open) popupAnimation.CloseUI();
    }
}
