using UnityEngine;
using FiniteStateMachine;

public class AI_Idle : State<AI>
{
    State<AI> bestState;
    private static AI_Idle _instance;
    public static string _name = "idle";
    private AI_Idle()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Idle instance
    {
        get
        {
            //if there is no instance
            if (_instance == null)
                new AI_Idle();      //create one
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
        Debug.Log("Entering Idle State");
        _owner.animator.Play("Idle");       //start playing animation when entering state
        _owner.seconds = 0;
    }


    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Idle State");
    }

    public override void UpdateState(AI _owner)
    {
     
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy())
        {
            bestState = _owner.BestState(Behaviours.EnemyEncounterBehaviours);
            if (bestState != null)
                _owner.stateMachine.ChangeState(bestState);
        }
        else if (_owner.critter.isChallenged)
        {
            bestState = _owner.BestState(Behaviours.ChallengerEncounterBehaviours);
            if (bestState != null)
                _owner.stateMachine.ChangeState(bestState);
        }
        else if (_owner.CanSeeOpponent())
        {
            bestState = _owner.BestState(Behaviours.SocialRankBehaviours);
            if (bestState != null)
                _owner.stateMachine.ChangeState(bestState);
        }
        else if (_owner.CanSeeTarget()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        else if (_owner.CanSeeMate())
        {
            bestState = _owner.BestState(Behaviours.MateEncounterBehaviours);
            if (bestState != null)
                _owner.stateMachine.ChangeState(bestState);
        }
    else if (_owner.switchState) { _owner.stateMachine.ChangeState(AI_Wander.instance); }

        //else if (_owner.critter.Energy < 90) { _owner.stateMachine.ChangeState(AI_Laydown.instance); }
    }
}
