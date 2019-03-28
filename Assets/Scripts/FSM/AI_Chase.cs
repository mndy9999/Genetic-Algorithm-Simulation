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

        Vector3 direction = _owner.transform.position - _owner.seek.Target.transform.position;
        _owner.transform.Rotate(direction);

        _owner.seek.Target.GetComponent<Critter>().IsAlarmed = true;

    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Chase State");
        _owner.agent.ResetPath();
        _owner.agent.speed = _owner.critter.critterTraitsDict[Trait.WalkSpeed];
        if(_owner.seek.LastKnownTarget != null)
            _owner.seek.LastKnownTarget.GetComponent<Critter>().IsAlarmed = false;
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.IsAttacked() && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }

        if (_owner.CanSeeTarget())
        {
            if (_owner.TargetIsEnemy())
            {
                bestState = _owner.BestState(Behaviours.EnemyEncounterBehaviours);
                if (bestState != null) { _owner.stateMachine.ChangeState(bestState); }
            }

            else _owner.agent.SetDestination(_owner.seek.Target.transform.position);

            if (_owner.TargetIsChallenger())
            {
                bestState = _owner.BestState(Behaviours.ChallengerEncounterBehaviours);
                if (bestState != null) { _owner.stateMachine.ChangeState(bestState); }
            }

            if (_owner.TargetIsCourter() && _owner.critter.availableBehaviours.Contains(AI_Watch.instance)) { _owner.stateMachine.ChangeState(AI_Watch.instance); }

            if (_owner.IsCloseEnough())
            {
                if (_owner.TargetIsMate() && _owner.critter.availableBehaviours.Contains(AI_Breed.instance)) { _owner.stateMachine.ChangeState(AI_Breed.instance); }
                if (_owner.TargetIsFood())
                {
                    if (_owner.TargetIsDead())
                    {
                        if (_owner.critter.availableBehaviours.Contains(AI_Eat.instance)) { _owner.stateMachine.ChangeState(AI_Eat.instance); }
                    }
                    if (_owner.seek.Target.GetComponent<Critter>().critterType == "Tree") { _owner.stateMachine.ChangeState(AI_Knock.instance); }
                    if (_owner.seek.Target.GetComponent<Critter>().critterType == "Dirt") { _owner.stateMachine.ChangeState(AI_Dig.instance); }
                    if (_owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
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

        }
        else
        {
            if (_owner.critter.availableBehaviours.Contains(AI_Wander.instance)) { _owner.stateMachine.ChangeState(AI_Wander.instance); }
            if (_owner.critter.availableBehaviours.Contains(AI_Idle.instance)) { _owner.stateMachine.ChangeState(AI_Idle.instance); }
        }
        }
}