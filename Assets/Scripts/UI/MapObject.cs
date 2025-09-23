using System;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    static readonly Vector2 OBJECT_SCALE = new Vector2(0.1f, 0.1f);

    [SerializeField] private float layerOffset;
    [SerializeField] private bool scaleWithZoom = false;

    [NonSerialized] public GameObject linkedObject;

    void Update()
    {
        if (linkedObject == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.localPosition = new Vector3(
            linkedObject.transform.position.x * OBJECT_SCALE.x * GameManager.instance.mapZoomLevel,
            linkedObject.transform.position.z * OBJECT_SCALE.y * GameManager.instance.mapZoomLevel,
            linkedObject.transform.position.z * 0.002f + layerOffset
        );

        if (scaleWithZoom)
        {
            transform.localScale = new Vector3(
                GameManager.instance.mapZoomLevel,
                GameManager.instance.mapZoomLevel,
                1.0f
            );
        }
    }
}
