using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CubeNavTest : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerTransform;
    public float rotationSpeed;
    public NavMeshAgent _agent;
    public Transform destination;
    //[SerializeField] Animator _animator;

    void Start()
    {
        /*_agent.SetDestination(destination.position);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (_agent.enabled)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(destination.transform.position, out hit, 100f, NavMesh.AllAreas))
            {
                _agent.SetDestination(hit.position);

                Vector3 playerDirection = playerTransform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(playerDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            }
            /*Vector3 playerDirection = playerTransform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);*/

            /*if(_agent.isStopped)
            {
                _agent.enabled= false;
            }*/
        }
        if (!_agent.enabled)
        {

            Vector3 playerDirection = playerTransform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

        //_agent.SetDestination(destination.position);

        /* float distToTarget = Vector3.Distance(transform.position, destination.position);
         if(distToTarget <= _agent.stoppingDistance)
         {
             _animator.SetFloat("y", 0);
         }*/
        //_agent.stoppingDistance = 5;
    }

    private void LateUpdate()
    {
        /*if (_agent.enabled)
        {
            Vector3 playerDirection = playerTransform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }*/
    }
}
