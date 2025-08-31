using UnityEngine;

public class HasMapObject : MonoBehaviour
{
    [SerializeField] private GameObject mapObjectPrefab;

    void Start()
    {
        GameManager.instance.AddMapObjectToMap(mapObjectPrefab, gameObject);
    }
}
