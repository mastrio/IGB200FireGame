using UnityEngine;

public class UITriggerCoolRelay : MonoBehaviour
{
    [SerializeField] private CoolburnGroundItem parentScriptCoolObject;

    private void OnTriggerEnter(Collider other)
    {
        parentScriptCoolObject?.OnTriggerEnterRelayFromChild(other);
    }

    private void OnTriggerExit(Collider other)
    {
        parentScriptCoolObject?.OnTriggerExitRelayFromChild(other);
    }
}

