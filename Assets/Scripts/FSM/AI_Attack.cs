using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Attack : State<AI>
{
    private static AI_Attack _instance;
    private AI_Attack()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get the instance of the state
    public static AI_Attack instance
    {
        get
        {
            //if there is no instance
            if (_instance == null)
                new AI_Attack();      //create one
            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Attack State");
        
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Attack State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        //if the enemy is in the AI's sight
        if (!_owner.CanSeeTarget())
        {
            _owner.stateMachine.ChangeState(AI_Idle.instance);     //change to idle state
        }
        if (_owner.TargetIsDead())
        {
            _owner.stateMachine.ChangeState(AI_Eat.instance);
        }
        if(_owner.critter.health < 40 && _owner.CanSeeEnemy())
        {
            _owner.stateMachine.ChangeState(AI_Evade.instance);     //change to evade state
        }
        if (!_owner.TargetIsDead())
        {
            float attackPower = 0.05f;
            //start playing the animation when entering state
            _owner.animator.Play("Attack");
            _owner.seek.Target.GetComponent<Critter>().IsAttacked = true;
            _owner.seek.Target.GetComponent<Critter>().health -= attackPower;
            
        }

    }

}