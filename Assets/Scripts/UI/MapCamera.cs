using UnityEngine;

public class MapCamera : MonoBehaviour
{
    static readonly Vector2 OBJECT_SCALE = new Vector2(0.1f, 0.1f);

    Vector3 basePos;

    void Start()
    {
        basePos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(
            GameManager.instance.playerObject.transform.position.x * OBJECT_SCALE.x,
            GameManager.instance.playerObject.transform.position.z * OBJECT_SCALE.y
        ) + basePos;
    }
}
