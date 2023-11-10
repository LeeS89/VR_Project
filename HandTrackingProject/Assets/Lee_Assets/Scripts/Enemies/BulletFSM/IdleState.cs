using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    //private BulletFsmController bulletFsmController;

    /*public override void EnterState()
    {
        *//*if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }*//*
    }*/

    public override void UpdateState()
    {
        //return;
    }

    public override void ExitState()
    {
        //bulletFsmController.NewState()
    }

    public override void LateUpdateState()
    {
        throw new System.NotImplementedException();
    }
}
