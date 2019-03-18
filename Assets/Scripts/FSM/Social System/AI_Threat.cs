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

    public override float GetWeight(AI _owner) { return _owner.critter.critterTraitsDict[Critter.Trait.ThreatPoints]; }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Threat State");
        _owner.animator.Play("ShowOff");      //play animation when entering state   

        CalculateRankPoints(_owner);

        _owner.seek.Opponent.GetComponent<Critter>().isChallenged = true;
        _owner.seek.Opponent.GetComponent<Critter>().challengeTimer = 0;
        _owner.critter.challengeTimer = 0;
        _owner.seek.Opponent.GetComponent<Seek>().Opponent = _owner.gameObject;    
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Threat State");
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {
        _owner.StartCoroutine(WaitForAnimation(_owner));
    }

    void CalculateRankPoints(AI _owner)
    {
        if (_owner.critter.canChallenge)
        {
            if (Random.Range(0, _owner.seek.Opponent.GetComponent<Critter>().fitnessScore + _owner.critter.fitnessScore) < _owner.critter.fitnessScore)
            {
                _owner.critter.critterTraitsDict[Critter.Trait.ThreatPoints] += 0.2f;
                _owner.critter.critterTraitsDict[Critter.Trait.RankPoints] += 1;
                _owner.seek.Opponent.GetComponent<Critter>().critterTraitsDict[Critter.Trait.RankPoints] -= 1;
            }
            else
            {
                _owner.seek.Opponent.GetComponent<Critter>().critterTraitsDict[Critter.Trait.ThreatPoints] += 0.2f;
                _owner.seek.Opponent.GetComponent<Critter>().critterTraitsDict[Critter.Trait.RankPoints] += 1;
                _owner.critter.critterTraitsDict[Critter.Trait.RankPoints] -= 1;
            }
            
        }
        _owner.critter.isChallenged = false;
        _owner.seek.LastKnownOpponent.GetComponent<Critter>().isChallenged = false;
    }

    IEnumerator WaitForAnimation(AI _owner)
    {       
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy())
        {
            bestState = _owner.BestState(Behaviours.EnemyEncounterBehaviours);
            if (bestState != null)
                _owner.stateMachine.ChangeState(bestState);
        }
        else if(_owner.critter.IsAlarmed || _owner.seek.LastKnownTarget.GetComponent<Critter>().IsAlarmed) { _owner.stateMachine.ChangeState(AI_Idle.instance); }
        yield return new WaitForSeconds(5);
        if (_owner.CanSeeTarget()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        else { _owner.stateMachine.ChangeState(AI_Idle.instance); }
    }
}
