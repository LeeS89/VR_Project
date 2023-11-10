using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    private Vector3 directionToTarget;

    public GameObject _raycastPoint;

    BulletSpawn spawn;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private Coroutine _coroutine;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        spawn = GetComponentInChildren<BulletSpawn>();

        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(FOVRoutine());
        }
    }

    private void OnEnable()
    {
        if(_coroutine == null)
        {
            _coroutine = StartCoroutine(FOVRoutine());
        }
    }

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(FOVRoutine());
            _coroutine = null;
        }
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.3f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void Update()
    {
        Debug.DrawRay(_raycastPoint.transform.position, directionToTarget * 10f, Color.red);
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(_raycastPoint.transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            directionToTarget = (target.position - _raycastPoint.transform.position).normalized;
            

            if (Vector3.Angle(_raycastPoint.transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(_raycastPoint.transform.position, target.position);

                if (!Physics.Raycast(_raycastPoint.transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    //spawn._isShooting = true;
                    canSeePlayer = true;
                }
                else
                {
                    //spawn.CancelShoot();
                    canSeePlayer = false;
                }
            }
            else
            {
                //spawn.CancelShoot();
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            //spawn.CancelShoot();
            canSeePlayer = false;
        }
    }

    public bool PlayerInView()
    {
        return canSeePlayer;
    }
}
