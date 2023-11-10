using System.Collections;
using UnityEngine;

/// <summary>
/// NEED TO OPTIMIZE FOR BETTER READABILITY
/// </summary>
public class EnemyShoot : MonoBehaviour
{
    [Header("Size of the spawner area")]
    public Vector3 spawnerSize;

    [Header("Rate of spawn")]
    public float spawnRate = 1f;
    //private int _index = 0;

    [Header("Model to spawn")]
    //[SerializeField] private GameObject _bullet;
    [SerializeField] GameObject _bulletSpawnPoint;
    [SerializeField] EnemyHealthController _healthController;

    public float _spawnTimer = 0f;

    private Vector3 goalPosition;
    private Vector3 _shootDirection;

    // Where to shoot
    private GameObject _playerCollider;
    private PlayerCollider _playerDefence;

    //[SerializeField] GameObject _lRObject;
    [SerializeField] private LineRenderer _lr;
    //[SerializeField] Material[] _materials;

    private Vector3[] _points;

    private bool _drawRay = false;
    //public bool _isShooting = false;

    public GameObject _playerLineRenderer;

    private Coroutine _coroutine;
    private EnemyShoot _bulletSpawn;

    
    [SerializeField]
    private FieldOfView _fieldOfView;
    private bool _canSeePlayer = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position, spawnerSize);
    }

    private void Awake()
    {
     
        _bulletSpawn = this;
        _playerCollider = GameObject.FindGameObjectWithTag("DefenceTarget");
        _playerDefence = _playerCollider.GetComponent<PlayerCollider>();
        //_lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdatePlayerInView();
    
        _spawnTimer += Time.deltaTime;
        if (_canSeePlayer)
        {
            PrepareToShoot();

            DrawAimingRay();
        }else if(!_canSeePlayer && _drawRay)
        {
            CancelShoot();
        }


    }


    /// <summary>
    /// Once the player is in view, the enemy aims at the player and this sets _drawRay to true
    /// which enables the line renderer and indicates to the player, where the shot is going to come from
    /// so they can act accordingly
    /// </summary>
    private void DrawAimingRay()
    {
        if (_drawRay)
        {
            //_shootPosition = _playerLineRenderer.transform.position;
            _points = new Vector3[] { _bulletSpawnPoint.transform.position, _shootDirection };
            if (_points != null)
            {

                _lr.SetPositions(_points);

            }

        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// Controls the rate of shooting
    /// When _spawnTimer > spawnRate then this sets up the bullet for firing
    /// </summary>
    private void PrepareToShoot()
    {
       
        if (_spawnTimer > spawnRate)
        {

            SpawnBulletDelay();
            _spawnTimer = 0f;

        }
       
    }


    /// <summary>
    /// Exits the shooting phase if enemy can no longer see the player or
    /// if the enemy is killed while bullet is being initialized
    /// </summary>
    public void CancelShoot()
    {
        //if (_canSeePlayer)
        //{
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = null;

        //_spawnTimer = 0.0f;
        StopRay();
        //_isShooting = false;
        //}
        /* else
         {
             return;
         }*/
    }

    /// <summary>
    /// Enables line renderer to incdicate direction of shot to player
    /// </summary>
    private void EnableRay()
    {
        //_lRObject.SetActive(true);
        _lr.enabled = true;
        _drawRay = true;
    }

    /// <summary>
    /// Disables the line renderer
    /// </summary>
    private void StopRay()
    {
        _lr.enabled = false;
        _drawRay = false;
    }

    /// <summary>
    /// First checks if the enemy is still "Alive" and can sstill see the player
    /// if both are true then area around the player in which the bullet will be shot towards is set up
    /// Line renderer is enabled and then The countdown to shooting starts
    /// 
    /// Bullets destination is a random point from within a collider attached to the player
    /// </summary>
    private void SpawnBulletDelay()
    {
        int _currentHealth = _healthController.GetHealth();

        if (_currentHealth <= 0 || !_canSeePlayer)
        {
            CancelShoot();
        }

        _shootDirection = _playerCollider.transform.position + new Vector3(UnityEngine.Random.Range(-_playerDefence.spawnerSize.x / 2, _playerDefence.spawnerSize.x / 2),
                                                             UnityEngine.Random.Range(-_playerDefence.spawnerSize.y / 2, _playerDefence.spawnerSize.y / 2),
                                                             UnityEngine.Random.Range(-_playerDefence.spawnerSize.z / 2, _playerDefence.spawnerSize.z / 2));
        //_playerLineRenderer.transform.position = _shootPosition;


        EnableRay();



        _coroutine = StartCoroutine(SpawnBullet());


    }


    /// <summary>
    /// Line renderer is disabled and the bullet is taken from the pool and fired at the player
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnBullet()
    {
        //_points = null;

        yield return new WaitForSeconds(2.0f);

        _drawRay = false;
        //_lRObject.SetActive(false);
        _lr.enabled = false;
        GameObject bullet = BulletPool.SharedInstance.GetPooledObject();
        if (bullet != null && _canSeePlayer)
        {

            
            BulletManager _activeState = bullet.GetComponent<BulletManager>();


            bullet.transform.position = _bulletSpawnPoint.transform.position;
            bullet.transform.rotation = _bulletSpawnPoint.transform.rotation;
            _activeState.InitializeBullet(_shootDirection, gameObject);

         
            bullet.SetActive(true);

        }
        _coroutine = null;

    }

    private void UpdatePlayerInView()
    {
        _canSeePlayer = _fieldOfView.PlayerInView();
    }


   
}
