using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : BaseState
{
    private Coroutine _disableTime;
    private WaitForSeconds _disableDelay;
    private float _shotTimer = 3.0f;
    private MeshCollider m_collider;
    //private float _slicedTimer = 0.1f;



    public bool IsEnabled = true;
    public delegate void EnemyEvent();
    public event EnemyEvent OnDisabled;
    
    public enum DeathType
    {
        Shot,
        Sliced
    }
    public DeathType deathType;


    protected override void Awake()
    {
        base.Awake();
        m_collider = GetComponentInChildren<MeshCollider>();
        _disableDelay = new WaitForSeconds(_shotTimer);
    }

    public override void EnterState()
    {
        m_collider.enabled = false;
        if (deathType == DeathType.Shot)
        {
            _agent.enabled = false;

            if (_disableTime == null)
            {
                _disableTime = StartCoroutine(DisableCountdown());
            }
        }else if(deathType == DeathType.Sliced)
        {
            DisableObject();
        }
        //base.EnterState();
    }

   /* public void SelectDeathType(DeathType type)
    {
        deathType = type;
    }*/

   /* public DeathType GetTypes()
    {
        return deathType;
    }*/

    /// <summary>
    /// Disables agent after a short time after the agent is killed by the player
    /// And invokes the OnDisabled event
    /// </summary>
    /// <returns></returns>
    IEnumerator DisableCountdown()
    {
        yield return _disableDelay;

        _disableTime = null;
        //Disable();
        DisableObject();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    private void DisableObject() => transform.root.gameObject.SetActive(false);

    public void Disable()
    {
        /*Debug.Log("Disable is being invoked properly!!!!!!!!!!!!!!!!");*/
        IsEnabled = false;
        //Debug.Log("Number of subscribers: " + (OnDisabled?.GetInvocationList().Length ?? 0));
        OnDisabled?.Invoke();
    }

    private void OnDisable()
    {
        Disable();
    }

    public void Revive()
    {
        IsEnabled = true;
    }

    public override void LateUpdateState() { }

    public override void ExitState()
    {
        m_collider.enabled = true;
        deathType = DeathType.Shot;
    }



}
