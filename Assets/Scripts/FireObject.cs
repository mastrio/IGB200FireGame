using System.Linq;
using UnityEngine;

public class FireObject : MonoBehaviour
{
    public float playerDetectionDistance = 10.0f;

    void Awake()
    {
        GameManager.instance.fireObjects.Add(gameObject);
        GameManager.instance.fireObjectScripts.Add(this);
    }

    void OnDestroy()
    {
        GameManager.instance.fireObjects.Remove(gameObject);
        GameManager.instance.fireObjectScripts.Remove(this);
    }
}
