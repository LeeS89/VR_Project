using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemyTest : MonoBehaviour
{
    public static KillEnemyTest instance;
    public GameObject[] _enemies;
    public bool _canKill = false;

    private void Awake()
    {
        instance = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        //_enemies = GameObject.FindGameObjectsWithTag("Agent");
    }

    public void DestroyEnemies()
    {
        if (_canKill)
        {
            foreach (var enemy in _enemies)
            {
                enemy.GetComponentInChildren<TriggerDamage>().KillEnemy();
            }
        }
    }

    public void SetCanKill(bool kill)
    {
        _canKill= kill;
    }
}
