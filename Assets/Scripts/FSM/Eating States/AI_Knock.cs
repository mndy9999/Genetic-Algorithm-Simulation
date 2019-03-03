using UnityEngine;
using FiniteStateMachine;

public class AI_Knock : State<AI>
{
    private static AI_Knock _instance;
    private static string _name = "knock";
    private AI_Knock()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get the instance of the state
    public static AI_Knock instance
    {
        get
        {
            //if there is no instance, create one
            if (_instance == null)
                new AI_Knock();
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
        Debug.Log("Entering Knock State");
            //start playing animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Knock State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy()) { _owner.stateMachine.ChangeState(AI_Evade.instance); }
        else if (_owner.CanSeeTarget() && _owner.IsCloseEnoughToEat()) { Knock(_owner); }
        else { _owner.stateMachine.ChangeState(AI_Idle.instance); }
    }

    void Knock(AI _owner)
    {
        float attackPower = 0.05f;
        var direction = _owner.seek.Target.transform.position - _owner.transform.position;
        _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                                    Quaternion.LookRotation(direction),
                                    _owner.critter.speed * Time.deltaTime);

        _owner.animator.Play("Attack");
        _owner.seek.Target.GetComponent<Critter>().health -= attackPower;
    }


}