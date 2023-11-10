using Oculus.Platform.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class ChasingState : BaseState
{
    
    private NavMeshObstacle _obstacle;

    //public Transform playerTransform;
    public float rotationSpeed;
    //private NavMeshAgent _agent;
    public Transform destination;
    private Transform tempDestination;

    private float _tempStopDistance = 0.0f;
    

    //public GameObject _raycastPoint;
    public float speed = 1.0f;
    //private FieldOfView _fieldOfView;
    private bool _canSeePlayer = false;

    private NavMeshPath _path;

   

    private Vector3 playerPos;
    //private Vector3 closestPointOnNavMesh;
    
    public bool isBackTracking = false;
    private Vector3 _animDestination;

    Vector3 playerDirection;
    float distToPlayer;
    //public GameObject _cube;
    //private Vector3 backtrackingStartPosition;
    //Vector3 newPosition;
    [SerializeField] float _runningDistance = 4f;

    //private AnimationState _animation;

    private bool _carve = false;
    private float distanceWeight = 1.0f;    // You can adjust these weights based on your design
    private float proximityWeight = 1.0f;
    private bool _isStationary = false;
    private bool _chasePlayer;
    Vector3 newDestination;

    protected override void Awake()
    {
        //_animation = GetComponent<AnimationState>();
        //_agent = GetComponent<NavMeshAgent>();
        _runningDistance *= _agent.stoppingDistance;
        
        _obstacle = GetComponent<NavMeshObstacle>();
       // tempDestination = destination;
        _path = new NavMeshPath();
        //_fieldOfView = GetComponent<FieldOfView>();
    }

    protected override void OnEnable()
    {
        
        _tempStopDistance = _agent.stoppingDistance;

    }

    public override void EnterState()
    {
        StartCoroutine(SetDest());

        /*_agent.SetDestination(playerPos);
        UpdatePlayerPos();*/
    }


    IEnumerator SetDest()
    {
        yield return new WaitForEndOfFrame();

        _chasePlayer = true;
        _agent.SetDestination(playerPos);
    }


    public override void UpdateState()
    {
        playerDirection = playerPos - transform.position;
        distToPlayer = playerDirection.magnitude;

       /* Debug.LogError("Chasing is: " + _chasePlayer);
        Debug.LogError("Stationary is: " + _isStationary);
        Debug.LogError("Backtracking is: " + isBackTracking);*/


        if (isBackTracking)
        {

            Backtrack();
            //_animation.BackTracking();

        }
        else if (_chasePlayer)
        {
            ChasePlayer();

        }
        else if (_isStationary)
        {
            Stationary();
        }

        /*if(distToPlayer <= _tempStopDistance && !_isStationary)
        {
            _animation.StoppingDistance();
            _chasePlayer = false;
            _isStationary = true;
            //Stationary();
        }*/

        /*if (distToPlayer <= (_tempStopDistance - 1.0f))
        {
            if (_isStationary)
            {
                ExitStationary();
            }

            StartBacktracking();
        }*/

        //ShouldDisableAgent();

        /* if (_carve && _obstacle.carving)
         {
             _obstacle.carving = false;
         }
         else if (!_carve && !_obstacle.carving)
         {
             _obstacle.carving = true;
         }*/




        Debug.LogWarning("Backtracking value: " + isBackTracking);
        //Debug.LogError("Stopping distance: " + _tempStopDistance);
        UpdateFieldOfView();
        //closestPointOnNavMesh = GetClosestPointOnNavMesh(transform.position);

       
    }

    private void Stationary()
    {
        _animation.StoppingDistance();

        if (_agent.enabled)
        {
            _agent.enabled = false;
        }

        if (!_obstacle.carving)
        {
            _obstacle.carving = true;
        }


        bool _checkValidPath = CheckPathValidity(transform.position, playerPos);
        if (distToPlayer > _tempStopDistance)
        {
            if (_checkValidPath)
            {

                ExitStationary();
                _chasePlayer = true;
                //StartCoroutine(SetDestination(playerPos, null, 5.0f));
            }
        }

        if (distToPlayer <= (_tempStopDistance - 1.0f))
        {

            //ExitStationary();
            StartCoroutine(EnterBAcktracking());

            //StartBacktracking();
        }
    }

    IEnumerator EnterBAcktracking()
    {
        ExitStationary();
        //_obstacle.carving = false;
        //_agent.enabled = true;
        _agent.destination = Vector3.zero;
        
        yield return new WaitForSeconds(0.2f);
        StartBacktracking();
        //_isStationary = false;
    }

    private void ExitStationary()
    {
        _obstacle.carving = false;
        _agent.enabled = true;

        _isStationary = false;
    }

    private void LateUpdate()
    {

        UpdatePlayerPos();


        



        //RotateTowardsPlayer();

    }

    private void UpdateFieldOfView()
    {
        _canSeePlayer = _fieldOfView.PlayerInView();
    }

    private void UpdatePlayerPos()
    {
        playerPos = PlayerPosSearch.instance.GetPlayerPos();
    }

    private void StartBacktracking()
    {
        /* if (_obstacle.carving)
         {
             _obstacle.carving = false;
         }*/

        //_chasePlayer = false;

        //isBackTracking = true;
        newDestination = SampleBacktrackPoints();
        
        //_animDestination = newDestination;
        //_agent.stoppingDistance = 0.0f;
        _agent.stoppingDistance = 0.0f;
        isBackTracking = true;
        _agent.SetDestination(newDestination);
        
        //StartCoroutine(SetBacktracking());
        //SetAgentDestination(newDestination, null);
        //_animation.SetBackTracking(true);
        //_agent.SetDestination(newDestination);
    }

   /* IEnumerator SetBacktracking()
    {
        yield return new WaitForSeconds(0.25f);
        isBackTracking = true;
    }*/

    /*private void SetAgentDestination(Vector3 dest, Transform dest2)
    {
        if(_agent.enabled && _obstacle.carving)
        {
            _obstacle.carving = false;
        }else if(!_agent.enabled && !_obstacle.carving)
        {
            _obstacle.carving = true;
        }


        if(dest == Vector3.zero)
        {
            _agent.SetDestination(dest2.position);
        }
        else if(dest2 == null)
        {
            _agent.SetDestination(dest);
        }
    }*/

    IEnumerator SetDestination(Vector3 dest, Transform dest2, float stopDist)
    {
        //_agent.enabled = ShouldDisableAgent();



        yield return new WaitForSeconds(0.2f);

        _agent.stoppingDistance = stopDist;
        if (dest == Vector3.zero)
        {
            _agent.SetDestination(dest2.position);
        }
        else if (dest2 == null)
        {
            _agent.SetDestination(dest);
        }
    }

    private void Backtrack()
    {
        Vector3 newDirection = newDestination - transform.position;
        float distToNew = newDirection.magnitude;
        _animation.BackTracking();
        if (distToNew <= (_agent.stoppingDistance + 0.5f) || distToPlayer > _tempStopDistance)
        {
            // Backtracking completed
            isBackTracking = false;
            _chasePlayer = true;
            _agent.stoppingDistance = _tempStopDistance;
            //_agent.SetDestination(playerPos);
            //_animation.SetBackTracking(false);
        }
    }

    private Vector3 SampleBacktrackPoints()
    {
        Vector3 playerToAgent = transform.position - playerPos;
        float angleRange = 360.0f; // Adjust this angle based on your design
        int numSamples = 8; // Adjust the number of samples based on your needs
        float samplingRadius = 5.0f; // Adjust the radius based on your needs

        Vector3 bestPoint = Vector3.zero;
        float bestScore = float.MinValue;

        for (int i = 0; i < numSamples; i++)
        {
            float angle = (i / (float)(numSamples - 1)) * angleRange - angleRange / 2.0f;
            Vector3 rotatedDirection = Quaternion.Euler(0, angle, 0) * playerToAgent;
            Vector3 samplePosition = transform.position + rotatedDirection.normalized * samplingRadius;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(samplePosition, out hit, samplingRadius, NavMesh.AllAreas))
            {
                float score = CalculateScore(samplePosition);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestPoint = samplePosition;
                }
            }
        }

        return bestPoint;
    }

    private float CalculateScore(Vector3 position)
    {
        float distanceScore = Vector3.Distance(position, playerPos) * distanceWeight;
        float proximityScore = CalculateEdgeProximityScore(position) * proximityWeight;

        return distanceScore + proximityScore;
    }

    private float CalculateEdgeProximityScore(Vector3 point)
    {
        NavMeshHit hit;

        // Cast a ray from the point downwards to find the nearest point on the NavMesh
        if (NavMesh.SamplePosition(point, out hit, 1.0f, NavMesh.AllAreas))
        {
            // Calculate the distance from the point to the nearest point on the NavMesh
            float distanceToNavMeshEdge = Vector3.Distance(point, hit.position);
            float edgeProximityScore = 1.0f - Mathf.Clamp01(distanceToNavMeshEdge);
            return edgeProximityScore;
        }

        // Point is not on the NavMesh, return a low score
        return 0.0f;
    }

    
   

   

    private void ChasePlayer()
    {
        if (distToPlayer <= (_tempStopDistance - 1.0f))
        {
            _chasePlayer = false;
            StartBacktracking();
        }

        if (distToPlayer <= _tempStopDistance)
        {
            _animation.StoppingDistance();
            _chasePlayer = false;
            _isStationary = true;
            //Stationary();
        }
        else if (distToPlayer <= _runningDistance)
        {
            _animation.WalkingDistance();
        }
        else
        {
            _animation.RunningDistance();
        }
        /*Vector3 playerToAgent = transform.position - playerPos;
        float distanceToPlayer = playerToAgent.magnitude;*/

        /*if(distToPlayer <= _runningDistance)
        {
            _animation.WalkingDistance();
        }*/
       


        //if (_agent.destination == playerPos)
        //{
        bool _checkValidPath = CheckPathValidity(transform.position, playerPos);
        if (_checkValidPath && distToPlayer > _agent.stoppingDistance)
        {
            //_agent.stoppingDistance = 5;
            //destination = tempDestination;

            //ShouldDisableAgent();
            StartCoroutine(SetDestination(playerPos, null, 5.0f));
            //SetAgentDestination(playerPos, null);
            //_agent.SetDestination(playerPos);
        }
        else
        {
            //_agent.stoppingDistance = 0;
            //destination = transform;

            // if (_agent.enabled)
            //{
            //ShouldDisableAgent();
            _chasePlayer = false;
            _isStationary = true;
            StartCoroutine(SetDestination(transform.position, null, 0.0f));
            //SetAgentDestination(transform.position, null);
            //}
            //_agent.SetDestination(transform.position);
        }
        //}

       /* if (_agent.destination != playerPos)
        {
            bool _checkValidPaths = CheckPathValidity(transform.position, playerPos);
            if (_checkValidPaths)
            {
                //_agent.stoppingDistance = 5;
                destination = tempDestination;

                //ShouldDisableAgent();
                StartCoroutine(SetDestination(playerPos, null, 5.0f));
                //SetAgentDestination(playerPos, null);
            }*/
            //_agent.SetDestination(playerPos);
        //}

        /*if (distanceToPlayer <= (_tempStopDistance - 1.0f))
        {
            StartBacktracking();
        }*/


        
    }

    

    public void DisableAgent()
    {
        _agent.enabled = false;
    }

    public void EnableAgent()
    {
        _agent.enabled = true;
    }

    private Vector3 GetClosestPointOnNavMesh(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return position;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 playerDirection = playerPos - transform.position;
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 4f);
    }

    private bool CheckPathValidity(Vector3 _startPoint, Vector3 _endPoint)
    {
        //NavMeshPath path = new NavMeshPath();
        bool hasPath = NavMesh.CalculatePath(_startPoint, _endPoint, NavMesh.AllAreas, _path);

        NavMeshPathStatus pathStatus = _path.status;
        if (pathStatus != NavMeshPathStatus.PathComplete)
        {

            Debug.LogWarning("Path Status: " + pathStatus);
            return false;
        }

        return hasPath && _path.corners.Length > 0; ;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void LateUpdateState()
    {
        throw new NotImplementedException();
    }
}
