using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveState : BaseState
{
    private GameObject _playerCollider;
    private PlayerCollider _playerDefence;
    private SphereCollider _collider;

    private Rigidbody rb;
    //private Transform target;
    public float speed = 0.0f;
    private float _startSpeed;
    private Vector3 goalPosition;
    //private Vector3 endPosition;
    private Vector3 _shootPosition;
    //[SerializeField] Vector3 testGoal;
    //DeflectPosition spawner;
    BulletSpawn spawner;

    private GameObject bulletStartPoint;
    //[SerializeField] GameObject _bulletSpawn;
    private bool _deflect = false;
    //private Vector3 newPos;
    private Vector3 movementVector;
    //private float x, y, z;
    [DoNotSerialize] public float _timer = 6.0f;
    //private float _resetTime = 6.0f;
    private bool _freezeTimer = false;
    //private bool _deflect = false;
    [SerializeField] AudioSource _deflectSound;

    //float minDist = 1f;
    private Coroutine _coroutine;

    [SerializeField] Transform _raycastOrigin;
    private RaycastHit _hit;

    //public bool _shouldHitEnemy = false;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        //spawner = bulletStartPoint.GetComponent<BulletSpawn>();
        //Debug.LogWarning("Hi how are you");
    }
    /*protected override void OnEnable()
    {

    }*/


    public override void EnterState()
    {
        _collider.enabled = true;
        //gameObject.tag = "IgnoreCollision";
        //_coroutine = StartCoroutine(ResetBulletTag());
        
        //_shouldHitEnemy = false;
        _deflectSound.Play();
       /* _timer = 6.0f;
        speed = 1.0f;
        _startSpeed = speed;
        _playerCollider = GameObject.FindGameObjectWithTag("DefenceTarget");
        _playerDefence = _playerCollider.GetComponent<PlayerCollider>();*/

        //bulletStartPoint = GetBulletStartPoint();

        if (bulletStartPoint != null)
        {
            goalPosition = bulletStartPoint.transform.position + new Vector3(UnityEngine.Random.Range(-spawner.spawnerSize.x / 2, spawner.spawnerSize.x / 2),
                                                                  UnityEngine.Random.Range(-spawner.spawnerSize.y / 2, spawner.spawnerSize.y / 2),
                                                                  UnityEngine.Random.Range(-spawner.spawnerSize.z / 2, spawner.spawnerSize.z / 2));
        }



    }

    public void InitializeBullet(Vector3 shootPosition, GameObject _bulletStartPoint)
    {
        bulletStartPoint = _bulletStartPoint;
        _shootPosition = shootPosition;
        spawner = bulletStartPoint.GetComponent<BulletSpawn>();
        SetMovementVector();
        _collider.enabled = true;
        _timer = 6.0f;
    }

    private void OnDisable()
    {
        _collider.enabled = false;
    }

    private IEnumerator ResetBulletTag()
    {
        yield return new WaitForSeconds(0.5f);

        // Set the bullet's tag back to the "Bullet" tag
        gameObject.tag = "Head";
    }

    public void ResetValues()
    {
        spawner= null;
        goalPosition = Vector3.zero;
        _playerCollider = null;
        _playerDefence = null;
    }

    public override void UpdateState()
    {
        if (Physics.Raycast(_raycastOrigin.position, _raycastOrigin.forward, out _hit, 0.1f))
        {
            //Debug.DrawRay(transform.position, _raycastOrigin.forward * _hit.distance, Color.yellow);
            if (_hit.transform.tag == "Blade")
            {

                ChangeDirection(speed * 30);

            }
        }

        if (!_freezeTimer)
        {
            //Debug.LogError("Timer: " + _timer);
            if (_timer > 0.0f)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
                //Destroy(transform.parent.gameObject);
            }
        }
    }

    public void FixedUpdateState()
    {
        /*if (Physics.Raycast(_raycastOrigin.position, _raycastOrigin.forward, out _hit, 0.4f))
        {
            //Debug.DrawRay(transform.position, _raycastOrigin.forward * _hit.distance, Color.yellow);
            if (_hit.transform.tag == "Blade")
            {

                ChangeDirection(speed * 30);

            }
        }*/

        if (_deflect)
        {
            //goalPosition = _bulletSpawn.transform.position;
            //movementVector = (goalPosition - transform.position).normalized * (speed * 30);

            transform.position = bulletStartPoint.transform.position;

            //rb.MovePosition(transform.position - _bulletSpawn.transform.position);
        }

        if (movementVector != null)
        {

            rb.MovePosition(transform.position + movementVector * Time.fixedDeltaTime);

        }
    }

   

   /* public void SetBulletStartPoint(GameObject o)
    {
        //spawner = _bulletSpawn;
        bulletStartPoint = o;
    }*/

    private void SetMovementVector()
    {
        //_shootPosition = playerPosition;
        //_points = new Vector3[] { transform.position, _shootPosition };
        //_shootPosition = GetShootPosition();
        movementVector = (_shootPosition - transform.position).normalized * (speed * 7.5f);
    }

   /* public void SetShootPosition(Vector3 shootPos)
    {
        _shootPosition = shootPos;
        SetMovementVector();

    }*/

    public void ChangeDirection(float speed)
    {
        bool hit = HitOrMiss();

        if (!hit)
        {
            _deflect = false;
            goalPosition = bulletStartPoint.transform.position + new Vector3(UnityEngine.Random.Range(-spawner.spawnerSize.x / 2, spawner.spawnerSize.x / 2),
                                                                  UnityEngine.Random.Range(-spawner.spawnerSize.y / 2, spawner.spawnerSize.y / 2),
                                                                  UnityEngine.Random.Range(-spawner.spawnerSize.z / 2, spawner.spawnerSize.z / 2));
            
            movementVector = (goalPosition - transform.position).normalized * (speed);

            
        }
        else
        {
            _deflect = true;
        }

    }

    private bool HitOrMiss()
    {
        int num = UnityEngine.Random.Range(1, 3);
       
        return num == 2;
    }

    public override void ExitState()
    {
        //StopCoroutine(ResetBulletTag());
        //_coroutine = null;
        _collider.enabled = false;
        ResetValues();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Blade")
        {
            //_shouldHitEnemy = true;
            ChangeDirection(speed * 30);

        }
        /*else
        {
            gameObject.SetActive(false);
        }*/
    }

    public override void LateUpdateState()
    {
        throw new NotImplementedException();
    }
}
