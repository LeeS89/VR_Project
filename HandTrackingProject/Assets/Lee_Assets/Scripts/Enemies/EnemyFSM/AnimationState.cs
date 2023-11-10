using Oculus.Platform.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationState : MonoBehaviour
{

    [SerializeField] SkinnedMeshRenderer m_Renderer;
    [SerializeField] Material _originalMat;
    [SerializeField] Material _damageMat;
    private float _damageVisual = 0.0f;
    /// <summary>
    /// /////////////////////
    /// </summary>

    public float targetSpeed = 0f; // Target speed for the agent
    public float speedTransitionDuration = 1f;

    [SerializeField] Animator _animator;
    [SerializeField] NavMeshAgent _agent;
    //[SerializeField] Transform destination;
    //private Vector3 _animDestination;
    private int id;
    private float currentValue;
    [SerializeField] float animSpeed = 0.0f;
   
    //[SerializeField] float _runningDistance = 4f;

    private float timer;
    //private ChaseUpdated _chase;

    //float distToTarget;
    //private Vector3 _destPos;
    //private bool _shouldDisable = false;
    private NavMeshObstacle _obstacle;

    //private bool _backTracking = false;
    private void Awake()
    {
        //_obstacle = GetComponent<NavMeshObstacle>();
        //_destPos = destination.position;
        //_chase = GetComponent<ChaseUpdated>();
        //_runningDistance *= _agent.stoppingDistance;
        id = GetComponentInChildren<EnemyHealthController>().id;
        //previousPosition = transform.position;
    }

    private void OnEnable()
    {
        if (EnemyDamageEvent.instance != null)
        {
            EnemyDamageEvent.instance.onDeathEnter += TriggerDeathAnimation;
        }
    }

    private void OnDisable()
    {
        EnemyDamageEvent.instance.onDeathEnter -= TriggerDeathAnimation;
    }

    // Start is called before the first frame update
    void Start()
    {

        EnemyDamageEvent.instance.onDeathEnter += TriggerDeathAnimation;
        //_animator.SetFloat("y", 1f);
        //ChasePlayer();
    }



    // Update is called once per frame
    void Update()
    {


        //distToTarget = Vector3.Distance(transform.position, destination.position);
        //_backTracking = _chase.isBackTracking;
        //RotateTowardsPlayer();
        /*if (_backTracking)
        {
            HandleBacktracking();
        }
        else
        {
            ChasePlayer();

        }*/
        
    }

   /* private void LateUpdate()
    {
        UpdateDestination(_chase.GetDestination());
    }*/


    private void RotateTowardsPlayer()
    {
        Vector3 playerDirection = PlayerPosSearch.instance.GetPlayerPos() - transform.position;
        playerDirection.y = 0.0f;
        //Vector3 playerDirection = PlayerPosSearch.instance.GetPlayerPos();
        //Quaternion rotation = Quaternion.LookRotation(playerDirection);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 4f);
        transform.rotation = Quaternion.LookRotation(playerDirection);

        //transform.LookRotation(playerDirection);
    }

    public void RunningDistance()
    {
        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / 1f);
        currentValue = Mathf.Lerp(_agent.speed, 1f, progress);



        _agent.speed = 1f;
        _animator.SetFloat("x", 0.0f);
        _animator.SetFloat("y", currentValue);
    }

    public void WalkingDistance()
    {
        targetSpeed = 0.5f;
        float _tempSpeed = _agent.speed;

        _tempSpeed = Mathf.Lerp(_tempSpeed, targetSpeed, Time.deltaTime / speedTransitionDuration);
        _agent.speed = Mathf.Lerp(_agent.speed, targetSpeed, Time.deltaTime / speedTransitionDuration);

        _animator.SetFloat("x", 0.0f);
        _animator.SetFloat("y", _tempSpeed);
    }

    public void StoppingDistance()
    {
        targetSpeed = 0f;
        float _tempSpeed = _agent.speed;
        _tempSpeed = Mathf.Lerp(_tempSpeed, targetSpeed, Time.deltaTime / speedTransitionDuration);

        // Update the agent's speed using Lerp for a smooth transition
        _agent.speed = Mathf.Lerp(_agent.speed, targetSpeed, Time.deltaTime / speedTransitionDuration);


        

        _animator.SetFloat("x", 0.0f);
        _animator.SetFloat("y", _tempSpeed);
        /*Vector3 playerDirection = destination.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 4f);*/
    }

    public void BackTracking()
    {

        RotateTowardsPlayer();
        targetSpeed = 0.5f;
        float _tempSpeed = _agent.speed;
        _tempSpeed = Mathf.Lerp(_tempSpeed, targetSpeed, Time.deltaTime / speedTransitionDuration);
        _agent.speed = Mathf.Lerp(_agent.speed, targetSpeed, Time.deltaTime / speedTransitionDuration);

        //_agent.speed = 0.5f;
        _animator.SetFloat("x", 0.0f);
        _animator.SetFloat("y", -(_tempSpeed *2f));

       /* Debug.LogError("X is: " + _animator.GetFloat("x"));
        Debug.LogError("Y is: " + _animator.GetFloat("y"));*/

        /* _agent.speed = 0.0f;
         _animator.SetFloat("x", 0.0f);
         _animator.SetFloat("y", 0.0f);
 */


    }

    public void Idle()
    {
        targetSpeed = 0f;
        float _tempSpeed = _agent.speed;
        _tempSpeed = Mathf.Lerp(_tempSpeed, targetSpeed, Time.deltaTime / speedTransitionDuration);

        // Update the agent's speed using Lerp for a smooth transition
        _agent.speed = Mathf.Lerp(_agent.speed, targetSpeed, Time.deltaTime / speedTransitionDuration);




        _animator.SetFloat("x", 0.0f);
        _animator.SetFloat("y", _tempSpeed);
    }

    public void SetAlert()
    {
        _animator.SetTrigger("Alert");
    }


    #region Test Can Delete Later
    /*private void ChasePlayer()
    {



       *//* if (distToTarget > _agent.stoppingDistance)
        {
            if (!_agent.enabled)
            {
                _shouldDisable = true;
                //_agent.enabled = true;
            }
        }*//*

        if (distToTarget <= _agent.stoppingDistance)
        {
            targetSpeed = 0f;

            // Update the agent's speed using Lerp for a smooth transition
            _agent.speed = Mathf.Lerp(_agent.speed, targetSpeed, Time.deltaTime / speedTransitionDuration);


            _shouldDisable = false;
            //_agent.enabled = false;

            _animator.SetFloat("x", _agent.speed);
            _animator.SetFloat("y", _agent.speed);
            Vector3 playerDirection = destination.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 4f);

        }
        else if (distToTarget <= _runningDistance)
        {
            targetSpeed = 0.5f;

            _agent.speed = Mathf.Lerp(_agent.speed, targetSpeed, Time.deltaTime / speedTransitionDuration);

            _animator.SetFloat("x", 0.0f);
            _animator.SetFloat("y", _agent.speed);


        }
        else
        {

            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / 1f);
            currentValue = Mathf.Lerp(_agent.speed, 1f, progress);



            _agent.speed = 1f;
            _animator.SetFloat("x", 0.0f);
            _animator.SetFloat("y", currentValue);

        }




    }*/

    /*public void SetBackTracking(bool backtrack)
    {
        _backTracking = backtrack;
    }*/

    /*private void HandleBacktracking()
    {
        //_agent.stoppingDistance = 0.0f;

        float distToDest = Vector3.Distance(transform.position, _agent.destination);

        _agent.speed = 0.5f;
        _animator.SetFloat("x", 0.0f);
        _animator.SetFloat("y", -1.0f);

        if (distToDest <= _agent.stoppingDistance)
        {
            _agent.speed = 0.0f;
            _animator.SetFloat("x", 0.0f);
            _animator.SetFloat("y", 0.0f);

            _shouldDisable = false;
            //_agent.enabled = false;

        }
        else if (distToDest > _agent.stoppingDistance)
        {

            _shouldDisable = true;
            //_agent.enabled = true;

        }
    }*/

    /*public bool GetShouldDisable()
    {
        return _shouldDisable;
    }
*/

    /*private void LateUpdate()
    {
        
    }*/
    #endregion


    private void TriggerDeathAnimation(int id)
    {
        if (id == this.id)
        {
            try
            {
                _animator.SetTrigger("Death");

            }
            catch (NullReferenceException e)
            {
                
                Debug.LogWarning(e);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            _damageVisual = 0.5f;
        }
    }
}
