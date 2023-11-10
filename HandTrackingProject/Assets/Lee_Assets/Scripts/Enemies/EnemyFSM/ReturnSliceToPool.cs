using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnSliceToPool : MonoBehaviour
{
    private void OnDisable()
    {
        EnemySlicePool.SharedInstance.ReturnToPool(gameObject);
    }
}
