using UnityEngine;
using FiniteStateMachine;

public class AI_Wander : State<AI>
{
    Vector3 waypoint;
    private static AI_Wander _instance;
    private AI_Wander()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    public static AI_Wander instance
    {
        get
        {
            if (_instance == null)
                new AI_Wander();
            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Wander State");
        _owner.animator.Play("Chase");
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Wander State");
    }

    public override void UpdateState(AI _owner)
    {
        if (Vector3.Distance(_owner.transform.position, _owner.traits.target.transform.position) < _owner.traits.sight)
        {
            _owner.stateMachine.ChangeState(AI_Chase.instance);
        }
        else if (!_owner.switchState)
        {
            _owner.stateMachine.ChangeState(AI_Idle.instance);
        }
        var direction = waypoint - _owner.transform.position;
        _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                                    Quaternion.LookRotation(direction),
                                    3.0f * Time.deltaTime);
        _owner.transform.Translate(0, 0, Time.deltaTime * 3.0f);
    }

    void genWaypoint(AI _owner)
    {
        float x = Random.Range(_owner.transform.position.x + 20, _owner.transform.position.x - 20);
        float y = _owner.transform.position.y;
        float z = Random.Range(_owner.transform.position.z + 20, _owner.transform.position.z - 20);
        waypoint = new Vector3(x, y, z);
    }
}
