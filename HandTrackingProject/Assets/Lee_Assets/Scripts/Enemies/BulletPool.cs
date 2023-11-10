using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    private Dictionary<GameObject, bool> bulletUsageDict;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        bulletUsageDict = new Dictionary<GameObject, bool>();

        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
            bulletUsageDict.Add(tmp, false);
            tmp.transform.parent = this.gameObject.transform;
            tmp.transform.position = this.transform.position;
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!bulletUsageDict[pooledObjects[i]])
            {
                //GameObject o = pooledObjects[i];
                //RemoveFromPool(o);
                bulletUsageDict[pooledObjects[i]] = true;
                return pooledObjects[i];
                //return o;

            }
        }
        return null;
    }

    public void ReturnToPool(GameObject bullet)
    {
        if (bulletUsageDict.ContainsKey(bullet))
        {
            bulletUsageDict[bullet] = false; // Mark the bullet as not in use when returning it to the pool
            bullet.SetActive(false);
            bullet.transform.position = this.transform.position;

        }
    }

    private void RemoveFromPool(GameObject o)
    {
        pooledObjects.Remove(o);
    }

    public void AddToPool(GameObject o)
    {
        pooledObjects.Add(o);
    }
}
