using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageEvent : MonoBehaviour
{
    public static EnemyDamageEvent instance;

    private void Awake()
    {
        instance = this;
    }

    public event Action<int,int> onDamageTriggerEnter;

    public void DamageTriggerEnter(int id,int damage)
    {
        if(onDamageTriggerEnter != null)
        {
            onDamageTriggerEnter(id,damage);
        }
    }

    public event Action<int> onDeathEnter;

    public void DeathEnter(int id)
    {
        if(onDeathEnter != null)
        {
            onDeathEnter(id);
        }
    }

    /* private void Update()
     {
         if(onDamageTriggerEnter != null)
         {
             Debug.LogWarning("Event is Subscribed");
         }
     }*/
}
