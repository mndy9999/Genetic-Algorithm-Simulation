using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Watch : State<AI>
{
    State<AI> bestState;
    private static AI_Watch _instance;
    private AI_Watch()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Watch instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Watch();     //create one
            return _instance;
        }
    }

    public override float GetWeight(AI _owner) { return _owner.critter.FitnessScore; }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Watch State");
        _owner.animator.Play("Idle");      //play animation when entering state   
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Watch State");
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {
        GenerateNewDirection(_owner);
        _owner.StartCoroutine(WaitForAnimation(_owner));
    }

    IEnumerator WaitForAnimation(AI _owner)
    {       
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.IsAttacked()) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeTarget() && _owner.TargetIsEnemy()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }

        yield return new WaitForSeconds(3);
        if (_owner.CanSeeTarget()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        else { _owner.stateMachine.ChangeState(AI_Wander.instance); }
    }

    void GenerateNewDirection(AI _owner)
    {
        Vector3 direction = _owner.seek.LastKnownOpponent.transform.position - _owner.transform.position;
        _owner.transform.Rotate(direction);
    }
}
