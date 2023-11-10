//using Oculus.Interaction.Surfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class PlayerPosSearch : MonoBehaviour
{
    public static PlayerPosSearch instance;
    //private Transform _playerPos;
    [SerializeField] Transform destination;
    private Vector3 lastValidPosition;
    public NavMeshData navMeshData;

    //public NavMeshSurface surface;


    /*private void Start()
    {
        InvokeRepeating("Rebake", 3.0f, 3.0f);
    }*/

    /*public void BakeNavMesh()
    {
        
        NavMeshDataInstance navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);
        //NavMeshBuilder.BuildNavMeshAsync(surface.navMeshData);
    }*/

   /* public void Rebake()
    {
        surface.BuildNavMesh();
    }*/

    private void Awake()
    {
        instance = this;

        //InvokeRepeating("UpdateMesh", 2.0f, 2.0f);
    }

    

    // Update is called once per frame
    void Update()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination.transform.position, out hit, 100f, NavMesh.AllAreas))
        {
            //_playerPos.transform.position = hit.position;
            //destination.transform.position = hit.position;
            lastValidPosition = hit.position;
            //_agent.SetDestination(hit.position);
            
        }
        
    }

    private void UpdateMesh()
    {
        NavMesh.AddNavMeshData(navMeshData);
    }

    public Vector3 GetPlayerPos()
    {
        if (lastValidPosition != Vector3.zero)
        {
            return lastValidPosition;
        }
        return Vector3.zero;
    }
}
