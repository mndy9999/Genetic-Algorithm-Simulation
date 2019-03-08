using UnityEngine;
using FiniteStateMachine;

public class AI_Dig : State<AI>
{ 
    private static AI_Dig _instance;
    private static string _name = "dig";
    private AI_Dig()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get the instance of the state
    public static AI_Dig instance
    {
        get
        {
            //if there is no instance, create one
            if (_instance == null)
                new AI_Dig();
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
        Debug.Log("Entering Dig State");
       // _owner.animator.Play("Dig");    //start playing animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Dig State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy()) { _owner.stateMachine.ChangeState(AI_Evade.instance); }
        else if (_owner.CanSeeTarget()) {
            if(_owner.seek.Target.GetComponent<Critter>().critterType == "Dirt") { Dig(_owner); }
            else { _owner.stateMachine.ChangeState(AI_Attack.instance); }
            
        }
        else { _owner.stateMachine.ChangeState(AI_Idle.instance); }
    }

    void Dig(AI _owner)
    {
        _owner.animator.Play("Dig");
        _owner.seek.Target.GetComponent<Critter>().Health -= 0.05f;
    }


}