using UnityEngine;
using FiniteStateMachine;

public class AI_Wander : State<AI>
{
    Vector3 waypoint;   //target position, randomly generated
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
        genWaypoint(_owner);
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Wander State");
    }

    public override void UpdateState(AI _owner)
    {
        //if the target is in the AI's sight
        if (Vector3.Distance(_owner.transform.position, _owner.traits.target.transform.position) < _owner.traits.sight)
        {
            _owner.stateMachine.ChangeState(AI_Chase.instance);     //change to chase state
        }
        //otherwise, check if the bool in the AI class is false
        else if (!_owner.switchState)
        {
            _owner.stateMachine.ChangeState(AI_Idle.instance);      //and change to idle state
        }

        //calculate the direction and rotation, and start moving towards the target
        var direction = waypoint - _owner.transform.position;
        _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                                    Quaternion.LookRotation(direction),
                                    3.0f * Time.deltaTime);
        _owner.transform.Translate(0, 0, Time.deltaTime * 3.0f);
    }

    void genWaypoint(AI _owner)
    {
        float x = Random.Range(_owner.transform.position.x + 180, _owner.transform.position.x - 180);
        float y = _owner.transform.position.y;
        float z = Random.Range(_owner.transform.position.z + 180, _owner.transform.position.z - 180);
        waypoint = new Vector3(x, y, z);
    }
}
