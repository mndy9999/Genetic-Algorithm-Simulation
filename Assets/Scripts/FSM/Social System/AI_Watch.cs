using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Watch : State<AI>
{
    private static AI_Watch _instance;
    private static string _name = "watch";
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

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Watch State");
        _owner.animator.Play("Idle");      //play animation when entering state   

    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Watch State");
        _owner.critter.isChallenged = false;
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {

        _owner.StartCoroutine(WaitForAnimation(_owner));
    }

    IEnumerator WaitForAnimation(AI _owner)
    {

        yield return new WaitForSeconds(5);
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy()) { _owner.stateMachine.ChangeState(AI_Evade.instance); }
        else if (_owner.CanSeeTarget()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        else { _owner.stateMachine.ChangeState(AI_Wander.instance); }
    }
}
