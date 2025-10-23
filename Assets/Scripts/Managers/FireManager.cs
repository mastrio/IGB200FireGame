using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class FireManager : MonoBehaviour
{
    [SerializeField] private InputAction MouseClick;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject FireParticlePrefab;

    public static FireManager instance;

    private Camera mainCamera;

    public Coroutine buttonCoroutine;
    private int coolburnLayer;
    private int groundLayer;

    [HideInInspector] public int FireDangerLevel;

    [HideInInspector] public int CurrentNumberOfFires = 0;

    [HideInInspector] public GameObject Fire1RefGameObject;
    [HideInInspector] public GameObject Fire2RefGameObject;

    [Header("Vignette Edge Effect")] // Vignette values and references
    [HideInInspector] public Volume globalVolume;
    private Vignette vignette;
    public float vignetteMax = 0.5f;
    public float vignetteFadeSpeed = 1.0f;
    private float vignetteTargetIntensity = 0f;

    private void Awake()
    {
        mainCamera = Camera.main;
        GetPlayer();
        SetFireDangerLevel();
        coolburnLayer = LayerMask.NameToLayer("Coolburn");
        groundLayer = LayerMask.NameToLayer("Ground");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (vignette != null) // Vignette fade every frame
        {
            vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, vignetteTargetIntensity, Time.deltaTime * vignetteFadeSpeed);
        }
    }

    public void SetVolume()
    {
        if (globalVolume != null) // Get vignette ref if volume is assigned & start with 0 intensity
        {
            if (globalVolume.profile.TryGet(out Vignette v))
            {
                vignette = v;
                vignette.intensity.value = 0f;
            }
        }
    }

    public void GetPlayer()
    {
        GameObject FindPlayer = GameObject.FindWithTag("Player");

        if (FindPlayer != null)
        {
            player = FindPlayer;
        }
        else
        {
            Debug.Log("No Player in scene");
        }
    }

    public void NewSceneCamera()
    {


        if (!mainCamera == Camera.main)
        {
            mainCamera = Camera.main; ;
        }
        else
        {
            Debug.Log("No New Camera in scene");
        }
    }

    private void OnEnable()
    {
        MouseClick.Enable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        MouseClick.Disable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) // sets Vignette to 0 on scene load
    {
        if (scene.name == "MainMenu" || scene.name == "Game")
        {
            SetVignetteTarget(0f);
        }
    }

    public void CoolButtonTrigger()
    {

        ClickToMove.movedisabled = true;
    }

    public bool mouseActionCheck(GameObject fireObjectPrefab) //InputAction.CallbackContext context
    {
        bool success = false;
        if (CurrentNumberOfFires >= 2)
        {
            ClickToMove.movedisabled = false;
            return success;
        }


        player.TryGetComponent<ClickToMove>(out ClickToMove clickToMove);
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        Physics.Raycast(ray: ray, hitInfo: out RaycastHit rayHit);
        int hitLayer = rayHit.collider.gameObject.layer;

        if (hitLayer == groundLayer || hitLayer == coolburnLayer)
        {
            ;
            Vector3 rayHitPos = rayHit.point;
            Vector3 spawnPos = new Vector3(rayHitPos.x, 0f, rayHitPos.z);
            GameObject FireObject = Instantiate(fireObjectPrefab, spawnPos, Quaternion.Euler(Vector3.zero));
            IncreaseNumberOfFires(FireObject);
            success = true;
        }
        ClickToMove.movedisabled = false;
        return success;
    }
    public void UpdateFireDangerLevel()
    {
        if (FireDangerLevel > 0)
        {
            FireDangerLevel--;
        }

    }
    public int GetFireDangerLevel()
    {
        return FireDangerLevel;
    }

    public void SetFireDangerLevel()
    {
        FireDangerLevel = 90;
    }

    public void IncreaseNumberOfFires(GameObject fireRef)
    {

        //Makes sure the assigment is correct
        if (Fire1RefGameObject == null)
        {
            Fire1RefGameObject = fireRef;
        }
        else if (Fire2RefGameObject == null)
        {
            Fire2RefGameObject = fireRef;
        }

        CurrentNumberOfFires += 1;

    }

    public void ReduceNumberOfFires()
    {
        CurrentNumberOfFires -= 1;
    }

    public int GetNumberOfFires(GameObject fireRef)
    {
        return CurrentNumberOfFires;
    }

    public void ResetNumOfFires()
    {
        CurrentNumberOfFires = 0;
    }

    //Stops it from playing diffrent times if two are present at once

    public void UpdateEmberParticles()
    {
        if (CurrentNumberOfFires == 0)
        {
            if (ScoreManager.instance.EmberParticles.gameObject.activeInHierarchy) ScoreManager.instance.EmberParticles.gameObject.SetActive(false);
            SetVignetteTarget(0f);
            return;
        }

        float fireObject1Intensity = 0f;
        float fireObject2Intensity = 0f;

        if (Fire1RefGameObject != null)
        {
            fireObject1Intensity = Fire1RefGameObject.GetComponent<FireObject>().fireIntensity;
        }

        if (Fire2RefGameObject != null)
        {
            fireObject2Intensity = Fire2RefGameObject.GetComponent<FireObject>().fireIntensity;
        }

        if (fireObject1Intensity > 130f || fireObject2Intensity > 130f)
        {
            if (!ScoreManager.instance.EmberParticles.gameObject.activeInHierarchy) ScoreManager.instance.EmberParticles.gameObject.SetActive(true);
            SetVignetteTarget(vignetteMax); // Fade vignette in
        }
        else
        {
            if (ScoreManager.instance.EmberParticles.gameObject.activeInHierarchy) ScoreManager.instance.EmberParticles.gameObject.SetActive(false);
            SetVignetteTarget(0f); // Fade vignette out
        }
    }
    private void SetVignetteTarget(float intensity)
    {
        vignetteTargetIntensity = intensity;
    }
}