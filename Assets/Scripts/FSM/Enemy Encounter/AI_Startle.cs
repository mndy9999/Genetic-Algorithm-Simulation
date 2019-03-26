using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Startle : State<AI>
{
    State<AI> bestState;
    private static AI_Startle _instance;
    private AI_Startle()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Startle instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Startle();     //create one
            return _instance;
        }
    }

    public override float GetWeight(AI _owner) { return _owner.critter.critterTraitsDict[Trait.ThreatPoints]; }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Starle State");
        _owner.animator.Play("ShowOff");      //play animation when entering state
        if (Random.Range(0, 10) < GetWeight(_owner))
        {
                _owner.seek.Enemy.GetComponent<Seek>().Enemy = _owner.gameObject;
                _owner.seek.Enemy.GetComponent<Seek>().enemyType = "Herbivore";
                _owner.seek.Enemy.GetComponent<Critter>().IsAlarmed = true;
                _owner.critter.critterTraitsDict[Trait.ThreatPoints] += 0.5f;
        }
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Starlte State");
        _owner.StopAllCoroutines();
        _owner.critter.IsAlarmed = false;
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.seek.Enemy)
        {
            var direction = _owner.seek.Enemy.transform.position - _owner.transform.position;
            _owner.transform.Rotate(direction);
        }

        _owner.StartCoroutine(WaitForAnimation(_owner));
    }

    IEnumerator WaitForAnimation(AI _owner)
    {
        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        yield return new WaitForSeconds(2f);
        if (_owner.CanSeeTarget() && _owner.critter.availableBehaviours.Contains(AI_Chase.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        if (_owner.critter.availableBehaviours.Contains(AI_Wander.instance)) { _owner.stateMachine.ChangeState(AI_Wander.instance); }
        if (_owner.critter.availableBehaviours.Contains(AI_Idle.instance)) { _owner.stateMachine.ChangeState(AI_Idle.instance); }
    }

}
