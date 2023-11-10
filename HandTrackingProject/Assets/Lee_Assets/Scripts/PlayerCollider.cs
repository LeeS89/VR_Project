using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] Animator _hurt;

    [Header("Size of the spawner area")]
    public Vector3 spawnerSize;

    /*[Header("Rate of spawn")]
    public float spawnRate = 1f;

    [Header("Model to spawn")]
    [SerializeField] private GameObject _asteroid;*/

    //private float _spawnTimer = 0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position, spawnerSize);
    }

    private void Update()
    {
       /* _spawnTimer += Time.deltaTime;

        if (_spawnTimer > spawnRate)
        {
            //Debug.Log("Spawning asteroid");
            _spawnTimer = 0f;
            SpawnAsteroid();
        }*/
    }

    /* private void SpawnAsteroid()
     {
         //Get a random position for the asteroid
         Vector3 spawnPoint = transform.position + new Vector3(UnityEngine.Random.Range(-spawnerSize.x / 2, spawnerSize.x / 2),
                                                               UnityEngine.Random.Range(-spawnerSize.y / 2, spawnerSize.y / 2),
                                                               UnityEngine.Random.Range(-spawnerSize.z / 2, spawnerSize.z / 2));

         GameObject asteroid = Instantiate(_asteroid, spawnPoint, transform.rotation);

         asteroid.transform.SetParent(this.transform);
     }*/

    private void OnTriggerEnter(Collider other)
    {
        /*if(other.tag == "Head")
        {
            _hurt.SetTrigger("Hurt");
            BulletPool.SharedInstance.ReturnToPool(other.gameObject);
            //other.gameObject.SetActive(false);
        }*/
        if (other.tag == "Bullet")
        {
            _hurt.SetTrigger("Hurt");
            BulletPool.SharedInstance.ReturnToPool(other.gameObject);
            //other.gameObject.SetActive(false);
        }
    }
}
