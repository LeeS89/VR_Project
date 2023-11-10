using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;

public class BacktrackState : BaseState
{
    private static List<Vector3> recentBacktrackPoints = new List<Vector3>();
    private const float POINT_REUSE_COOLDOWN = 5.0f; // seconds
    private const float MIN_DISTANCE_BETWEEN_POINTS = 2.0f; // units


    //private AnimationState _animation;
    private Vector3 newDestination;
    //private NavMeshAgent _agent;

    //private float distanceWeight = 1.0f;    
    //private float proximityWeight = 1.0f;

    private Vector3 playerDirection;
    private float distToPlayer;
    private Vector3 playerPos;
    private float _tempStopDistance = 5.0f;
    private NavMeshPath _path;
    private Coroutine _coroutine;

    private bool _shouldUpdate = false;

    public GameObject _destCube;
    /* private void Awake()
     {
         _animation = GetComponent<AnimationState>();
         _agent = GetComponent<NavMeshAgent>();
     }*/

    protected override void Awake()
    {
        base.Awake();
        
    }

    public override void EnterState()
    {
        //_coroutine = StartCoroutine(StartUpdate());
        newDestination = Vector3.zero;
        //_agent.SetDestination(Vector3.zero);
        //_agent.stoppingDistance = 0.0f;
        _path = new NavMeshPath();

        if (!_agent.enabled)
        {
            _agent.enabled = true;
        }
        StartBacktracking();
        
    }

    /*IEnumerator StartUpdate()
    {
        yield return new WaitForSeconds;

        _shouldUpdate = true;

        _coroutine = null;
    }*/

    public override void UpdateState()
    {
        playerDirection = playerPos - transform.position;
        distToPlayer = playerDirection.magnitude;

        _animation.BackTracking();

        if (_shouldUpdate)
        {
            Backtrack();
        }
        /*if(_coroutine != null)
        {
            if(distToPlayer <= (_tempStopDistance - 1.0f))
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
                StartBacktracking();
            }
        }*/
        //RotateTowardsPlayer();

    }

    /*private void LateUpdate()
    {
        Backtrack();
        UpdatePlayerPos();
    }*/

    public override void LateUpdateState()
    {
        /*Backtrack();*/
        UpdatePlayerPos();
    }

    private void RotateTowardsPlayer()
    {
        Vector3 playerDirection = playerPos - transform.position;
        playerDirection.y = 0.0f;

        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 4f);
    }

    

    private void StartBacktracking()
    {


        //_chasePlayer = false;

        //isBackTracking = true;
        //newDestination = SampleBacktrackPoints();

        //_animDestination = newDestination;
        //bool _validPath = CheckPathValidity(GetClosestPointOnNavMesh(transform.position), newDestination);

        //if (_validPath)
        //{
        //newDestination = (Vector3)SampleForBacktrack();
        newDestination = SampleAndVisualize();
        _agent.stoppingDistance = 0.0f;

        //isBackTracking = true;
        if (newDestination != Vector3.zero)
        {
            _agent.SetDestination(newDestination);
            //GameObject e = Instantiate(_destCube, newDestination, _destCube.transform.rotation);

            _shouldUpdate = true;
        }
        else
        {
            //StartBacktracking();
            transform.position = 2 * (transform.position - playerPos);

            //GetComponent<NewChasingState>().ResetPos();
            BaseState newState = GetComponent<NewChasingState>();

            _stateController.RequestStateChange(newState);
        }
        //}
        /*else
        {
            StartBacktracking();
        }*/


    }
    #region Sampling new method

    public Vector3 SampleAndVisualize()
    {
        List<Vector3> navMeshHitPoints = new List<Vector3>();
        float angleRange = 180.0f;
        int numSamples = 8;
        float samplingRadius = 2f;

         

        // Adjust ordering to start with 12 o'clock, then go counterclockwise, then clockwise.
        List<float> sampleAngles = new List<float>();
        sampleAngles.Add(0);
        for (int i = 1; i <= (numSamples - 1) / 2; i++)
        {
            sampleAngles.Add(-i * (angleRange / (numSamples - 1)));
            sampleAngles.Add(i * (angleRange / (numSamples - 1)));
        }

        // Get points based on NavMesh.Raycast
        foreach (float angle in sampleAngles)
        {
            Vector3 sampleDirection = Quaternion.Euler(0, angle, 0) * -transform.forward;

            NavMeshHit navHit;
            if (NavMesh.Raycast(GetClosestPointOnNavMeshs(transform.position), GetClosestPointOnNavMeshs(transform.position) + sampleDirection.normalized * samplingRadius, out navHit, NavMesh.AllAreas))
            {
                navMeshHitPoints.Add(navHit.position);
                Debug.DrawLine(transform.position, navHit.position, Color.blue, 2.0f);
            }
            else
            {
                navMeshHitPoints.Add(transform.position + sampleDirection.normalized * samplingRadius);
                Debug.DrawLine(transform.position, transform.position + sampleDirection.normalized * samplingRadius, Color.red, 2.0f);
            }
        }

        // Validate those points
        Vector3 validPoint = Vector3.zero;

        foreach (var point in navMeshHitPoints)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(point, out hit, 0.5f, NavMesh.AllAreas))
            {
                // Check for straight-line obstructions using NavMesh.Raycast
                NavMeshHit raycastHit;
                if (!NavMesh.Raycast(GetClosestPointOnNavMeshs(transform.position), hit.position, out raycastHit, NavMesh.AllAreas))
                {
                    // Ray didn't hit an obstruction; now check for valid path
                    NavMeshPath path = new NavMeshPath();
                    if (NavMesh.CalculatePath(GetClosestPointOnNavMeshs(transform.position), hit.position, NavMesh.AllAreas, path))
                    {
                        Vector3 _directionToNewDest = hit.position - transform.position;
                        float _distance = _directionToNewDest.magnitude;

                        if (_distance >= (samplingRadius - 0.15f))
                        {
                            if (path.status == NavMeshPathStatus.PathComplete)
                            {
                                Debug.DrawLine(transform.position, hit.position, Color.green, 2.0f);

                                if (validPoint == Vector3.zero)
                                {
                                    validPoint = hit.position; // Assign to validPoint only if it's null
                                }
                            }
                        }
                    }
                }else if(NavMesh.Raycast(GetClosestPointOnNavMeshs(transform.position), hit.position, out raycastHit, NavMesh.AllAreas))
                {
                    NavMeshPath path = new NavMeshPath();
                    if (NavMesh.CalculatePath(GetClosestPointOnNavMeshs(transform.position), hit.position, NavMesh.AllAreas, path))
                    {
                        Vector3 _directionToNewDest = hit.position - transform.position;
                        float _distance = _directionToNewDest.magnitude;

                        if (_distance >= (samplingRadius - 0.15f))
                        {
                            if (path.status == NavMeshPathStatus.PathComplete)
                            {
                                Debug.DrawLine(transform.position, hit.position, Color.green, 2.0f);

                                if (validPoint == Vector3.zero)
                                {
                                    validPoint = hit.position; // Assign to validPoint only if it's null
                                }
                            }
                        }
                    }
                }
            }
        }

        return validPoint;
    }

    private Vector3 GetClosestPointOnNavMeshs(Vector3 position)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(position, out hit, 5.0f, NavMesh.AllAreas);
        return hit.position;
    }

    private Vector3 GetClosestPointOnNavMesh(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return position;
    }

    #endregion
    #region New backtrack method (Redundant)
    /*public Vector3? SampleForBacktracking()
    {
        int numSamples = 8;
        float angleRange = 180.0f;
        float samplingRadius = 3.5f;
        float angleIncrement = angleRange / (numSamples - 1);

        Vector3? primaryPoint = null; // Store the first valid point found

        NavMeshPath path = new NavMeshPath();  // Used to check path validity

        // Helper function to check if a path is valid
        bool IsValidPath(Vector3 start, Vector3 end)
        {
            if (NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path))
            {
                return path.status == NavMeshPathStatus.PathComplete;
            }
            return false;
        }

        // 12 o'clock direction first
        Vector3 backDirection = -transform.forward;
        Vector3 samplePosition = transform.position + backDirection.normalized * samplingRadius;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(samplePosition, out hit, 0.5f, NavMesh.AllAreas) && IsValidPath(GetClosestPointOnNavMesh(transform.position), hit.position))
        {
            Debug.DrawLine(transform.position, hit.position, Color.green, 2.0f);
            primaryPoint = hit.position;  // Store this point if it's valid
        }
        else
        {
            Debug.DrawLine(transform.position, samplePosition, Color.red, 2.0f);
        }

        // Then from 12 to 9
        for (float angle = -angleIncrement; angle >= -90.0f; angle -= angleIncrement)
        {
            Vector3 sampleDirection = Quaternion.Euler(0, angle, 0) * backDirection;
            samplePosition = transform.position + sampleDirection.normalized * samplingRadius;

            if (NavMesh.SamplePosition(samplePosition, out hit, 0.5f, NavMesh.AllAreas) && IsValidPath(transform.position, hit.position))
            {
                Debug.DrawLine(transform.position, hit.position, Color.green, 2.0f);
                if (!primaryPoint.HasValue)
                    primaryPoint = hit.position;
            }
            else
            {
                Debug.DrawLine(transform.position, samplePosition, Color.red, 2.0f);
            }
        }

        // From 12 to 3
        for (float angle = angleIncrement; angle <= 90.0f; angle += angleIncrement)
        {
            Vector3 sampleDirection = Quaternion.Euler(0, angle, 0) * backDirection;
            samplePosition = transform.position + sampleDirection.normalized * samplingRadius;

            if (NavMesh.SamplePosition(samplePosition, out hit, 0.5f, NavMesh.AllAreas) && IsValidPath(transform.position, hit.position))
            {
                Debug.DrawLine(transform.position, hit.position, Color.green, 2.0f);
                if (!primaryPoint.HasValue)
                    primaryPoint = hit.position;
            }
            else
            {
                Debug.DrawLine(transform.position, samplePosition, Color.red, 2.0f);
            }
        }

        return primaryPoint;
    }*/
    #endregion

    #region Finding destination to backtrack to (Redundant Can remove later)


    private void Update()
    {
        //SampleBacktrackPoints();
        /*Vector3 backward = -transform.forward;
        Vector3 left = Quaternion.Euler(0, -90, 0) * backward;
        Vector3 right = Quaternion.Euler(0, 90, 0) * backward;

        Debug.DrawLine(transform.position, transform.position + backward * 5.0f, Color.green, 2.0f);
        Debug.DrawLine(transform.position, transform.position + left * 5.0f, Color.yellow, 2.0f);
        Debug.DrawLine(transform.position, transform.position + right * 5.0f, Color.red, 2.0f);*/
        //RaycastSample();
        //SampleBacktrackDestinations();
        //GetValidBacktrackDestination();
        /*SampleAndVisualize();*/
        /*SampleForBacktrack();*/
        //GetValidBacktrackPoint();
        //SampleForBacktracking();
        //SampleAndVisualize();
    }

    
/*
    private bool IsValidPoint(Vector3 point)
    {
        foreach (Vector3 usedPoint in recentBacktrackPoints)
        {
            if (Vector3.Distance(usedPoint, point) < MIN_DISTANCE_BETWEEN_POINTS)
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator ClearBacktrackPointAfterDelay(Vector3 point, float delay)
    {
        yield return new WaitForSeconds(delay);
        recentBacktrackPoints.Remove(point);
    }*/
    
    #endregion


    private bool CheckPathValidity(Vector3 _startPoint, Vector3 _endPoint)
    {
        _path = new NavMeshPath();
        bool hasPath = NavMesh.CalculatePath(_startPoint, _endPoint, NavMesh.AllAreas, _path);

        NavMeshPathStatus pathStatus = _path.status;
        if (pathStatus != NavMeshPathStatus.PathComplete)
        {

            Debug.LogWarning("Path Status: " + pathStatus);
            return false;
        }

        return hasPath && _path.corners.Length > 0; ;
    }

    private void Backtrack()
    {
        Vector3 newDirection = newDestination - transform.position;
        float distToNew = newDirection.magnitude;
        _animation.BackTracking();

        bool _validPath = CheckPathValidity(transform.position, newDestination);

        if (_validPath)
        {
            Debug.LogError("Path is bloody valid, so work please!!");

            if (distToNew <= (_agent.stoppingDistance + 0.75f) || distToPlayer >= _tempStopDistance)
            {
                // Backtracking completed
                //isBackTracking = false;
                //_chasePlayer = true;
                if (_coroutine == null)
                {

                    BaseState newState = GetComponent<NewChasingState>();
                    //_stateController.RequestStateChange(newState);
                    _coroutine = StartCoroutine(StateChange(newState));
                }
                //_agent.SetDestination(playerPos);
                //_animation.SetBackTracking(false);
            }
        }
        else
        {
            Debug.LogError("Ahh, so the path is not valid!!");
            if (_coroutine == null)
            {
                StartBacktracking();
            }
        }

        /*else if (distToPlayer <= _tempStopDistance - 1.0f)
        {
            StartBacktracking();
        }*/

    }

    IEnumerator StateChange(BaseState _newState)
    {
        yield return new WaitForSeconds(0.2f);

        if (distToPlayer <= _tempStopDistance - 1.0f)
        {
            StartBacktracking();
            StopCoroutine(StateChange(_newState));
            _coroutine = null;
        }
        else
        {
            _agent.stoppingDistance = _tempStopDistance;

            _stateController.RequestStateChange(_newState);
        }
    }

    

    

    private void UpdatePlayerPos()
    {
        playerPos = PlayerPosSearch.instance.GetPlayerPos();
    }

    public override void ExitState()
    {
        base.ExitState();
        _shouldUpdate= false;
        if(_coroutine != null )
        {
            _coroutine = null;
        }
    }

    
}
