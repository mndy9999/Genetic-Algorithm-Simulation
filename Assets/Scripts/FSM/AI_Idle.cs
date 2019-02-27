using UnityEngine;
using FiniteStateMachine;

public class AI_Idle : State<AI>
{
    private static AI_Idle _instance;
    public static string _name = "idle";
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

    public static string name
    {
        get { return _name; }
        set { _name = value; }
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
        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.name)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy()) { _owner.stateMachine.ChangeState(AI_Evade.instance); }
        else if (_owner.CanSeeTarget()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        else if (_owner.switchState) { _owner.stateMachine.ChangeState(AI_Wander.instance); }
    }
}
