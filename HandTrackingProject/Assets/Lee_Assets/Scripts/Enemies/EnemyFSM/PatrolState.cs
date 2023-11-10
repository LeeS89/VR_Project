using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This script handles the enemy patrolling state
/// It is responsible for giving the enemy a destination to travel to and a new destination
/// each time it reaches its current destination.
/// 
/// It also calls the appropriate animations from the animation script depending on wether it is currently walking or paused at its current destination
/// 
/// This script also inherits the field of view from the base class which, when the player enters the field of view this triggers a state change to chasing
/// and alerts each agent within the group of the alerting agent to the players destination
/// </summary>
public class PatrolState : BaseState
{
    public Transform[] _destination;
    private Vector3 _distance;
    private float distance;
    int index = 0;

    //public static bool _alert = false;
    private static AudioSource _source;

    //private FieldOfView _fieldOfView;
    //private bool _canSeePlayer = false;

    private WaitForSeconds _pauseDelay;
    private Coroutine _coroutine;
    public float _delay;

    public AgentGroup parentGroup;

    private WaitUntil _waitUntilDestinationReached;
    
    private Func<bool> hasReachedDestination;

    private bool _walk = false;
    public Animator _animator;

    //public PatrolState _patrolState;
    
    protected override void Awake()
    {
        base.Awake();
        //_fieldOfView = GetComponent<FieldOfView>();
        _pauseDelay = new WaitForSeconds(_delay);
        hasReachedDestination = CheckDestinationReached;
        _waitUntilDestinationReached = new WaitUntil(hasReachedDestination);
        _source = GetComponent<AudioSource>();
    }

    public override void EnterState()
    {
        //base.EnterState();
        
        if(!_agent.enabled)
        {
            _agent.enabled = true;
        }

        if(_destination.Length > 0)
        {
            _agent.SetDestination(_destination[0].position);
            _coroutine = StartCoroutine(StopAndRotate());
        }
        else
        {
            _agent.SetDestination(transform.position);
        }
        _agent.stoppingDistance = 0.0f;

       
    }

    public override void UpdateState()
    {
        
        base.UpdateState();

        if(_walk)
        {
            _animation.WalkingDistance();
        }
        else
        {
            _animation.Idle();
        }


        /*if (_patrolState != null)
        {
            Debug.Log("Why arent you working?!!!!1");
        }*/

       /* if (distance > (_agent.stoppingDistance + 0.5f))
        {
            _animation.WalkingDistance();
        }
        else
        {
            _animation.Idle();
        }*/
        if(_canSeePlayer)
        {

            _animator.SetBool("Alert", true);
            parentGroup.AlertAgents();
        }

        /*if (_canSeePlayer)
        {
            
            _alert = true;
            _source.Play();
            
        }
        
        if(_alert)
        {
            _animation.SetAlert();
            BaseState newState = GetComponent<NewChasingState>();

            _stateController.RequestStateChange(newState);
        }*/

        //Patrol();
    }

    public void ActivateAlertPhase()
    {
        _animation.SetAlert();
        BaseState newState = GetComponent<NewChasingState>();

        _stateController.RequestStateChange(newState);
    }

    private void GetDistanceToDestination()
    {
        _distance = transform.position - _destination[index].position;
        distance = _distance.magnitude;
    }

    private Vector3 GetClosestPointOnNavMesh(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 10f, NavMesh.GetAreaFromName("Walkable")))
        {
            return hit.position;
        }
        return position;
    }

    /*bool HasReachedDestination(NavMeshAgent agent)
    {
        return !agent.pathPending && agent.remainingDistance <= (agent.stoppingDistance + 0.5f) && !agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete;
    }*/

    private bool CheckDestinationReached()
    {
        GetDistanceToDestination();
        return distance <= (_agent.stoppingDistance + 0.5f);
    }


    /// <summary>
    /// While in this current state, this coroutine handles how the agent iterates through its destinations
    /// which are stored in an array. 
    /// While the agent is further than 0.5f away from its current destination, the walking animation is played
    /// yield return _waitUntilDestinationReached; is a cached WaitUntil() using a delegate 'hasReachedDestination', which has the method 
    /// CheckDestinationReached() assigned to it
    /// Once agent reaches its destination, the Idle animation is called and the agent stops for _pauseDelay number of seconds before its destination is updated
    /// </summary>
    /// <returns></returns>
    IEnumerator StopAndRotate()
    {
        while (true)
        {
            _walk = true;
            _agent.SetDestination(_destination[index].position);

            yield return _waitUntilDestinationReached;
           
            _walk = false;
           
            yield return _pauseDelay;

            if (index == _destination.Length - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }
    }

    /*private void Patrol()
    {
        if (_destination.Length > 0)
        {
            _agent.SetDestination(_destination[index].position);
        }
        else
        {
            _agent.stoppingDistance = 0.0f;
            _agent.SetDestination(transform.position);
            return;
        }

        _distance = transform.position - _agent.destination;
        distance = _distance.magnitude;

        if (distance <= _agent.stoppingDistance + 0.5f)
        {

            index++;

            if (index == _destination.Length)
            {
                index = 0;
            }
        }
    }*/

   /* private void UpdateFieldOfView()
    {
        _canSeePlayer = _fieldOfView.PlayerInView();
    }*/


    public override void ExitState()
    {
        base.ExitState();

        if(_coroutine != null)
        {
            StopCoroutine(StopAndRotate());
            _coroutine = null;
        }
    }

    public override void LateUpdateState()
    {
        //UpdateFieldOfView();
    }
}
