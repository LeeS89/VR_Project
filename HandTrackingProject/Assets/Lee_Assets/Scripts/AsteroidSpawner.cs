using System;
using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Size of the spawner area")]
    public Vector3 spawnerSize;

    [Header("Rate of spawn")]
    public float spawnRate = 1f;
    private int _index = 0;

    [Header("Model to spawn")]
    [SerializeField] private GameObject _asteroid;
    [SerializeField] GameObject _bulletSpawn;

    private float _spawnTimer = 0f;

    private Vector3 goalPosition;
    private Vector3 _shootPosition;
    private GameObject _playerCollider;
    private PlayerCollider _playerDefence;
    [SerializeField] private LineRenderer _lr;
    [SerializeField] Material[] _materials;

    private Vector3[] _points;

    private bool _drawRay = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position, spawnerSize);
    }

    private void Awake()
    {
        _playerCollider = GameObject.FindGameObjectWithTag("DefenceTarget");
        _playerDefence = _playerCollider.GetComponent<PlayerCollider>();
        //_lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer > spawnRate)
        {
            //Debug.Log("Spawning asteroid");
            _spawnTimer = 0f;
            SpawnBulletDelay();
        }


        if (_drawRay)
        {
            //if (!_lr.enabled)
            //{
                //_lr.enabled = true;
                _points = new Vector3[] { _bulletSpawn.transform.position, _shootPosition };
                if (_points != null)
                {

                    _lr.SetPositions(_points);

                }
            //}
        }

        //transform.LookAt(_playerCollider.transform.position + _shootPosition);
        /*_bulletSpawn.transform.rotation = Quaternion.LookRotation((_playerCollider.transform.position + _shootPosition) - transform.position);
        transform.rotation = _bulletSpawn.transform.rotation;*/
        /*else if(!_drawRay)
        {
            if(_points != null)
            {
                Array.Clear(_points, 0, _points.Length);
                _lr.enabled = false;
            }
        }*/

    }

   /* public void RotateTowardsPlayer()
    {
        transform.rotation = Quaternion.LookRotation((_playerCollider.transform.position + _shootPosition) - transform.position);
    }
*/
    private void DrawRay()
    {
        /*_shootPosition = _playerCollider.transform.position + new Vector3(UnityEngine.Random.Range(-_playerDefence.spawnerSize.x / 2, _playerDefence.spawnerSize.x / 2),
                                                             UnityEngine.Random.Range(-_playerDefence.spawnerSize.y / 2, _playerDefence.spawnerSize.y / 2),
                                                             UnityEngine.Random.Range(-_playerDefence.spawnerSize.z / 2, _playerDefence.spawnerSize.z / 2));*/
        //_points = new Vector3[] { _bulletSpawn.transform.position, _shootPosition };
        _lr.enabled = true;
        _drawRay = true;
    }

    private void SpawnBulletDelay()
    {
        //Get a random position for the asteroid
        /*Vector3 spawnPoint = transform.position + new Vector3(UnityEngine.Random.Range(-spawnerSize.x / 2, spawnerSize.x / 2),
                                                              UnityEngine.Random.Range(-spawnerSize.y / 2, spawnerSize.y / 2),
                                                              UnityEngine.Random.Range(-spawnerSize.z / 2, spawnerSize.z / 2));*/

        _shootPosition = _playerCollider.transform.position + new Vector3(UnityEngine.Random.Range(-_playerDefence.spawnerSize.x / 2, _playerDefence.spawnerSize.x / 2),
                                                             UnityEngine.Random.Range(-_playerDefence.spawnerSize.y / 2, _playerDefence.spawnerSize.y / 2),
                                                             UnityEngine.Random.Range(-_playerDefence.spawnerSize.z / 2, _playerDefence.spawnerSize.z / 2));
        DrawRay();

        /*_points = new Vector3[] { _bulletSpawn.transform.position, _shootPosition };*/
        //_drawRay = true;

        StartCoroutine(SpawnBullet());

        /*goalPosition = transform.position + new Vector3(UnityEngine.Random.Range(-spawnerSize.x / 2, spawnerSize.x / 2),
                                                              UnityEngine.Random.Range(-spawnerSize.y / 2, spawnerSize.y / 2),
                                                              UnityEngine.Random.Range(-spawnerSize.z / 2, spawnerSize.z / 2));*/
        /*GameObject asteroid;
        
        asteroid = Instantiate(_asteroid, spawnPoint, transform.rotation);
        asteroid.GetComponentInChildren<MoveCube>().SetShootPosition(_shootPosition);

        asteroid.transform.SetParent(this.transform);*/
    }

    IEnumerator SpawnBullet()
    {
        //_points = null;
        
        yield return new WaitForSeconds(2.0f);

        
        /*Array.Clear(_points, 0, _points.Length);*/
        _drawRay = false;
        _lr.enabled = false;
        GameObject asteroid;

        asteroid = Instantiate(_asteroid, _bulletSpawn.transform.position, _bulletSpawn.transform.rotation);
        asteroid.GetComponentInChildren<MoveCube>().SetShootPosition(_shootPosition);

        //asteroid.transform.SetParent(this.transform);

    }

    private void ChangeColor()
    {
        GetComponent<MeshRenderer>().material = _materials[_index];
        _index++;

        if(_index == _materials.Length)
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
