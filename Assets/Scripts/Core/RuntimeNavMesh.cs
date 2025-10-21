using Unity.AI.Navigation;
using UnityEngine;

public class RuntimeNavMesh : MonoBehaviour
{
    public static RuntimeNavMesh instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RebuildRecreateRecoverFourthWord();
    }

    // im so good at naming methods
    public void RebuildRecreateRecoverFourthWord()
    {
        NavMeshSurface navSurface = gameObject.GetComponent<NavMeshSurface>();
        navSurface.BuildNavMesh();
    }
}
