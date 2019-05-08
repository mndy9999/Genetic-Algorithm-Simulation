using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Getup : State<AI>
{
    State<AI> bestState;
    private static AI_Getup _instance;
    private static string _name = "getup";
    private AI_Getup()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Getup instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Getup();     //create one
            return _instance;
        }
    }

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    private float weight = 1;
    public override float GetWeight(AI _owner) { return weight; }

    public override void EnterState(AI _owner)
    {
        //Debug.Log("Entering Getup State");
        _owner.animator.Play("Getup");      //play animation when entering state        
    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting Getup State");
    }

    public override void UpdateState(AI _owner)
    {
        _owner.StartCoroutine(WaitForAnimation(_owner));
    }

    IEnumerator WaitForAnimation(AI _owner)
    {
        yield return new WaitForSeconds(_owner.animator.GetCurrentAnimatorStateInfo(0).length + _owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        if (_owner.CanSeeEnemy())
        {
            bestState = _owner.BestState(Behaviours.EnemyEncounterBehaviours);
            if (bestState != null)
                _owner.stateMachine.ChangeState(bestState);
        }
        if (_owner.CanSeeTarget()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        if (_owner.switchState) { _owner.stateMachine.ChangeState(AI_Wander.instance); }
    }
}
