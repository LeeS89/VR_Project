using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlicePool : MonoBehaviour
{
    public static EnemySlicePool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    private Dictionary<GameObject, bool> sliceUsageDict;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        sliceUsageDict = new Dictionary<GameObject, bool>();

        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
            sliceUsageDict.Add(tmp, false);
            tmp.transform.parent = this.gameObject.transform;
            tmp.transform.position = this.transform.position;
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!sliceUsageDict[pooledObjects[i]])
            {
                //GameObject o = pooledObjects[i];
                //RemoveFromPool(o);
                sliceUsageDict[pooledObjects[i]] = true;
                return pooledObjects[i];
                //return o;

            }
        }
        return null;
    }

    public void ReturnToPool(GameObject bullet)
    {
        if (sliceUsageDict.ContainsKey(bullet))
        {
            sliceUsageDict[bullet] = false; // Mark the bullet as not in use when returning it to the pool
            
            bullet.SetActive(false);
            bullet.transform.position = this.transform.position;

        }
    }

   
}
