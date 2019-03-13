using UnityEngine;
using FiniteStateMachine;

public class AI_Submit : State<AI>
{

    private static AI_Submit _instance;
    private static string _name = "submit";
    private AI_Submit()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Submit instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Submit();     //create one
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
        Debug.Log("Entering Submit State");
        _owner.animator.Play("Dig");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Submit State");
    }

    public override void UpdateState(AI _owner)
    {

    }
}