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
        //if the AI is not close enough to the target
        if (!_owner.IsCloseEnoughToEat())
        {
            _owner.stateMachine.ChangeState(AI_Chase.instance);     //change into chase state
        }
        //otherwise, change the health level of the food target
        //and add points to the AI's food level - not yet implemented
        //(all food starts with 10HP. when it reaches 0, the gameObject is destroyed
        //  and the AI get the position of the next closest food source)
        else
        {
            float hpEaten = 0.1f;
            _owner.seek.Target.GetComponent<Critter>().health -= hpEaten;
            _owner.critter.energy += hpEaten * eatHPToEnergy;
        }
            
            

    }
}