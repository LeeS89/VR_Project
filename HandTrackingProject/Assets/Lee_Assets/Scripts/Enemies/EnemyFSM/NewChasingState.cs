using Oculus.Platform.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NewChasingState : BaseState
{

    //private NavMeshObstacle _obstacle;

    //public Transform playerTransform;
    public float rotationSpeed;
    //private NavMeshAgent _agent;
    public Transform destination;
    //private Transform tempDestination;

    private float _tempStopDistance = 5.0f;


    //public GameObject _raycastPoint;
    public float speed = 1.0f;
    //private FieldOfView _fieldOfView;
    //private bool _canSeePlayer = false;

    private NavMeshPath _path;

    private Coroutine _coroutine;

    private Vector3 playerPos;
    //private Vector3 closestPointOnNavMesh;

    //public bool isBackTracking = false;
    //private Vector3 _animDestination;

    Vector3 playerDirection;
    float distToPlayer;
    //public GameObject _cube;
    //private Vector3 backtrackingStartPosition;
    //Vector3 newPosition;
    [SerializeField] float _runningDistance = 4f;

    //private AnimationState _animation;

    //private bool _carve = false;
    //private float distanceWeight = 1.0f;    // You can adjust these weights based on your design
    //private float proximityWeight = 1.0f;
    //private bool _isStationary = false;
    private bool _chasePlayer = false;
    Vector3 newDestination;
    NavMeshObstacle _obstacle;
    public Vector3 _startPos;
    public Quaternion _startRot;

    private bool _playerDestSet = false;

    protected override void Awake()
    {
        base.Awake();
        _startPos = transform.position;
        _startRot = transform.rotation;
        //_animation = GetComponent<AnimationState>();
        //_agent = GetComponent<NavMeshAgent>();
        _runningDistance *= _agent.stoppingDistance;
        _obstacle= GetComponent<NavMeshObstacle>();
        //_obstacle = GetComponent<NavMeshObstacle>();
        // tempDestination = destination;
        _path = new NavMeshPath();
        //_fieldOfView = GetComponent<FieldOfView>();
        //StartCoroutine(SetDest());
    }

    public void ResetPos()
    {
        transform.position = _startPos;
        transform.rotation = _startRot;
    }

    protected override void OnEnable()
    {

        //_tempStopDistance = _agent.stoppingDistance;

    }

    public override void EnterState()
    {
        if (!_agent.enabled)
        {
            _agent.enabled = true;
        }

        if (_playerDestSet)
        {
            _coroutine = StartCoroutine(SetDest());
        }
        else
        {
            _agent.SetDestination(playerPos);
            _agent.stoppingDistance = 5.0f;
            _chasePlayer = true;
        }


        
        //_agent.stoppingDistance = 5.0f;
        _tempStopDistance = 5.0f;
        //_agent.SetDestination(playerPos);
        //_chasePlayer = true;
        /*_agent.SetDestination(playerPos);
        UpdatePlayerPos();*/
    }


    IEnumerator SetDest()
    {
        yield return new WaitForEndOfFrame();

       _chasePlayer = true;
        _agent.SetDestination(playerPos);
        _chasePlayer = true;
        _playerDestSet = false;
        _coroutine = null;
    }


    public override void UpdateState()
    {
        playerDirection = playerPos - transform.position;
        //playerDirection.y *= 0.5f;
        distToPlayer = playerDirection.magnitude;

        //distToPlayer = GetDistanceToTarget(playerPos);
        if(_chasePlayer)
        {
            
            ChasePlayer();

            /*if(!_obstacle.enabled)
            {
                _obstacle.enabled = true;
            }*/
        }

        
        base.UpdateState();




        //Debug.LogWarning("Backtracking value: " + isBackTracking);
        //Debug.LogError("Stopping distance: " + _tempStopDistance);
        //UpdateFieldOfView();
        //closestPointOnNavMesh = GetClosestPointOnNavMesh(transform.position);

        if(_canSeePlayer)
        {
            RotateTowardsPlayer();
        }


    }

    private float GetPathLength(NavMeshPath path)
    {
        float length = 0.0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return length;
    }

    public float GetDistanceToTarget(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (_agent.CalculatePath(targetPosition, path))
        {
            return GetPathLength(path);
        }
        return 0.0f;
    }

    /*private void Stationary()
    {
        _animation.StoppingDistance();

        if (_agent.enabled)
        {
            _agent.enabled = false;
        }

        *//*if (!_obstacle.carving)
        {
            _obstacle.carving = true;
        }*//*


        bool _checkValidPath = CheckPathValidity(transform.position, playerPos);
        if (distToPlayer > _tempStopDistance)
        {
            if (_checkValidPath)
            {

                //ExitStationary();
                //_chasePlayer = true;
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
        //ExitStationary();
        //_obstacle.carving = false;
        //_agent.enabled = true;
        _agent.destination = Vector3.zero;

        yield return new WaitForSeconds(0.2f);
        StartBacktracking();
        //_isStationary = false;
    }*/

    /* private void ExitStationary()
     {
         _obstacle.carving = false;
         _agent.enabled = true;

         _isStationary = false;
     }*/

    /* private void LateUpdate()
     {

         UpdatePlayerPos();






         //RotateTowardsPlayer();

     }*/

    public override void LateUpdateState()
    {
        UpdatePlayerPos();
    }


    /*private void UpdateFieldOfView()
    {
        _canSeePlayer = _fieldOfView.PlayerInView();
    }*/

    private void UpdatePlayerPos()
    {
        playerPos = PlayerPosSearch.instance.GetPlayerPos();
    }

    private void StartBacktracking()
    {
        
        //newDestination = SampleBacktrackPoints();

        //_animDestination = newDestination;
        //_agent.stoppingDistance = 0.0f;
        _agent.stoppingDistance = 0.0f;
        //isBackTracking = true;
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

    /*IEnumerator SetDestination(Vector3 dest, Transform dest2, float stopDist)
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
    }*/

    /*private void Backtrack()
    {
        Vector3 newDirection = newDestination - transform.position;
        float distToNew = newDirection.magnitude;
        _animation.BackTracking();
        if (distToNew <= (_agent.stoppingDistance + 0.5f) || distToPlayer > _tempStopDistance)
        {
            // Backtracking completed
            //isBackTracking = false;
            //_chasePlayer = true;
            _agent.stoppingDistance = _tempStopDistance;
            //_agent.SetDestination(playerPos);
            //_animation.SetBackTracking(false);
        }
    }*/

    /*private Vector3 SampleBacktrackPoints()
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
    }*/

    /*private float CalculateScore(Vector3 position)
    {
        float distanceScore = Vector3.Distance(position, playerPos) * distanceWeight;
        float proximityScore = CalculateEdgeProximityScore(position) * proximityWeight;

        return distanceScore + proximityScore;
    }*/

    /*private float CalculateEdgeProximityScore(Vector3 point)
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
    }*/






    private void ChasePlayer()
    {
        bool _checkValidPath = CheckPathValidity(GetClosestPointOnNavMesh(transform.position), playerPos);

        if(!_checkValidPath )
        {
            BaseState newState = GetComponent<StationaryState>();
            _stateController.RequestStateChange(newState);
        }
        else
        {
            _agent.stoppingDistance = 5.0f;
            _agent.SetDestination(playerPos);
        }


        if (distToPlayer <= (_tempStopDistance - 1.0f))
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;

                BaseState newState = GetComponent<BacktrackState>();
                _stateController.RequestStateChange(newState);
                /*BaseState newState = new BacktrackState();
                _stateController.RequestStateChange(newState);*/
            }
           
        }

        if (distToPlayer <= (_tempStopDistance + 0.1f))
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(PlayerProximityDelay());

                /*_animation.StoppingDistance();
                BaseState newState = new StationaryState();
                _stateController.RequestStateChange(newState);*/
            }
            
            //_chasePlayer = false;
            //_isStationary = true;
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
       



       
       /* if (_checkValidPath && distToPlayer > _agent.stoppingDistance)
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
            //_chasePlayer = false;
            //_isStationary = true;
            StartCoroutine(SetDestination(transform.position, null, 0.0f));
            //SetAgentDestination(transform.position, null);
            //}
            //_agent.SetDestination(transform.position);
        }*/
       


    }

    IEnumerator PlayerProximityDelay()
    {
        yield return new WaitForSeconds(0.2f);

        Debug.LogError("Successful change");
        //_animation.StoppingDistance();
        _coroutine = null;
        BaseState newState = GetComponent<StationaryState>();
        _stateController.RequestStateChange(newState);
        //_stateController.RequestStateChangeToStationary();
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
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 10f, NavMesh.GetAreaFromName("Walkable")))
        {
            return hit.position;
        }
        return position;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 playerDirection = playerPos - transform.position;
        playerDirection.y = 0.0f;
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        //rotation.y = 0.0f;
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
        //Debug.LogError("Start is: Happening");
        //base.ExitState();
        _chasePlayer = false;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        
    }

    
}
