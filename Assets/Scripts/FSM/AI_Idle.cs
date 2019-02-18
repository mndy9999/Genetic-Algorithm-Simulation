using UnityEngine;
using FiniteStateMachine;

public class AI_Idle : State<AI>
{
    private static AI_Idle _instance;
    private AI_Idle()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Idle instance
    {
        get
        {
            //if there is no instance
            if (_instance == null)
                new AI_Idle();      //create one
            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Idle State");
        _owner.animator.Play("Idle");       //start playing animation when entering state
    }


    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Idle State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { Debug.Log("hi"); _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        if (_owner.CanSeeEnemy()) { _owner.stateMachine.ChangeState(AI_Evade.instance); }
        if (_owner.CanSeeTarget()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        else if (_owner.switchState) { _owner.stateMachine.ChangeState(AI_Wander.instance); }
    }
}
