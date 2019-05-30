using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Alarm : State<AI>
{
    State<AI> bestState;
    private static AI_Alarm _instance;

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

    public override float GetWeight(AI _owner) { return Vector3.Distance(_owner.transform.position, _owner.seek.Enemy.transform.position)/10 + _owner.critter.critterTraitsDict[Trait.VoiceStrenght]; }

    public override void EnterState(AI _owner)
    {
        //Debug.Log("Entering Alarm State");
        _owner.animator.Play("Bee");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting Alarm State");
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {
        _owner.StartCoroutine(WaitForAnimation(_owner));
    }


    IEnumerator WaitForAnimation(AI _owner)
    {

        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.IsAttacked() && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }

        yield return new WaitForSeconds(1);
        foreach (GameObject c in _owner.seek.visibleTargets)
        {
            c.GetComponent<Critter>().IsAlarmed = true;
        }
        _owner.critter.ResetAlarm();

        yield return new WaitForSeconds(1);
        if (_owner.critter.availableBehaviours.Contains(AI_Wander.instance))_owner.stateMachine.ChangeState(AI_Wander.instance);
        if (_owner.critter.availableBehaviours.Contains(AI_Idle.instance))_owner.stateMachine.ChangeState(AI_Idle.instance);
    }

}