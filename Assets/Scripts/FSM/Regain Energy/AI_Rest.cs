using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Rest : State<AI>
{
    private static AI_Rest _instance;
    private static string _name = "rest";
    private AI_Rest()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Rest instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Rest();     //create one
            return _instance;
        }
    }

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    public override float GetWeight(AI _owner) { return _owner.critter.Energy; }

    public override void EnterState(AI _owner)
    {
        //Debug.Log("Entering Rest State");
        _owner.animator.Play("Sleep");      //play animation when entering state        
    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting Rest State");
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked || _owner.CanSeeEnemy() || _owner.critter.Energy > 90) { _owner.stateMachine.ChangeState(AI_Getup.instance); }
        _owner.critter.Energy += 0.1f;
    }

    IEnumerator WaitForAnimation(AI _owner)
    {
        yield return new WaitForSeconds(1);
    }
}
