using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Submit : State<AI>
{
    State<AI> bestState;
    private static AI_Submit _instance;
    private AI_Submit()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Submit instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Submit();     //create one
            return _instance;
        }
    }

    public override float GetWeight(AI _owner) { return Critter.crittersDict[_owner.critter.critterType][0].FitnessScore - _owner.critter.FitnessScore; }

    public override void EnterState(AI _owner)
    {
        //Debug.Log("Entering Submit State");
        _owner.animator.Play("Dig");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting Submit State");
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {
        GenerateNewDirection(_owner);
        _owner.StartCoroutine(WaitForAnimation(_owner));
    }

    IEnumerator WaitForAnimation(AI _owner)
    {
        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.IsAttacked() && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        if (_owner.CanSeeTarget() && _owner.TargetIsEnemy() && _owner.critter.availableBehaviours.Contains(AI_Chase.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }

        yield return new WaitForSeconds(3);
        if (_owner.CanSeeTarget() && _owner.critter.availableBehaviours.Contains(AI_Chase.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        else if (_owner.critter.availableBehaviours.Contains(AI_Wander.instance)){ _owner.stateMachine.ChangeState(AI_Wander.instance); }
        else if (_owner.critter.availableBehaviours.Contains(AI_Idle.instance)){ _owner.stateMachine.ChangeState(AI_Idle.instance); }
    }

    void GenerateNewDirection(AI _owner)
    {
        Vector3 direction = _owner.seek.LastKnownOpponent.transform.position - _owner.transform.position;
        _owner.transform.Rotate(direction);
    }
}