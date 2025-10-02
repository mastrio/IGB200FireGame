using System;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    static readonly Vector2 OBJECT_SCALE = new Vector2(0.1f, 0.1f);
    static readonly float maxTimerVal = 15;

    [SerializeField] private float layerOffset;
    [SerializeField] private bool scaleWithZoom = false;

    [SerializeField] private float updateTimer = maxTimerVal;

    [NonSerialized] public GameObject linkedObject;

    private Vector3 linkedObjPos;

    void Update()
    {
        transform.localPosition = new Vector3(
            linkedObjPos.x * OBJECT_SCALE.x * GameManager.instance.mapZoomLevel,
            linkedObjPos.z * OBJECT_SCALE.y * GameManager.instance.mapZoomLevel,
            linkedObjPos.z * 0.002f + layerOffset
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

    void FixedUpdate()
    {
        // Everything before this will run every tick.
        // Everything after this will run every `maxTimerVal` ticks.
        updateTimer -= 1;
        if (updateTimer <= 0)
        {
            updateTimer = maxTimerVal;
        }
        else return;

        if (linkedObject == null)
        {
            Destroy(gameObject);
            return;
        }

        linkedObjPos = linkedObject.transform.position;
    }
}
