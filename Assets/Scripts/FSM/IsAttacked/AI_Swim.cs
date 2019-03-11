using UnityEngine;
using FiniteStateMachine;

public class AI_Swim : State<AI>
{
    private static AI_Swim _instance;
    private static string _name = "swim";
    private AI_Swim()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Swim instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Swim();     //create one
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
        Debug.Log("Entering Swim State");
        _owner.animator.Play("Run");      //play animation when entering state
        _owner.agent.SetDestination(_owner.seek.water.transform.position);
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Swim State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (!_owner.CanSeeEnemy())
        {
            _owner.critter.IsAlarmed = false; _owner.stateMachine.ChangeState(AI_Wander.instance);                                                                                                         
            Swim(_owner);
        }
        else { _owner.critter.IsAlarmed = false; _owner.stateMachine.ChangeState(AI_Wander.instance); }
    }

    void Swim(AI _owner)
    {
        Debug.Log("swimming");

    }
}