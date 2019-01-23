using UnityEngine;
using FiniteStateMachine;

public class AI_Eat : State<AI>
{
    private static AI_Eat _instance;
    private AI_Eat()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    public static AI_Eat instance
    {
        get
        {
            if (_instance == null)
                new AI_Eat();
            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Eat State");
        _owner.animator.Play("Eat");
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Eat State");
    }

    public override void UpdateState(AI _owner)
    {
        if (Vector3.Distance(_owner.transform.position, _owner.traits.target.transform.position) > 0.3f)
        {
            _owner.stateMachine.ChangeState(AI_Chase.instance);
        }
        _owner.traits.target.GetComponent<food>().level-=0.1f;
    }
}