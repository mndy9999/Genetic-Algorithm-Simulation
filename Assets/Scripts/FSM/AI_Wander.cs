using UnityEngine;
using FiniteStateMachine;

public class AI_Wander : State<AI>
{
    private static AI_Wander _instance;
    private AI_Wander()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get the instance of the state
    public static AI_Wander instance
    {
        get
        {
            //if there is no instance
            if (_instance == null)
                new AI_Wander();      //create one
            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Wander State");
        _owner.animator.Play("Chase");  //start playing the animation when entering state
        _owner.genWaypoint();
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Wander State");
    }

    public override void UpdateState(AI _owner)
    {

            //if the target is in the AI's sight
        if (_owner.CanSeeTarget())
        {
            _owner.stateMachine.ChangeState(AI_Chase.instance);     //change to chase state
        }
        
        
        //otherwise, check if the bool in the AI class is false
        else if (!_owner.switchState)
        {
            _owner.stateMachine.ChangeState(AI_Idle.instance);      //and change to idle state
        }

        //calculate the direction and rotation, and start moving towards the target
        var direction = _owner.waypoint - _owner.transform.position;
        _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                                    Quaternion.LookRotation(direction),
                                    3.0f * Time.deltaTime);
        _owner.transform.Translate(0, 0, Time.deltaTime * 3.0f);
    }


}
