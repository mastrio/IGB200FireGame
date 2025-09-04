using System;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    static readonly Vector2 OBJECT_SCALE = new Vector2(0.1f, 0.1f);

    [SerializeField] private float layerOffset;

    [NonSerialized] public GameObject linkedObject;

    void Update()
    {
        if (linkedObject == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.localPosition = new Vector3(
            linkedObject.transform.position.x * OBJECT_SCALE.x,
            linkedObject.transform.position.z * OBJECT_SCALE.y,
            linkedObject.transform.position.z * 0.01f + layerOffset
        );

        //transform.localScale = linkedObject.transform.localScale / 50.0f;
    }
}
