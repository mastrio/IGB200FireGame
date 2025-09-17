using UnityEngine;

public class CoolburnButton : MonoBehaviour
{
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
        dragging = true;
        resetAnimation = null;
    }

    public void PointerUp()
    {
        dragging = false;
        resetAnimation = new SpringDamperVector3(30.0f, 30.0f, startPos);
    }
}
