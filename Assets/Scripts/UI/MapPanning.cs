using UnityEngine;

public class MapPanning : MonoBehaviour
{
    private bool panning = false;
    private Vector3 startDragCamPos;
    private Vector3 startDragMousePos;

    void Update()
    {
        if (panning)
        {
            GameManager.instance.mapCameraOffset = -(Input.mousePosition - startDragMousePos) / GameManager.instance.mapZoomLevel * 0.25f + startDragCamPos; // Long ass line lol
            if ((Input.mousePosition - startDragMousePos).magnitude > 0.01f) GameManager.instance.hasPannedMap = true;
        }
    }

    public void StartDrag()
    {
        startDragCamPos = GameManager.instance.mapCameraOffset;
        startDragMousePos = Input.mousePosition;
        panning = true;
    }

    public void StopDrag()
    {
        panning = false;
    }
}
