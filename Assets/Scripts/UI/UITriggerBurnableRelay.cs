using UnityEngine;

public class UITriggerBurnableRelay : MonoBehaviour
{
    [SerializeField] private BurnableObject parentScriptBurnableObject;

    private void OnTriggerEnter(Collider other)
    {
        parentScriptBurnableObject?.OnTriggerEnterRelayFromChild(other);
    }

    private void OnTriggerExit(Collider other)
    {
        parentScriptBurnableObject?.OnTriggerExitRelayFromChild(other);
    }
}
