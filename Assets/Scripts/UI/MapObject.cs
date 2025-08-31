using System;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [NonSerialized] public GameObject linkedObject;

    void Update()
    {
        if (linkedObject == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.localPosition = new Vector3(
            linkedObject.transform.position.x,
            linkedObject.transform.position.z,
            0.0f
        );
    }
}
