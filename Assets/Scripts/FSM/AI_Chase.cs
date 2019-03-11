﻿using UnityEngine;
using FiniteStateMachine;

public class AI_Chase : State<AI>
{
    private static AI_Chase _instance;
    private static string _name = "chase";
    private AI_Chase()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Chase instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Chase();     //create one
            return _instance;
        }
    }

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Chase State");
        _owner.animator.Play("Run");      //play animation when entering state
        
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Chase State");
        _owner.seek.LastKnownTarget.GetComponent<Critter>().IsAlarmed = false;
        _owner.agent.SetDestination(_owner.transform.position);
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy()) { _owner.stateMachine.ChangeState(AI_Evade.instance); }
        else if (_owner.CanSeeTarget())
        {
            if (_owner.IsCloseEnough())
            {
                if (_owner.TargetIsFood()) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
                else if (_owner.TargetIsMate()) { _owner.stateMachine.ChangeState(AI_Breed.instance); }
            }
            else
            {
                _owner.seek.Target.GetComponent<Critter>().IsAlarmed = true;
                _owner.agent.SetDestination(_owner.seek.Target.transform.position);
            }
        }
        else { _owner.stateMachine.ChangeState(AI_Idle.instance); }     
    }

}