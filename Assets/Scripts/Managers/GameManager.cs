using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerObject;

    [DoNotSerialize] public List<GameObject> fireObjects;
    [DoNotSerialize] public List<FireObject> fireObjectScripts;

    [DoNotSerialize] public bool playerDraggingFireButton = false;

    [SerializeField] private GameObject mapObjectContainer;
    [SerializeField] private GameObject[] levelList;

    void Awake()
    {
        // Assign self to static instance.
        if (instance == null) instance = this;
        else Destroy(gameObject);

        LoadLevel();
    }

    // Tries to load a level prefab from the `levelList`.
    // `Global.scenarioNum` is set by choosing a scenario from the main menu
    private void LoadLevel()
    {
        if (levelList.Length < Global.scenarioNum)
        {
            Debug.Log("Failed to load level \"" + Global.scenarioNum + "\"");
            return;
        }

        Instantiate(levelList[Global.scenarioNum - 1]);
    }

    public void AddMapObjectToMap(GameObject mapObjectPrefab, GameObject sourceObject)
    {
        GameObject mapObject = Instantiate(mapObjectPrefab, mapObjectContainer.transform);
        mapObject.GetComponent<MapObject>().linkedObject = sourceObject;
    }
}
