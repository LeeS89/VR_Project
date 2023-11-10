using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathController : MonoBehaviour
{
    public static EnemyDeathController Instance { get; private set; }

    private List<GameObject> _enemies = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddtoDisabledList(GameObject o)
    {
        _enemies.Add(o);

        StartCoroutine(ReviveEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ReviveEnemies()
    {
        yield return new WaitForSeconds(5.0f);

        foreach(GameObject o in _enemies)
        {
            o.SetActive(true);
        }
    }
}
