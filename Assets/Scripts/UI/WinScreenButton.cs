using UnityEngine;

public class WinScreenButton : MonoBehaviour
{
    private UIRevealAnimation revealAnim;
    private float timer;

    void OnEnable()
    {
        revealAnim = gameObject.GetComponent<UIRevealAnimation>();
        timer = Time.unscaledTime + revealAnim.startDelay;
    }

    void Update()
    {
        if (Time.unscaledTime <= timer)
        {
            revealAnim.scaleAnimation = new SpringDamperVector3(revealAnim.spring, revealAnim.damp, Vector3.one, 0.0f);
        }
    }
}
