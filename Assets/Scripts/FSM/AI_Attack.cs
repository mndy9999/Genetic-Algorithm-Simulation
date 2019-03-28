using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Attack : State<AI>
{
    private static AI_Attack _instance;

    private AI_Attack()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get the instance of the state
    public static AI_Attack instance
    {
        get
        {
            //if there is no instance
            if (_instance == null)
                new AI_Attack();      //create one
            return _instance;
        }
    }

    private float weight = 1;
    public override float GetWeight(AI _owner) { return weight; }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Attack State");        
        if(_owner.seek.Target) _owner.seek.Target.GetComponent<Critter>().IsAttacked = true;
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Attack State");
        if (_owner.seek.LastKnownTarget)
        {
            _owner.seek.LastKnownTarget.GetComponent<Critter>().IsAlarmed = false;
            _owner.seek.LastKnownTarget.GetComponent<Critter>().IsAttacked = false;
            _owner.critter.IsAttacked = false;
        }
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }                
        if (_owner.CanSeeTarget()) {
            if (!_owner.IsCloseEnough() && _owner.critter.availableBehaviours.Contains(AI_Chase.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
            if (_owner.TargetIsDead() && _owner.critter.availableBehaviours.Contains(AI_Eat.instance)) { _owner.stateMachine.ChangeState(AI_Eat.instance); }
            Attack(_owner);
        }
        else if (_owner.critter.availableBehaviours.Contains(AI_Wander.instance)){ _owner.stateMachine.ChangeState(AI_Wander.instance); }
        else if (_owner.critter.availableBehaviours.Contains(AI_Idle.instance)){ _owner.stateMachine.ChangeState(AI_Idle.instance); }
        

    }

    void Attack(AI _owner)
    {        
        var direction = _owner.seek.Target.transform.position - _owner.transform.position;
        if (direction != Vector3.zero)
        {
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                                        Quaternion.LookRotation(direction),
                                        _owner.agent.speed * Time.deltaTime);
        }
        //start playing the animation
        _owner.animator.Play("Attack");
        _owner.seek.Target.GetComponent<Critter>().Health -= _owner.critter.critterTraitsDict[Trait.AttackPoints]/100;                
    }

}