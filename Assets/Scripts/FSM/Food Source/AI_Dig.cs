using UnityEngine;
using FiniteStateMachine;

public class AI_Dig : State<AI>
{
    State<AI> bestState;
    private static AI_Dig _instance;
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

    private float weight = 1;
    public override float GetWeight(AI _owner) { return weight; }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Dig State");
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Dig State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.IsAttacked()) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        if (_owner.CanSeeTarget() && _owner.seek.Target.GetComponent<Critter>().critterType != "Dirt") { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        if (!_owner.CanSeeTarget()) { _owner.stateMachine.ChangeState(AI_Wander.instance); }
        Dig(_owner);
    }

    void Dig(AI _owner)
    {
        _owner.animator.Play("Dig");
        _owner.seek.Target.GetComponent<Critter>().Health -= _owner.critter.critterTraitsDict[Trait.AttackPoints];
    }


}