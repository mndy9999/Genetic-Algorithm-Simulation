using UnityEngine;
using FiniteStateMachine;

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

    private float weight = 1;
    public override float GetWeight(AI _owner) { return weight; }

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Aggress State");
        _owner.animator.Play("Attack");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Aggress State");
    }

    public override void UpdateState(AI _owner)
    {

    }
}