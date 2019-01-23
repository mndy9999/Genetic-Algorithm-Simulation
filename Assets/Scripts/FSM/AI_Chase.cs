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
    public static AI_Chase instance
    {
        get
        {
            if (_instance == null)
                new AI_Chase();
            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Chase State");
        _owner.animator.Play("Chase");
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Chase State");
    }

    public override void UpdateState(AI _owner)
    {
        var direction = _owner.traits.target.transform.position - _owner.transform.position;
        _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                                    Quaternion.LookRotation(direction),
                                    3.0f * Time.deltaTime);
        _owner.transform.Translate(0, 0, Time.deltaTime * 3.0f);


        if (Vector3.Distance(_owner.transform.position, _owner.traits.target.transform.position) < 0.3f)
        {
            _owner.stateMachine.ChangeState(AI_Eat.instance);
        }
        else if (Vector3.Distance(_owner.transform.position, _owner.traits.target.transform.position) > _owner.traits.sight)
        {
            _owner.stateMachine.ChangeState(AI_Idle.instance);
        }
       
    }
}