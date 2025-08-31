using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject mapObjectContainer;

    void Awake()
    {
        // Assign self to static instance.
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddMapObjectToMap(GameObject mapObjectPrefab, GameObject sourceObject)
    {
        GameObject mapObject = Instantiate(mapObjectPrefab, mapObjectContainer.transform);
        mapObject.GetComponent<MapObject>().linkedObject = sourceObject;
    }
}
