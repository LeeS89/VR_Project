using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AgentNAvigation : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerTransform;
    public float rotationSpeed;
    public NavMeshAgent _agent;
    public Transform destination;
    [SerializeField] Animator _animator;


    //Test variables
    public LayerMask obstacleLayer;
    public float checkVisibilityInterval = 0.5f;
    public GameObject _raycastPoint;
    private bool isMovingToNewPosition = false;
    private Vector3 newPosition;
    private float _stoppingDistance = 0;
    public float _distanceToStop = 5;
    public float moveSpeed = 0.0f;

    public float maxDistFromPlayer = 5f;

    private bool _setPos = true;
    public float speed = 1.0f;
    private float _turnSpeed = 0.0f;
    private BulletSpawn _bulletSpawn;

    void Start()
    {
        _bulletSpawn = GetComponentInChildren<BulletSpawn>();
        _turnSpeed = _agent.angularSpeed;
        _stoppingDistance = _agent.stoppingDistance;
        //_agent.SetDestination(destination.position);
        //InvokeRepeating("CheckVisibility", 0f, checkVisibilityInterval);
        /*_agent.SetDestination(destination.position);*/
    }

    /*private void CheckVisibility()
    {
        Vector3 agentPosition = _raycastPoint.transform.position;
        Vector3 playerPosition = destination.position;

        RaycastHit hit;
        if(Physics.Raycast(agentPosition,playerPosition - agentPosition, out hit, Mathf.Infinity))
        {
            if(hit.collider.gameObject.layer == 9)
            {
                Debug.LogWarning("Enemy in the way!!!!!!!");
                newPosition = CalculateNewPosition(agentPosition, playerPosition);
                //_agent.Warp(newPosition);
                //_agent.SetDestination(destination.position);

                if(!isMovingToNewPosition)
                {
                    isMovingToNewPosition = true;
                    _agent.isStopped = true;
                    StartCoroutine(MoveToNewPosition());
                }
            }
        }else if (isMovingToNewPosition)
        {
            StopCoroutine(MoveToNewPosition());
            _agent.isStopped = false;
            _agent.stoppingDistance = _stoppingDistance;
            _agent.SetDestination(destination.position);
            isMovingToNewPosition = false;
        }
    }*/

    /*private IEnumerator MoveToNewPosition()
    {
        while(Vector3.Distance(_agent.transform.position, newPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(_agent.transform.position, newPosition, moveSpeed * Time.deltaTime);
            _agent.stoppingDistance = 0;
            //_agent.SetDestination(newPosition);
            yield return null;
        }

        _agent.isStopped = false;
        _agent.stoppingDistance = _stoppingDistance;
        _agent.SetDestination(destination.position);
        isMovingToNewPosition = false;
    }*/



    /*private Vector3 CalculateNewPosition(Vector3 agentPosition,Vector3 playerPosition)
    {
        Vector3 direction = (playerPosition - agentPosition).normalized;
        Vector3 newPosition = agentPosition + direction *20f;

        return newPosition;
    }*/

    // Update is called once per frame
    void Update()
    {

        Vector3 raycastOriginPosition = _raycastPoint.transform.position;
        Vector3 targetDirection = (destination.position + new Vector3(0, 0.5f, 0)) - raycastOriginPosition;
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(_raycastPoint.transform.forward, targetDirection, singleStep, 0.0f);
        _raycastPoint.transform.rotation = Quaternion.LookRotation(newDirection);

        Debug.DrawRay(raycastOriginPosition, newDirection * 5f, Color.red);

        //RaycastHit hit2;
        if (Physics.Raycast(raycastOriginPosition, newDirection, out RaycastHit hit2, Mathf.Infinity))
        {
            // Check if the player is not obstructed by other agents
            if (hit2.collider.gameObject.layer == 7)
            {
                Debug.LogError("Player is visible!");
                //_agent.angularSpeed= 0.0f;
                Vector3 playerDirection = playerTransform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(playerDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
                _bulletSpawn._isShooting = true;

            }
            else
            {
                _bulletSpawn._isShooting = false;
                _bulletSpawn._spawnTimer = 0.0f;
                //_agent.angularSpeed = _turnSpeed;
            }
        }

        //if (_agent.enabled)
        //{
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination.transform.position, out hit, 100f, NavMesh.AllAreas))
        {
            if (_setPos)
            {
                _agent.SetDestination(hit.position);
                //SetDestinationPos(hit.position);
                //_setPos= false;
            }

            /* Vector3 playerDirection = playerTransform.position - transform.position;
             Quaternion rotation = Quaternion.LookRotation(playerDirection);
             transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);*/
        }

        

        /*Vector3 playerDirection = playerTransform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);*/

        /*if(_agent.isStopped)
        {
            _agent.enabled= false;
        }*/
        //}
        /*if(!_agent.enabled)
        {

            Vector3 playerDirection = playerTransform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }*/

        //_agent.SetDestination(destination.position);

        /* float distToTarget = Vector3.Distance(transform.position, destination.position);
         if(distToTarget <= _agent.stoppingDistance)
         {
             _animator.SetFloat("y", 0);
         }*/
        //_agent.stoppingDistance = 5;
    }

    private void SetDestinationPos(Vector3 pos)
    {
        destination.position = pos;
    }

    private void LateUpdate()
    {
        /*Vector3 raycastOriginPosition = _raycastPoint.transform.position;
        Vector3 targetDirection = (destination.position + new Vector3(0, 0.5f, 0)) - raycastOriginPosition;
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(_raycastPoint.transform.forward, targetDirection, singleStep, 0.0f);
        _raycastPoint.transform.rotation = Quaternion.LookRotation(newDirection);

        Debug.DrawRay(raycastOriginPosition, newDirection * 5f, Color.red);

        //RaycastHit hit2;
        if (Physics.Raycast(raycastOriginPosition, newDirection, out RaycastHit hit2, Mathf.Infinity))
        {
            // Check if the player is not obstructed by other agents
            if (hit2.collider.gameObject.layer == 7)
            {
                Debug.LogError("Player is visible!");
                //_agent.angularSpeed= 0.0f;
                Vector3 playerDirection = playerTransform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(playerDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
                _bulletSpawn._isShooting = true;

            }
            else
            {
                _bulletSpawn._isShooting = false;
                //_agent.angularSpeed = _turnSpeed;
            }
        }*/

        /*if (_agent.enabled)
        {
            Vector3 playerDirection = playerTransform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }*/
    }
}
