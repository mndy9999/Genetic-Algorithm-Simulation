using UnityEngine;
using FiniteStateMachine;

public class AI_Breed : State<AI>
{
    float time;
    private static AI_Breed _instance;
    private static string _name = "breed";
    private AI_Breed()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Breed instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Breed();     //create one
            return _instance;
        }
    }

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Breed State");
        _owner.animator.Play("Jump");      //play animation when entering state
        time = Time.time;
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Breed State");
    }

    public override void UpdateState(AI _owner)
    {        
        if(Time.time >= time + 4.6) {
            Debug.Log("Hi");
            _owner.critter.BreedTimer = true;
            _owner.stateMachine.ChangeState(AI_Idle.instance);

        }
    }
}