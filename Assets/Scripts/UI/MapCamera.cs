using UnityEngine;

public class MapCamera : MonoBehaviour
{
    static readonly Vector2 OBJECT_SCALE = new Vector2(0.1f, 0.1f);

    Vector3 basePos;

    void Start()
    {
        basePos = transform.position;
    }

    void OnEnable()
    {
        if (GameManager.instance == null) return; // Stops an error message from showing up when the game starts

        GameManager.instance.mapCameraOffset = new Vector3(
            GameManager.instance.playerObject.transform.position.x,
            GameManager.instance.playerObject.transform.position.z
        );
    }

    void Update()
    {
        transform.position = new Vector3(
            GameManager.instance.mapCameraOffset.x * OBJECT_SCALE.x * GameManager.instance.mapZoomLevel,
            GameManager.instance.mapCameraOffset.y * OBJECT_SCALE.y * GameManager.instance.mapZoomLevel
        ) + basePos;
    }
}
