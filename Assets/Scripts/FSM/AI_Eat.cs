using UnityEngine;
using FiniteStateMachine;

public class AI_Eat : State<AI>
{
    State<AI> bestState;

    public float eatHPPerSecond = 5f;
    public float eatHPToEnergy = 2f;

    private static AI_Eat _instance;
    private static string _name = "eat";
    private AI_Eat()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get the instance of the state
    public static AI_Eat instance
    {
        get
        {
            //if there is no instance, create one
            if (_instance == null)
                new AI_Eat();
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
        //Debug.Log("Entering Eat State");
        _owner.animator.Play("Eat");    //start playing animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting Eat State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.critter.IsAttacked && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        if (_owner.CanSeeEnemy())
        {
            bestState = _owner.BestState(Behaviours.EnemyEncounterBehaviours);
            if (bestState != null)
                _owner.stateMachine.ChangeState(bestState);
        }
        if (_owner.CanSeeTarget())
        {
            if (_owner.IsCloseEnough())
            {
                if (_owner.TargetIsFood())
                {
                    if (_owner.TargetIsDead()) { Eat(_owner); }
                    else if (_owner.critter.availableBehaviours.Contains(AI_Attack.instance)){ _owner.stateMachine.ChangeState(AI_Attack.instance); }
                }
            }
            else if(_owner.critter.availableBehaviours.Contains(AI_Chase.instance)){ _owner.stateMachine.ChangeState(AI_Chase.instance); }
        }
        else if (_owner.critter.availableBehaviours.Contains(AI_Wander.instance)) { _owner.stateMachine.ChangeState(AI_Wander.instance); }
        else if (_owner.critter.availableBehaviours.Contains(AI_Idle.instance)){ _owner.stateMachine.ChangeState(AI_Idle.instance); }
    }

    void Eat(AI _owner)
    {
        
        float hpEaten = 0.1f;
        _owner.seek.Target.GetComponent<Critter>().Resource -= hpEaten;
        _owner.critter.Energy += hpEaten * eatHPToEnergy;
    }


}