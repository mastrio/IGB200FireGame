using UnityEngine;

public class FireManagementUI : MonoBehaviour
{
    [SerializeField] private PopupUIAnimation popupAnimation;
    [SerializeField] private GameObject backgroundCloseButton;
    [SerializeField] private FireManagementBar fireManagementBar;
    [SerializeField] private TutorialDragThingy tutorialThing;

    void Update()
    {
        if (popupAnimation.open && Time.timeScale == 0.0f) fireManagementBar.gameObject.SetActive(false);
        if (popupAnimation.open && Time.timeScale == 1.0f) fireManagementBar.gameObject.SetActive(true);

        if (GameManager.instance.fireObjects.Count == 0)
        {
            if (popupAnimation.open)
            {
                popupAnimation.CloseUI();
                backgroundCloseButton.SetActive(false);
            }
            return;
        }

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

        if (closestDistance <= detectionRange)
        {
            fireManagementBar.fireObject = closestFireObject.GetComponent<FireObject>();
        }

        if (closestDistance <= detectionRange && !popupAnimation.open)
        {
            popupAnimation.OpenUI();
            TutorialManager.instance.DisplayTutorial("FireManagementBar");
            if (tutorialThing != null) tutorialThing.StartTutorial();
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
