using UnityEngine;

public class MapFireIcon : MonoBehaviour
{
    private static readonly int UPDATE_COUNTDOWN_MIN = 30;
    private static readonly int UPDATE_COUNTDOWN_MAX = 50;

    private int countdown = 0;

    void FixedUpdate()
    {
        countdown--;
        if (countdown <= 0)
        {
            countdown = Random.Range(
                UPDATE_COUNTDOWN_MIN,
                UPDATE_COUNTDOWN_MAX
            );

            transform.localPosition = new Vector3(
                Random.Range(-0.1f, 0.1f),
                Random.Range(-0.1f, 0.1f),
                0.0f
            );
        }
    }
}
