using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_PlayDead : State<AI>
{
    private static AI_PlayDead _instance;
    private AI_PlayDead()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_PlayDead instance
    {
        get
        {
            //if there is no instance
            if (_instance == null)
                new AI_PlayDead();      //create one
            return _instance;
        }
    }

    public override float GetWeight(AI _owner) { return _owner.critter.critterTraitsDict[Trait.Acting]; }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering PlayDead State");
        _owner.animator.Play("Dead");       //start playing animation when entering state
        _owner.critter.isAlarmed = true;
        _owner.critter.isVisible = Random.Range(0, 10) < _owner.critter.critterTraitsDict[Trait.Acting];
        _owner.StartCoroutine(WaitForAnimation(_owner));
    }


    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting PlayDead State");
        _owner.animator.Play("Dead");
        _owner.critter.isVisible = true;
        _owner.critter.IsAlarmed = false;
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.IsAttacked() && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
    }

    IEnumerator WaitForAnimation(AI _owner)
    {
        yield return new WaitForSeconds(3f);
        if (_owner.CanSeeTarget() && _owner.critter.availableBehaviours.Contains(AI_Chase.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        if (_owner.critter.availableBehaviours.Contains(AI_Wander.instance)) { _owner.stateMachine.ChangeState(AI_Wander.instance); }
        if (_owner.critter.availableBehaviours.Contains(AI_Idle.instance)) { _owner.stateMachine.ChangeState(AI_Idle.instance); }
    }
}
