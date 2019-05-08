using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Aggress : State<AI>
{
    State<AI> bestState;
    private static AI_Aggress _instance;
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

    public override float GetWeight(AI _owner) { return _owner.critter.critterTraitsDict[Trait.VoiceStrenght]; }

    public override void EnterState(AI _owner)
    {
        //Debug.Log("Entering Aggress State");
        _owner.animator.Play("Bee");      //play animation when entering state   

        CalculateRankPoints(_owner);

        _owner.seek.Target.GetComponent<Critter>().isChallenged = true;
        _owner.seek.Target.GetComponent<Critter>().ResetChallenge();
        _owner.critter.ResetChallenge();
        _owner.seek.Opponent.GetComponent<Seek>().Opponent = _owner.gameObject;

    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting Aggress State");
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {
        Vector3 direction = _owner.seek.LastKnownOpponent.transform.position - _owner.transform.position;
        _owner.transform.Rotate(direction);
        _owner.StartCoroutine(WaitForAnimation(_owner));
    }

    void CalculateRankPoints(AI _owner)
    {

        if (Random.Range(0, _owner.seek.Opponent.GetComponent<Critter>().critterTraitsDict[Trait.VoiceStrenght] + _owner.critter.critterTraitsDict[Trait.VoiceStrenght]) < _owner.critter.critterTraitsDict[Trait.VoiceStrenght])
        {
            _owner.critter.critterTraitsDict[Trait.VoiceStrenght] += 0.2f;
            _owner.critter.critterTraitsDict[Trait.RankPoints] += 1;
            _owner.seek.Target.GetComponent<Critter>().critterTraitsDict[Trait.RankPoints] -= 1;
        }
        else
        {
            _owner.seek.Target.GetComponent<Critter>().critterTraitsDict[Trait.VoiceStrenght] += 0.2f;
            _owner.seek.Target.GetComponent<Critter>().critterTraitsDict[Trait.RankPoints] += 1;
            _owner.critter.critterTraitsDict[Trait.RankPoints] -= 1;
        }

        _owner.seek.Target.GetComponent<Critter>().isChallenged = false;
    }

    IEnumerator WaitForAnimation(AI _owner)
    {
        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeTarget() && _owner.TargetIsEnemy() && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        yield return new WaitForSeconds(3);
        if (_owner.CanSeeTarget() && _owner.critter.availableBehaviours.Contains(AI_Chase.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        if (_owner.critter.availableBehaviours.Contains(AI_Chase.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        if (_owner.critter.availableBehaviours.Contains(AI_Idle.instance)) { _owner.stateMachine.ChangeState(AI_Idle.instance); }
    }
}