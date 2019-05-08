using UnityEngine;
using FiniteStateMachine;

public class AI_Knock : State<AI>
{
    State<AI> bestState;
    private static AI_Knock _instance;
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

    private float weight = 1;
    public override float GetWeight(AI _owner) { return weight; }

    public override void EnterState(AI _owner)
    {
        //Debug.Log("Entering Knock State");
    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting Knock State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.IsAttacked() && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        if (_owner.CanSeeTarget() && _owner.seek.Target.GetComponent<Critter>().critterType != "Tree" && _owner.critter.availableBehaviours.Contains(AI_Chase.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        if(!_owner.CanSeeTarget() && _owner.critter.availableBehaviours.Contains(AI_Wander.instance)) { _owner.stateMachine.ChangeState(AI_Wander.instance); }
        if(!_owner.CanSeeTarget() && _owner.critter.availableBehaviours.Contains(AI_Idle.instance)) { _owner.stateMachine.ChangeState(AI_Idle.instance); }
        if(_owner.CanSeeTarget() && _owner.seek.Target.GetComponent<Critter>().critterType == "Tree") Knock(_owner);
    }

    void Knock(AI _owner)
    {
        var direction = _owner.seek.Target.transform.position - _owner.transform.position;
        _owner.transform.Rotate(direction);

        _owner.animator.Play("Attack");
        _owner.seek.Target.GetComponent<Critter>().Health -= _owner.critter.critterTraitsDict[Trait.AttackPoints];
    }


}