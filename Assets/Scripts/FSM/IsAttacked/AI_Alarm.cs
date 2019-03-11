using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Alarm : State<AI>
{
    private static AI_Alarm _instance;
    private static string _name = "alarm";
    private AI_Alarm()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Alarm instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Alarm();     //create one
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
        Debug.Log("Entering Alarm State");
        _owner.animator.Play("Bee");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Alarm State");
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else { _owner.StartCoroutine(Alarm(_owner)); }
       
    }

    IEnumerator Alarm(AI _owner)
    {
        yield return new WaitForSeconds(1);
        foreach (GameObject c in _owner.seek.visibleTargets)
        {
            c.GetComponent<Critter>().IsAlarmed = true;
        }
        _owner.critter.CanAlarm = false;
        yield return new WaitForSeconds(2);
        _owner.stateMachine.ChangeState(AI_Evade.instance);
    }
}