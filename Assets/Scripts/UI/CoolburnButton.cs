using UnityEngine;

public class CoolburnButton : MonoBehaviour
{
    [SerializeField] private GameUI gameUI;
    [SerializeField] private GameObject fireObjectPrefab;

    Vector3 startPos;
    bool dragging = false;

    SpringDamperVector3 resetAnimation;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        if (dragging)
        {
            // TODO: Make this perfectly frame independent
            transform.position = Vector3.Lerp(transform.position, Input.mousePosition, 50.0f * Time.deltaTime);
        }

        if (resetAnimation != null)
        {
            transform.localPosition = resetAnimation.Update(transform.localPosition);
        }
    }

    public void PointerDown()
    {
        GameManager.instance.playerDraggingFireButton = true;
        dragging = true;
        resetAnimation = null;
        Cursor.visible = false;
    }

    public void PointerUp()
    {
        bool worked = gameUI.coolburnStart.mouseActionCheck(fireObjectPrefab);
        if (worked)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            transform.localPosition = startPos;
        }
        else
        {
            resetAnimation = new SpringDamperVector3(30.0f, 30.0f, startPos);
        }

        GameManager.instance.playerDraggingFireButton = false;
        dragging = false;
        Cursor.visible = true;
    }
}
