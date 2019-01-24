using UnityEngine;
using FiniteStateMachine;

public class AI_Chase : State<AI>
{
    private static AI_Chase _instance;
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
    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Chase State");
        _owner.animator.Play("Chase");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Chase State");
    }

    public override void UpdateState(AI _owner)
    {
        //calculate direction, rotation and start moving towards the target
        var direction = _owner.traits.target.transform.position - _owner.transform.position;
        _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                                    Quaternion.LookRotation(direction),
                                    3.0f * Time.deltaTime);
        _owner.transform.Translate(0, 0, Time.deltaTime * 3.0f);

        //if the AI is close enough to the target
        if (Vector3.Distance(_owner.transform.position, _owner.traits.target.transform.position) < 0.3f)
        {
            _owner.stateMachine.ChangeState(AI_Eat.instance);       //change to eating state
        }
        //if the target is out of the AI's sight
        else if (Vector3.Distance(_owner.transform.position, _owner.traits.target.transform.position) > _owner.traits.sight)
        {
            _owner.stateMachine.ChangeState(AI_Idle.instance);      //change to idle
        }
       
    }
}