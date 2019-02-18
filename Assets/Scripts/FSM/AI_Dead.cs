﻿using UnityEngine;
using FiniteStateMachine;

public class AI_Dead : State<AI>
{

    public float eatHPPerSecond = 5f;
    public float eatHPToEnergy = 2f;

    private static AI_Dead _instance;
    private AI_Dead()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get the instance of the state
    public static AI_Dead instance
    {
        get
        {
            //if there is no instance, create one
            if (_instance == null)
                new AI_Dead();
            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Dead State");
        _owner.animator.Play("Dead");    //start playing animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Dead State");
    }

    public override void UpdateState(AI _owner)
    {
        if(_owner.critter.resource <= 0) { _owner.critter.KillSelf(); }
    }


}