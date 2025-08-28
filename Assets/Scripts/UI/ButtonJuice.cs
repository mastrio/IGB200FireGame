using UnityEngine;

// NOTE: Unused, for now.
public class ButtonJuice : MonoBehaviour
{
    [SerializeField] private float spring = 0.1f;
    [SerializeField] private float damp = 0.25f;
    private Vector3 velocity;
    private Vector3 targetScale = new Vector3(1.0f, 1.0f, 1.0f);

    void Update()
    {
        // Spring Dampener
        velocity += (targetScale - transform.localScale) * spring;
        velocity -= velocity * damp;
        transform.localScale += velocity;

        // Lerp targetScale to 1x1
        targetScale = Vector3.Lerp(targetScale, new Vector3(1.0f, 1.0f, 1.0f), 16.0f * Time.deltaTime);
    }

    public void Pressed()
    {
        targetScale = new Vector3(0.75f, 0.75f, 1.0f);
    }
}
