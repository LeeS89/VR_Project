using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFsmController : MonoBehaviour
{
    private BaseState _currentState;
    public ActiveState _activeState;
    private IdleState _idleState;

    private BulletPool _bulletPool;
    //private Rigidbody _rigidbody;

    private GameObject bulletStartPoint;
    private Vector3 _shootPosition;
    private bool _skipFirstEnable = false;


    void Awake()
    {
        //_rigidbody = GetComponent<Rigidbody>();

        _idleState = GetComponent<IdleState>();
        _activeState = GetComponent<ActiveState>();
        
        
        _currentState = _idleState;
        
        _currentState.EnterState();
    }

    private void OnEnable()
    {

        /* if (!_skipFirstEnable)
         {*/
        /*_currentState.ExitState();
        _currentState = _idleState;*/

        //_activeState.SetBulletStartPoint(bulletStartPoint);
        //_activeState.SetShootPosition(_shootPosition);
        //_currentState.EnterState();
        ChangeState(_activeState);
        _skipFirstEnable = true;
        /*}
        else
        {
            ChangeState(_idleState);
            _skipFirstEnable = false;
        }*/



    }

    private void OnDisable()
    {
        ChangeState(_idleState);
        BulletPool.SharedInstance.ReturnToPool(gameObject);
        //_bulletPool.AddToPool(gameObject);
        //_skipFirstEnable = true;
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateState();
        //Debug.LogWarning("Current State: "+ _currentState.ToString());
        /*if(_currentState == _activeState)
        {
            if(_activeState._timer <= 0.0f)
            {
                ChangeState(_idleState);
            }
        }*/
    }

    private void FixedUpdate()
    {
        if(_currentState == _activeState)
        {
            _activeState.FixedUpdateState();
        }
    }

   /* public void SetShootPosition(Vector3 shootPos)
    {
        _shootPosition = shootPos;

    }

    public void SetBulletStartPoint(GameObject o)
    {
        bulletStartPoint = o;
    }*/

    /*public void NewState()
    {
        ChangeState(_activeState);
    }*/

    private void ChangeState(BaseState state)
    {

        //Debug.LogWarning("State Changed to: "+state.ToString());
        /*if (state == _activeState)
        {
            _activeState.SetBulletStartPoint(bulletStartPoint);
            _activeState.SetShootPosition(_shootPosition);

        }*/


        _currentState.ExitState();

        _currentState = state;
        _currentState.EnterState();
    }
}
