using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    ActiveState _activeState;
    [SerializeField] private int id;
    //public MeshCollider _collider;
    EnemySliceController _enemySliceController;

    private void Awake()
    {
        _enemySliceController= transform.root.GetComponent<EnemySliceController>();
        id = GetComponent<EnemyHealthController>().id;
        //_collider = GetComponent<MeshCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Head"))
        {

            EnemyDamageEvent.instance.DamageTriggerEnter(id, 4);

            _collider.enabled = false;
            BulletPool.SharedInstance.ReturnToPool(other.gameObject);
            Destroy(transform.root.gameObject, 3.0f);

        }*/

        if (other.CompareTag("Bullet"))
        {

            EnemyDamageEvent.instance.DamageTriggerEnter(id, 4);

            //_collider.enabled = false;
            BulletPool.SharedInstance.ReturnToPool(other.gameObject);
            //Destroy(transform.root.gameObject, 3.0f);

        }

        if (other.CompareTag("Blade"))
        {
            _enemySliceController.InstantDeath();
        }
    }

    public void KillEnemy()
    {
        EnemyDamageEvent.instance.DamageTriggerEnter(id, 4);
    }
}
