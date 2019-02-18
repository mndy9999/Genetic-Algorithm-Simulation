using UnityEngine;
using FiniteStateMachine;

public class AI_Eat : State<AI>
{

    public float eatHPPerSecond = 5f;
    public float eatHPToEnergy = 2f;

    private static AI_Eat _instance;
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
    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Eat State");
        _owner.animator.Play("Eat");    //start playing animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Eat State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        if (!_owner.IsCloseEnoughToEat()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        if (_owner.CanSeeTarget())
        {
            float hpEaten = 0.1f;
            _owner.seek.Target.GetComponent<Critter>().resource -= hpEaten;
            _owner.critter.energy += hpEaten * eatHPToEnergy;
        }
    }


}