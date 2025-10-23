using UnityEngine;

public class ConfineMouse : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // Keep cursor movement inside the game window
    }

    void OnApplicationFocus(bool hasFocus) // Re-apply if focus lost
    {
        if (hasFocus)
            Cursor.lockState = CursorLockMode.Confined;
    }
}