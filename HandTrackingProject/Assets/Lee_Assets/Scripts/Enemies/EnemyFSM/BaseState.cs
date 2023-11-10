using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The base class of the enemy states
/// Contains the shared variables and classes required in each state
/// </summary>
public abstract class BaseState : MonoBehaviour
{
    protected NavMeshAgent _agent;
    protected AnimationState _animation;
    protected StateController _stateController;
    protected FieldOfView _fieldOfView;
    protected bool _canSeePlayer;


    protected virtual void Awake()
    {
        _fieldOfView = GetComponent<FieldOfView>();
        _stateController = GetComponent<StateController>();
        _agent = GetComponent<NavMeshAgent>();
        _animation = GetComponent<AnimationState>();
    }
    protected virtual void OnEnable() { }

    public virtual void EnterState() { }

    public virtual void UpdateState()
    {
        _canSeePlayer = _fieldOfView.PlayerInView();
    }

    public abstract void LateUpdateState();

    public virtual void ExitState() { }

}
