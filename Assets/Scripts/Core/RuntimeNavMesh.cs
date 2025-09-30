using Unity.AI.Navigation;
using UnityEngine;

public class RuntimeNavMesh : MonoBehaviour
{
    void Start()
    {
        NavMeshSurface navSurface = gameObject.GetComponent<NavMeshSurface>();
        navSurface.BuildNavMesh();
    }
}
