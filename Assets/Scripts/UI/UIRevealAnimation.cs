using UnityEngine;

public class UIRevealAnimation : MonoBehaviour
{
    [SerializeField] private float startDelay = 0.0f;
    [SerializeField] private float spring = 2f;
    [SerializeField] private float damp = 8f;

    private SpringDampenerVector3 scaleAnimation;
    private SpringDampenerVector3 rotationAnimation;

    void Update()
    {
        transform.localScale = scaleAnimation.Update(transform.localScale);
    }

    void OnDisable()
    {
        transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
    }

    void OnEnable()
    {
        transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);

        scaleAnimation = new SpringDampenerVector3(spring, damp, Vector3.one, startDelay);
    }
}
