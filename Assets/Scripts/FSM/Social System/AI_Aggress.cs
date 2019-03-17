using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Aggress : State<AI>
{

    private static AI_Aggress _instance;
    private static string _name = "aggress";
    private AI_Aggress()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Aggress instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Aggress();     //create one
            return _instance;
        }
    }

    public override float GetWeight(AI _owner) { return _owner.critter.critterTraitsDict[Critter.Trait.VoiceStrenght]; }

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Aggress State");
        _owner.animator.Play("Bee");      //play animation when entering state   

        CalculateRankPoints(_owner);

        _owner.seek.Opponent.GetComponent<Critter>().isChallenged = true;
        _owner.seek.Opponent.GetComponent<Critter>().challengeTimer = 0;
        _owner.critter.challengeTimer = 0;
        _owner.seek.Opponent.GetComponent<Seek>().Opponent = _owner.gameObject;     
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Aggress State");
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
            if (Random.Range(0, _owner.seek.Target.GetComponent<Critter>().critterTraitsDict[Critter.Trait.VoiceStrenght] + _owner.critter.critterTraitsDict[Critter.Trait.VoiceStrenght]) < _owner.critter.critterTraitsDict[Critter.Trait.VoiceStrenght])
            {
                _owner.critter.critterTraitsDict[Critter.Trait.VoiceStrenght] += 0.2f;
                _owner.critter.critterTraitsDict[Critter.Trait.RankPoints] += 1;
                _owner.seek.Opponent.GetComponent<Critter>().critterTraitsDict[Critter.Trait.RankPoints] -= 1;
            }
            else
            {
                _owner.seek.Opponent.GetComponent<Critter>().critterTraitsDict[Critter.Trait.VoiceStrenght] += 0.2f;
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
        else if (_owner.CanSeeEnemy()) { _owner.stateMachine.ChangeState(AI_Evade.instance); }
        yield return new WaitForSeconds(5);
        if (_owner.CanSeeTarget()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        else { _owner.stateMachine.ChangeState(AI_Idle.instance); }
    }
}