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

        //if the target is in the AI's sight
        if (_owner.CanSeeTarget())
        {
            _owner.stateMachine.ChangeState(AI_Chase.instance);     //change to chase state
        }
        //otherwise, check if the bool in the AI class is true
        else if (_owner.switchState)
        {
            _owner.stateMachine.ChangeState(AI_Wander.instance);    //and change to wander state
        }
    }
}
