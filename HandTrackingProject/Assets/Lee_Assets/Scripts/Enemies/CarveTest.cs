using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarveTest : MonoBehaviour
{
    public float carveRadius = 3f; // The radius of the carved area

    private void Start()
    {
        // Carve the NavMesh around this agent at the start
        CarveNavMesh();
    }

    private void CarveNavMesh()
    {
        // Carve the NavMesh to create a cutout around this agent
        //NavMeshCarve.Instance.Carve(transform.position, carveRadius);
    }
}
