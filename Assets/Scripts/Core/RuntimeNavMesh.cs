using Unity.AI.Navigation;
using UnityEngine;

public class RuntimeNavMesh : MonoBehaviour
{
    public static RuntimeNavMesh instance;

    public static bool doRebuildNavmesh = true;

    private NavMeshSurface navSurface;
    private int timer;

    void Awake()
    {
        instance = this;
        navSurface = gameObject.GetComponent<NavMeshSurface>();
        navSurface.BuildNavMesh();
    }

    void FixedUpdate()
    {
        timer--;
        if (timer <= 0)
        {
            if (doRebuildNavmesh)
            {
                RebuildRecreateRecoverFourthWord();
                doRebuildNavmesh = false;
                timer = 30;
            }
        }
    }

    // im so good at naming methods
    public void RebuildRecreateRecoverFourthWord()
    {
        navSurface.UpdateNavMesh(navSurface.navMeshData);
    }
}
