using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerObject;

    [HideInInspector] public List<GameObject> fireObjects;
    [HideInInspector] public List<FireObject> fireObjectScripts;

    [HideInInspector] public bool playerDraggingFireButton = false;

    [HideInInspector] public float mapZoomLevel = 1.0f;
    [HideInInspector] public Vector3 mapCameraOffset = Vector3.zero;

    // Tutorial checks
    [HideInInspector] public bool hasMoved = false;
    [HideInInspector] public bool hasPlacedFire = false;
    [HideInInspector] public bool hasPannedMap = false;
    [HideInInspector] public bool hasManagedFire = false;

    [SerializeField] private GameObject winScreenObject;
    [SerializeField] private GameObject mapObjectContainer;
    [SerializeField] private GameObject[] levelList;

    void Awake()
    {
        // Assign self to static instance.
        if (instance == null) instance = this;
        else Destroy(gameObject);

        LoadLevel();
    }

    void Update()
    {
        if (FireManager.instance.FireDangerLevel < 1)
        {
            winScreenObject.SetActive(true);
        }
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

        //Error with Game manager null ref when trying

        if (FireManager.instance != null)
        {
            FireManager.instance.NewSceneCamera();
            FireManager.instance.GetPlayer();
            FireManager.instance.ResetNumOfFires();
            FireManager.instance.SetFireDangerLevel();
        }
    }

    public void AddMapObjectToMap(GameObject mapObjectPrefab, GameObject sourceObject)
    {
        GameObject mapObject = Instantiate(mapObjectPrefab, mapObjectContainer.transform);
        mapObject.GetComponent<MapObject>().linkedObject = sourceObject;
    }
}
