using UnityEngine;

public class FireObject : MonoBehaviour
{
    public float playerDetectionDistance = 10.0f;

    void Awake()
    {
        GameManager.instance.fireObject = gameObject;
        GameManager.instance.fireObjectScript = this;
    }
}
