using UnityEngine;
using FiniteStateMachine;

public class AI_Chase : State<AI>
{
    State<AI> bestState;

    private static AI_Chase _instance;
    private static string _name = "chase";
    private AI_Chase()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Chase instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Chase();     //create one
            return _instance;
        }
    }

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    private float weight = 1;
    public override float GetWeight(AI _owner) { return weight; }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Chase State");
        _owner.animator.Play("Run");      //play animation when entering state
        _owner.agent.ResetPath();
        _owner.agent.speed = _owner.critter.critterTraitsDict[Trait.RunSpeed];

        _owner.agent.SetDestination(_owner.seek.Target.transform.position);
        _owner.seek.Target.GetComponent<Critter>().IsAlarmed = true;

    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Chase State");
        _owner.agent.ResetPath();
        _owner.agent.speed = _owner.critter.critterTraitsDict[Trait.WalkSpeed];
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.IsAttacked()) { _owner.stateMachine.ChangeState(AI_Attack.instance); }

        if (_owner.CanSeeTarget())
        {
            if (_owner.TargetIsEnemy())
            {
                bestState = _owner.BestState(Behaviours.EnemyEncounterBehaviours);
                if (bestState != null) { _owner.stateMachine.ChangeState(bestState); }
            }

            if (_owner.TargetIsChallenger())
            {
                bestState = _owner.BestState(Behaviours.ChallengerEncounterBehaviours);
                if (bestState != null) { _owner.stateMachine.ChangeState(bestState); }
            }

            if (_owner.TargetIsCourter()) { _owner.stateMachine.ChangeState(AI_Watch.instance); }

            if (_owner.IsCloseEnough())
            {
                if (_owner.TargetIsMate()) { _owner.stateMachine.ChangeState(AI_Breed.instance); }
                if (_owner.TargetIsFood())
                {
                    if (_owner.TargetIsDead())
                    {
                        if (_owner.seek.Target.GetComponent<Critter>().critterType == "Tree") { _owner.stateMachine.ChangeState(AI_Knock.instance); }
                        if (_owner.seek.Target.GetComponent<Critter>().critterType == "Dirt") { _owner.stateMachine.ChangeState(AI_Dig.instance); }
                        _owner.stateMachine.ChangeState(AI_Eat.instance);
                    }
                    else { _owner.stateMachine.ChangeState(AI_Attack.instance); }
                }
            }

            if (_owner.TargetIsPotentialMate() && !_owner.IsCourted())
            {
                bestState = _owner.BestState(Behaviours.MateEncounterBehaviours);
                if (bestState != null) { _owner.stateMachine.ChangeState(bestState); }
            }

            if (_owner.TargetIsOpponent())
            {
                bestState = _owner.BestState(Behaviours.SocialRankBehaviours);
                if (bestState != null) { _owner.stateMachine.ChangeState(bestState); }
            }
            _owner.agent.SetDestination(_owner.seek.Target.transform.position);
        }
        else { _owner.stateMachine.ChangeState(AI_Wander.instance); }
    }
}