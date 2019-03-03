using UnityEngine;
using FiniteStateMachine;

public class AI_Chase : State<AI>
{
    private static AI_Chase _instance;
    private static string _name = "chase";
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

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Chase State");
        _owner.animator.Play("Run");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Chase State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy()) { _owner.stateMachine.ChangeState(AI_Evade.instance); }
        else if (_owner.CanSeeTarget())
        {
            if (_owner.IsCloseEnoughToEat())
            {
                if (_owner.TargetIsFood()) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
                else if (_owner.TargetIsMate()) { _owner.stateMachine.ChangeState(AI_Breed.instance); }
            }
            else { Chase(_owner); }
        }
        else { _owner.stateMachine.ChangeState(AI_Idle.instance); }     
    }

    void Chase(AI _owner)
    {
        _owner.seek.Target.GetComponent<Critter>().IsChased = true;

        var direction = _owner.seek.Target.transform.position - _owner.transform.position;
        _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                                    Quaternion.LookRotation(direction),
                                    _owner.critter.speed * Time.deltaTime);
        _owner.transform.Translate(0, 0, Time.deltaTime * _owner.critter.speed);
    }

}