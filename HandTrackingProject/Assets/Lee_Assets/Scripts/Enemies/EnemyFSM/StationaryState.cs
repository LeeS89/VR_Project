using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StationaryState : BaseState
{
    //private AnimationState _animation;
    private NavMeshObstacle _obstacle;
    private Vector3 playerPos;
    private NavMeshPath _path;
    private bool _start = false;

    //private NavMeshAgent _agent;

    private Vector3 playerDirection;
    private float distToPlayer;
    private float _tempStopDistance = 5.0f;

    private Coroutine _coroutine;
    private float _tempRadius = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        _path = new NavMeshPath();
        //_agent = GetComponent<NavMeshAgent>();
        _obstacle = GetComponent<NavMeshObstacle>();

    }

    public override void EnterState()
    {
        //Debug.LogError("Ok so this is correct then!!!!!");

        playerPos = PlayerPosSearch.instance.GetPlayerPos();
        _agent.stoppingDistance = 0.0f;
        _agent.SetDestination(transform.position);

        //_coroutine = StartCoroutine(BeginUpdate());


        //Debug.LogError("Yaaaaaaayyy!!!");
        if (_agent.enabled)
        {
            _agent.enabled = false;
        }

        if (!_obstacle.carving)
        {
            _obstacle.carving = true;
        }


    }

    /*IEnumerator BeginUpdate()
    {
        Debug.LogError("Should be working fine!!!");
        yield return new WaitForEndOfFrame();

        _start = true;
    }*/


    public override void UpdateState()
    {
        //_agent.SetDestination(transform.position);
        //if (_start)
        //{

            //Debug.LogError("Should be working fine!!!");
            playerDirection = playerPos - transform.position;
            distToPlayer = playerDirection.magnitude;


            RotateTowardsPlayer();
            Stationary();

            _animation.StoppingDistance();
        //}
        //Debug.LogError("Start is: " + _start);
        
    }

    /*private void LateUpdate()
    {
        UpdatePlayerPos();
    }*/

    public override void LateUpdateState()
    {
        UpdatePlayerPos();
    }

    private void RotateTowardsPlayer()
    {
        Vector3 playerDirection = playerPos - transform.position;
        playerDirection.y = 0.0f;
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        //rotation.y = 0.0f;
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 4f);
    }

    private Vector3 GetClosestPointOnNavMesh(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return position;
    }

    private void Stationary()
    {
        //_agent.SetDestination(transform.position);

        bool _checkValidPath = CheckPathValidity(GetClosestPointOnNavMesh(transform.position), playerPos);
        if (distToPlayer > _tempStopDistance)
        {
            if (_checkValidPath && _coroutine == null)
            {
                if (_obstacle.carving)
                {
                    _obstacle.carving = false;
                }

                /*if (!_agent.enabled)
                {
                    _agent.enabled = true;
                }*/
                BaseState newState = GetComponent<NewChasingState>();
                _coroutine = StartCoroutine(StateChange(newState));
                
                //ExitStationary();
                //_chasePlayer = true;
                //StartCoroutine(SetDestination(playerPos, null, 5.0f));
            }
        }

        if (distToPlayer <= (_tempStopDistance - 1.0f))
        {
            if (_coroutine == null)
            {
                BaseState newState = GetComponent<BacktrackState>();
                _coroutine = StartCoroutine(StateChange(newState));
                //ExitStationary();
                //StartCoroutine(EnterBAcktracking());

                //StartBacktracking();
            }
        }
    }

    IEnumerator StateChange(BaseState _newState)
    {
        if (_obstacle.carving)
        {
            _obstacle.carving = false;
        }

        /*if (!_agent.enabled)
        {
            _agent.enabled = true;
        }*/

        yield return new WaitForSeconds(0.2f);

        /*if (!_agent.enabled)
        {
            _agent.enabled = true;
        }*/
        _stateController.RequestStateChange(_newState);
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

    private void UpdatePlayerPos()
    {
        playerPos = PlayerPosSearch.instance.GetPlayerPos();
    }

    public override void ExitState()
    {
        //Debug.LogError("Yaaaaaaayyy!!!");

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        //_obstacle.enabled= false;
        /*if (_obstacle.carving)
        {
            _obstacle.carving = false;
        }

        if (!_agent.enabled)
        {
            _agent.enabled = true;
        }*/


    }

   
}
