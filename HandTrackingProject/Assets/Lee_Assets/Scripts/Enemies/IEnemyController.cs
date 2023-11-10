using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyController
{
    void SetHealth(int health);
    void UpdateHealth(int damageAmount);
    void TriggerDeathAnimation();
}
