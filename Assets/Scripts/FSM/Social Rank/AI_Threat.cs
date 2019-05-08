using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Threat : State<AI>
{
    State<AI> bestState;
    private static AI_Threat _instance;
    private static string _name = "threat";
    private AI_Threat()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Threat instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Threat();     //create one
            return _instance;
        }
    }

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    public override float GetWeight(AI _owner) { return _owner.critter.critterTraitsDict[Trait.ThreatPoints]; }

    public override void EnterState(AI _owner)
    {
        //Debug.Log("Entering Threat State");
        _owner.animator.Play("ShowOff");      //play animation when entering state   

        CalculateRankPoints(_owner);

        _owner.seek.Target.GetComponent<Critter>().isChallenged = true;
        _owner.seek.Target.GetComponent<Critter>().ResetChallenge();
        _owner.critter.ResetChallenge();
        _owner.seek.Target.GetComponent<Seek>().Opponent = _owner.gameObject;    
    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting Threat State");
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {
        _owner.StartCoroutine(WaitForAnimation(_owner));
    }

    void CalculateRankPoints(AI _owner)
    {

        if (Random.Range(0, _owner.seek.Opponent.GetComponent<Critter>().FitnessScore + _owner.critter.FitnessScore) < _owner.critter.FitnessScore)
        {
            _owner.critter.critterTraitsDict[Trait.ThreatPoints] += 0.2f;
            _owner.critter.critterTraitsDict[Trait.RankPoints] += 1;
            _owner.seek.Target.GetComponent<Critter>().critterTraitsDict[Trait.RankPoints] -= 1;
        }
        else
        {
            _owner.seek.Target.GetComponent<Critter>().critterTraitsDict[Trait.ThreatPoints] += 0.2f;
            _owner.seek.Target.GetComponent<Critter>().critterTraitsDict[Trait.RankPoints] += 1;
            _owner.critter.critterTraitsDict[Trait.RankPoints] -= 1;
        }

        _owner.seek.Target.GetComponent<Critter>().isChallenged = false;
    }

    IEnumerator WaitForAnimation(AI _owner)
    {
        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeTarget() && _owner.TargetIsEnemy() && _owner.critter.availableBehaviours.Contains(AI_Chase.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        yield return new WaitForSeconds(3);
        if (_owner.CanSeeTarget() && _owner.critter.availableBehaviours.Contains(AI_Chase.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        if (_owner.critter.availableBehaviours.Contains(AI_Wander.instance)) { _owner.stateMachine.ChangeState(AI_Wander.instance); }
        if (_owner.critter.availableBehaviours.Contains(AI_Idle.instance)) { _owner.stateMachine.ChangeState(AI_Idle.instance); }
    }
}
