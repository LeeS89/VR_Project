using System.Collections;
using UnityEngine;

/// <summary>
/// NEED TO OPTIMIZE FOR BETTER READABILITY
/// </summary>
public class BulletSpawn : MonoBehaviour
{
    [Header("Size of the spawner area")]
    public Vector3 spawnerSize;

    [Header("Rate of spawn")]
    public float spawnRate = 1f;
    private int _index = 0;

    [Header("Model to spawn")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] GameObject _bulletSpawnPoint;
    [SerializeField] EnemyHealthController _healthController;

    public float _spawnTimer = 0f;

    private Vector3 goalPosition;
    private Vector3 _shootPosition;
    private GameObject _playerCollider;
    private PlayerCollider _playerDefence;

    //[SerializeField] GameObject _lRObject;
    [SerializeField] private LineRenderer _lr;
    [SerializeField] Material[] _materials;

    private Vector3[] _points;

    private bool _drawRay = false;
    public bool _isShooting = false;

    public GameObject _playerLineRenderer;

    private Coroutine _coroutine;
    private BulletSpawn _bulletSpawn;


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
        _spawnTimer += Time.deltaTime;
        if (_isShooting)
        {
            //_spawnTimer += Time.deltaTime;

            if (_spawnTimer > spawnRate)
            {
                //Debug.Log("Spawning asteroid");
                SpawnBulletDelay();
                _spawnTimer = 0f;

            }
        }


            if (_drawRay)
            {
                //_shootPosition = _playerLineRenderer.transform.position;
                _points = new Vector3[] { _bulletSpawnPoint.transform.position, _shootPosition };
                if (_points != null)
                {
                    
                    _lr.SetPositions(_points);

                }

            }
        
       /* else if (!_drawRay && !_isShooting)
        {
            StopCoroutine(SpawnBullet());
        }*/
        /*else
        {
            SpawnBullet().Reset();
        }*/


    }

    public void CancelShoot()
    {
        if (_isShooting)
        {
            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = null;
            
            //_spawnTimer = 0.0f;
            StopRay();
            _isShooting = false;
        }
        else
        {
            return;
        }
    }

   
    private void DrawRay()
    {
        //_lRObject.SetActive(true);
        _lr.enabled = true;
        _drawRay = true;
    }

    private void StopRay()
    {
        _lr.enabled = false;
        _drawRay = false;
    }

    private void SpawnBulletDelay()
    {
        int _currentHealth = _healthController.GetHealth();

        if (_currentHealth <= 0)
        {
            CancelShoot();
        }

            _shootPosition = _playerCollider.transform.position + new Vector3(UnityEngine.Random.Range(-_playerDefence.spawnerSize.x / 2, _playerDefence.spawnerSize.x / 2),
                                                                 UnityEngine.Random.Range(-_playerDefence.spawnerSize.y / 2, _playerDefence.spawnerSize.y / 2),
                                                                 UnityEngine.Random.Range(-_playerDefence.spawnerSize.z / 2, _playerDefence.spawnerSize.z / 2));
        //_playerLineRenderer.transform.position = _shootPosition;


        DrawRay();



        _coroutine = StartCoroutine(SpawnBullet());


    }

    IEnumerator SpawnBullet()
    {
        //_points = null;

        yield return new WaitForSeconds(2.0f);


        /*Array.Clear(_points, 0, _points.Length);*/
        _drawRay = false;
        //_lRObject.SetActive(false);
        _lr.enabled = false;
        GameObject bullet = BulletPool.SharedInstance.GetPooledObject();
        if (bullet != null)
        {

            //BulletFsmController bulletController = bullet.GetComponent<BulletFsmController>();
            BulletManager _activeState = bullet.GetComponent<BulletManager>();

            //bulletController.SetBulletStartPoint(this.gameObject);
            //bulletController.SetShootPosition(_shootPosition);
            //bullet.GetComponentInChildren<BulletController>().SetShootPosition(_shootPosition);


            bullet.transform.position = _bulletSpawnPoint.transform.position;
            bullet.transform.rotation = _bulletSpawnPoint.transform.rotation;
            _activeState.InitializeBullet(_shootPosition, gameObject);

            //bulletController.ResetValues();
            /*bullet.tag = "IgnoreCollision";
            StartCoroutine(ResetBulletTag(bullet));*/
            bullet.SetActive(true);

            //bulletController.ChangeState(bulletController._activeState);

            //bulletController.NewState();
            //bulletController.ResetValues();

            //bullet.SetActive(true);
        }




        /*GameObject bullets;

        bullets = Instantiate(_bullet, _bulletSpawnPoint.transform.position, _bulletSpawnPoint.transform.rotation);
        ActiveState bulletController = bullets.GetComponent<ActiveState>();
        bulletController.InitializeBullet(_shootPosition, gameObject);*/

        //bullets.GetComponentInChildren<BulletController>().SetBulletStartPoint(this.gameObject);
        //bullets.GetComponentInChildren<BulletController>().SetShootPosition(_shootPosition);

        //asteroid.transform.SetParent(this.transform);

    }

   /* private IEnumerator ResetBulletTag(GameObject bullet)
    {
        yield return new WaitForSeconds(0.2f);

        // Set the bullet's tag back to the "Bullet" tag
        bullet.tag = "Head";
    }*/

    private void ChangeColor()
    {
        GetComponent<MeshRenderer>().material = _materials[_index];
        _index++;

        if (_index == _materials.Length)
        {
            _index = 0;
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Head")
        {
            ChangeColor();
            Destroy(other.gameObject);
        }
    }*/
}
