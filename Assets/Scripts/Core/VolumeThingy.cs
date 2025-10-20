using UnityEngine;
using UnityEngine.Rendering;

public class VolumeThingy : MonoBehaviour
{
    void Start()
    {
        FireManager.instance.globalVolume = gameObject.GetComponent<Volume>();
        FireManager.instance.SetVolume();
    }
}
