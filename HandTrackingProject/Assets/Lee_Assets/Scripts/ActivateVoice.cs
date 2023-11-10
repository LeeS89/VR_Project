using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ActivateVoice : MonoBehaviour
{

    [SerializeField] private Wit wit;

    // Update is called once per frame
    void Update()
    {
        bool _voiceListen = KillEnemyTest.instance._canKill;

        if(wit == null)
        {
            wit = GetComponent<Wit>();
        }

        if (_voiceListen)
        {
            wit.Activate();
        }
    }
}
