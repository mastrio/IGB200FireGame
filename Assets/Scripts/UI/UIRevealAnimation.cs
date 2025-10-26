using UnityEngine;

public class UIRevealAnimation : MonoBehaviour
{
    public float startDelay = 0.0f;
    public float spring = 50.0f;
    public float damp = 25.0f;
    [SerializeField] private bool runOnUpdate = true;

    [HideInInspector] public SpringDamperVector3 scaleAnimation;

    void OnDisable()
    {
        transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
    }

    void OnEnable()
    {
        transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
        scaleAnimation = new SpringDamperVector3(spring, damp, Vector3.one, startDelay);
    }

    void Update()
    {
        if (!runOnUpdate) return;
        transform.localScale = scaleAnimation.Update(transform.localScale);
    }

    void FixedUpdate()
    {
        if (runOnUpdate) return;
        transform.localScale = scaleAnimation.Update(transform.localScale);
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
