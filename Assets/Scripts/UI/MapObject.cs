using System;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    static readonly Vector2 OBJECT_SCALE = new Vector2(0.1f, 0.1f);

    [SerializeField] private float layerOffset;

    [NonSerialized] public GameObject linkedObject;

    private float targetAlpha = 1.0f;

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
            linkedObject.transform.position.z * 0.002f + layerOffset
        );

        //transform.localScale = linkedObject.transform.localScale / 50.0f;
    }
}
