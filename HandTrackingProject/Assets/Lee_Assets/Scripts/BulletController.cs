//using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.GraphicsBuffer;

public class BulletController : MonoBehaviour
{
    //[SerializeField]private LineRenderer _lr;
    //private Vector3[] _points;



    private Rigidbody rb;
    private GameObject _playerCollider;
    private PlayerCollider _playerDefence;
    //private Transform target;
    public float speed = 0.0f;
    private float _startSpeed;
    private Vector3 goalPosition;
    //private Vector3 endPosition;
    private Vector3 _shootPosition;
    //[SerializeField] Vector3 testGoal;
    //DeflectPosition spawner;
    BulletSpawn spawner;
    [SerializeField] private GameObject bulletStartPoint;
    [SerializeField] GameObject _bulletSpawn;
    private bool _deflect = false;
    //private Vector3 newPos;
    private Vector3 movementVector;
    //private float x, y, z;
    private float _timer = 6.0f;
    //private float _resetTime = 6.0f;
    private bool _freezeTimer = false;
    //private bool _deflect = false;
    [SerializeField] AudioSource _deflectSound;

    //float minDist = 1f;

    [SerializeField] Transform _raycastOrigin;
    private RaycastHit _hit;

    private void Awake()
    {
        
        //bulletStartPoint = GameObject.Find("BulletGoal");

        //_bulletSpawn = GameObject.FindGameObjectWithTag("BulletSpawn");
        rb = GetComponent<Rigidbody>();
        //_lr = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

        _startSpeed = speed;
        _playerCollider = GameObject.FindGameObjectWithTag("DefenceTarget");
        _playerDefence = _playerCollider.GetComponent<PlayerCollider>();


        //PoseDetection.PopulateList(gameObject);
        //targetDeflect = GameObject.FindGameObjectWithTag("Respawn");


        //Here
        spawner =  GetBulletStartPoint().GetComponent<BulletSpawn>();

        //target = targetDeflect.transform;
        /*_shootPosition = _playerCollider.transform.position + new Vector3(UnityEngine.Random.Range(-_playerDefence.spawnerSize.x / 2, _playerDefence.spawnerSize.x / 2),
                                                              UnityEngine.Random.Range(-_playerDefence.spawnerSize.y / 2, _playerDefence.spawnerSize.y / 2),
                                                              UnityEngine.Random.Range(-_playerDefence.spawnerSize.z / 2, _playerDefence.spawnerSize.z / 2));*/

        //And here
        goalPosition = bulletStartPoint.transform.position + new Vector3(UnityEngine.Random.Range(-spawner.spawnerSize.x / 2, spawner.spawnerSize.x / 2),
                                                              UnityEngine.Random.Range(-spawner.spawnerSize.y / 2, spawner.spawnerSize.y / 2),
                                                              UnityEngine.Random.Range(-spawner.spawnerSize.z / 2, spawner.spawnerSize.z / 2));

       //endPosition = goalPosition;

        /*_points = new Vector3[] { transform.position, _shootPosition };*/
        //_points[0] = transform.position;
        //_points[1] = _shootPosition;


        //testGoal = spawner.spawnerSize;
        /*movementVector = (goalPosition - transform.position).normalized * (speed*2);*/
        //movementVector = (_shootPosition - transform.position).normalized * (speed*9);
        //rb = GetComponent<Rigidbody>();
        //rb.velocity = new Vector3(speed, 0, 0);
    }

    private void OnEnable()
    {
        //ResetValues();
        /*rb = GetComponent<Rigidbody>();
        _timer = 6.0f;
        _playerCollider = GameObject.FindGameObjectWithTag("DefenceTarget");

        spawner = bulletStartPoint.GetComponent<BulletSpawn>();
        _playerDefence = _playerCollider.GetComponent<PlayerCollider>();
        goalPosition = bulletStartPoint.transform.position + new Vector3(UnityEngine.Random.Range(-spawner.spawnerSize.x / 2, spawner.spawnerSize.x / 2),
                                                              UnityEngine.Random.Range(-spawner.spawnerSize.y / 2, spawner.spawnerSize.y / 2),
                                                              UnityEngine.Random.Range(-spawner.spawnerSize.z / 2, spawner.spawnerSize.z / 2));
        speed = _startSpeed;*/
    }

    public void ResetValues()
    {
        rb = GetComponent<Rigidbody>();
        _timer = 6.0f;
        _playerCollider = GameObject.FindGameObjectWithTag("DefenceTarget");

        spawner = bulletStartPoint.GetComponent<BulletSpawn>();
        _playerDefence = _playerCollider.GetComponent<PlayerCollider>();
        goalPosition = bulletStartPoint.transform.position + new Vector3(UnityEngine.Random.Range(-spawner.spawnerSize.x / 2, spawner.spawnerSize.x / 2),
                                                              UnityEngine.Random.Range(-spawner.spawnerSize.y / 2, spawner.spawnerSize.y / 2),
                                                              UnityEngine.Random.Range(-spawner.spawnerSize.z / 2, spawner.spawnerSize.z / 2));
        speed = _startSpeed;
        //SetMovementVector();
    }

    private void Update()
    {
        if (!_freezeTimer)
        {
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

        //for(int i = 0; i < _points.Length; i++)
        //{
        /*if (_points != null)
        {
            _lr.SetPositions(_points);
        }*/
        //_lr.SetPosition(i, _points[i]);
        //}

        //_lr.SetPosition(0, _shootPosition);
        //Vector3 _dir = (_shootPosition - transform.position);

        //Debug.DrawRay(transform.position, _dir, Color.blue);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.localScale = new Vector3(0.3f, 0.02f, 0.02f);
        /* if(_deflect)
         {*/

        /* transform.Translate(targetDeflect.transform.position + new Vector3(UnityEngine.Random.Range(-spawner.spawnerSize.x / 2, spawner.spawnerSize.x / 2),
                                                           UnityEngine.Random.Range(-spawner.spawnerSize.y / 2, spawner.spawnerSize.y / 2),
                                                           UnityEngine.Random.Range(-spawner.spawnerSize.z / 2, spawner.spawnerSize.z / 2)));*/
        //Debug.DrawRay(_raycastOrigin.transform.position, _raycastOrigin.forward * 0.1f, Color.yellow);
        if (Physics.Raycast(_raycastOrigin.position, _raycastOrigin.forward, out _hit, 0.4f))
        {
            //Debug.DrawRay(transform.position, _raycastOrigin.forward * _hit.distance, Color.yellow);
            if (_hit.transform.tag == "Shield" || _hit.transform.tag == "Blade")
            {
                /*if(_hit.transform.tag == "Shield")
                {*/
                //_deflect = true;
                ChangeDirection(speed * 30);
                //}
            }
        }

        if (_deflect)
        {
            /* goalPosition = _bulletSpawn.transform.position;
             movementVector = (goalPosition - transform.position).normalized * (speed * 30);*/

            transform.position = bulletStartPoint.transform.position;

            //rb.MovePosition(transform.position - _bulletSpawn.transform.position);
        }

        if (movementVector != null)
        {
            //if (!_deflect)
            //{
            //ChangeDirection(speed);
            rb.MovePosition(transform.position + movementVector * Time.fixedDeltaTime);
            //}
        }



        /*if (_deflect)
        {
            *//*goalPosition = targetDeflect.transform.position + new Vector3(UnityEngine.Random.Range(-spawner.spawnerSize.x / 2, spawner.spawnerSize.x / 2),
                                                              UnityEngine.Random.Range(-spawner.spawnerSize.y / 2, spawner.spawnerSize.y / 2),
                                                              UnityEngine.Random.Range(-spawner.spawnerSize.z / 2, spawner.spawnerSize.z / 2));*//*
            //endPosition = targetDeflect.transform.position + goalPosition;
            *//*movementVector = (endPosition - transform.position).normalized * (speed);*//*
            ChangeDirection(speed * 22);
            rb.MovePosition(transform.position + movementVector * Time.deltaTime);
        }*/

        /*float _dist = Vector3.Distance(transform.position, goalPosition);
        if(_dist <= minDist)
        {
            _deflect = false;
        }*/
        //rb.DOMove(movementVector* Time.deltaTime,10);
        //transform.position += movementVector * Time.deltaTime;


        //Vector3 newDirection = Vector3.RotateTowards(new Vector3(0,0,0), goalPosition - transform.position, speed, 0.0f);
        //transform.rotation = Quaternion.LookRotation(newDirection);
        //transform.position = Vector3.MoveTowards(transform.position, goalPosition, (speed * 6) * Time.deltaTime);
        //transform.rotation = Quaternion.Euler(x, y, transform.position.z - goalPosition.z);

        //rb.DOLookAt(goalPosition, 0.2f, 0, new Vector3(1, 0, 0));
        //transform.DORotate(goalPosition, 0.5f);
        //Debug.DrawRay(transform.position, goalPosition, Color.red);
        //transform.position = Vector3.MoveTowards(transform.position, targetDeflect.transform.position, (speed*2) * Time.deltaTime);

        //transform.position += movementVector * Time.deltaTime;
        //}
    }

    public void SetBulletStartPoint(GameObject o)
    {
        bulletStartPoint = o;
    }

    private GameObject GetBulletStartPoint()
    {
        return bulletStartPoint;
    }

    private void SetMovementVector()
    {
        //_shootPosition = playerPosition;
        //_points = new Vector3[] { transform.position, _shootPosition };
        movementVector = (_shootPosition - transform.position).normalized * (speed * 6f);
    }

    public void SetShootPosition(Vector3 shootPos)
    {
        _shootPosition = shootPos;
        SetMovementVector();

    }

    public void FreezeSpeed()
    {
        _freezeTimer = true;
        StopBullets();
        //ChangeDirection(0.0f);
    }

    private void StopBullets()
    {
        movementVector = Vector3.zero;
    }

    public void ResumeSPeed()
    {
        //PoseDetection.RemoveFromList(gameObject);
        _freezeTimer = false;
        ChangeDirection(speed * 30);
    }

    public void ChangeDirection(float speed)
    {
        bool hit = HitOrMiss();

        if (!hit)
        {
            _deflect = false;
            goalPosition = bulletStartPoint.transform.position + new Vector3(UnityEngine.Random.Range(-spawner.spawnerSize.x / 2, spawner.spawnerSize.x / 2),
                                                                  UnityEngine.Random.Range(-spawner.spawnerSize.y / 2, spawner.spawnerSize.y / 2),
                                                                  UnityEngine.Random.Range(-spawner.spawnerSize.z / 2, spawner.spawnerSize.z / 2));
            /*movementVector = (endPosition - transform.position).normalized * (speed);*/
            movementVector = (goalPosition - transform.position).normalized * (speed);

            /*endPosition = targetDeflect.transform.position + goalPosition;*/
        }
        else
        {
            _deflect = true;
        }

    }

    private bool HitOrMiss()
    {
        int num = Random.Range(1, 3);
        //bool hit = false;
        return num == 2;
    }

    /* private void OnCollisionEnter(Collision collision)
     {
         if (CompareTag("Blade"))
         {

             //rb.DORotate(goalPosition, 0.5f);

             //newPos = transform.position + goalPosition * (speed*2) * Time.deltaTime;
             //rb.MovePosition(newPos);
             //transform.rotation.SetLookRotation(newPos);
             //transform.rotation = Quaternion.Euler(newPos);
             *//*movementVector = (goalPosition - transform.position).normalized * (speed * 6);*//*
             ChangeDirection(speed * 6);
             //_deflect = true;

             //transform.rotation = Quaternion.Euler(0, 0, 180);

             //transform.Translate(goalPosition.x, goalPosition.y, goalPosition.z, speed*2);
             //rb.velocity = new Vector3 (-speed *2, 0, 0);
             //GetComponent<AudioSource>().Play();
             //_deflectSound.Play();
         }
         if (collision.gameObject.tag == "Reset")
         {
             //PoseDetection.RemoveFromList(gameObject);
             Destroy(gameObject);
         }
         if (collision.gameObject.tag == "Shield")
         {
             ChangeDirection(speed * 6);
         }
         Physics.IgnoreLayerCollision(4, 7);
     }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Blade")
        {

            //rb.DORotate(goalPosition, 0.5f);

            //newPos = transform.position + goalPosition * (speed*2) * Time.deltaTime;
            //rb.MovePosition(newPos);
            //transform.rotation.SetLookRotation(newPos);
            //transform.rotation = Quaternion.Euler(newPos);
            //movementVector = (goalPosition - transform.position).normalized * (speed * 6);

            //_deflect = true;
            ChangeDirection(speed * 30);
            //_deflect = true;

            //transform.rotation = Quaternion.Euler(0, 0, 180);

            //transform.Translate(goalPosition.x, goalPosition.y, goalPosition.z, speed*2);
            //rb.velocity = new Vector3 (-speed *2, 0, 0);
            //GetComponent<AudioSource>().Play();
            //_deflectSound.Play();
        }
        if (other.tag == "Reset")
        {
            //PoseDetection.RemoveFromList(gameObject);
            //Destroy(gameObject);
        }
        /* if (other.tag == "Shield")
         {
             ChangeDirection(speed * 6);
         }*/
        //Physics.IgnoreLayerCollision(4, 7);
        if (other.tag == "Robot")
        {
            _deflectSound.Play();
            gameObject.SetActive(false);
            //Destroy(transform.parent.gameObject);
        }
        //gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        //PoseDetection.RemoveFromList(gameObject);
    }
}
