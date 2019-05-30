using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Laydown : State<AI>
{
    private static AI_Laydown _instance;
    private static string _name = "laydown";
    private AI_Laydown()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Laydown instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Laydown();     //create one
            return _instance;
        }
    }

    private float weight = 1;
    public override float GetWeight(AI _owner) { return weight; }

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    public override void EnterState(AI _owner)
    {
        //Debug.Log("Entering Laydown State");
        _owner.animator.Play("Laydown");      //play animation when entering state        
    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting Laydown State");
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {
        _owner.StartCoroutine(WaitForAnimation(_owner));
               
    }
    IEnumerator WaitForAnimation(AI _owner)
    {
        yield return new WaitForSeconds(_owner.animator.GetCurrentAnimatorStateInfo(0).length + _owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (_owner.critter.Energy > 30) { _owner.stateMachine.ChangeState(AI_Rest.instance); }
        if (_owner.critter.Energy < 30) { _owner.stateMachine.ChangeState(AI_Sleep.instance); }
    }
}
