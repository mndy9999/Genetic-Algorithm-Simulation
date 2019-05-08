using UnityEngine;
using FiniteStateMachine;

public class AI_Idle : State<AI>
{
    State<AI> bestState;
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

    private float weight = 1;
    public override float GetWeight(AI _owner) { return weight; }

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    public override void EnterState(AI _owner)
    {
        //Debug.Log("Entering Idle State");
        _owner.animator.Play("Idle");       //start playing animation when entering state
        _owner.seconds = 0;
    }


    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting Idle State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.IsAttacked() && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        if (_owner.CanSeeTarget() && _owner.critter.availableBehaviours.Contains(AI_Chase.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        if (_owner.switchState && _owner.critter.availableBehaviours.Contains(AI_Wander.instance)) { _owner.stateMachine.ChangeState(AI_Wander.instance); }
    }
}
