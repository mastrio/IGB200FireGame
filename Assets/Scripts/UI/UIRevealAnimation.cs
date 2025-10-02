using UnityEngine;

public class UIRevealAnimation : MonoBehaviour
{
    [SerializeField] private float startDelay = 0.0f;
    [SerializeField] private float spring = 50.0f;
    [SerializeField] private float damp = 25.0f;

    private SpringDamperVector3 scaleAnimation;

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
        scaleAnimation = new SpringDamperVector3(spring, damp, Vector3.one, startDelay);
    }

    public void OnHover()
    {
        scaleAnimation.targetVal = new Vector3(1.2f, 1.2f, 1.0f);
    }

    public void OnReverseHover()
    {
        scaleAnimation.targetVal = Vector3.one;
    }
}
