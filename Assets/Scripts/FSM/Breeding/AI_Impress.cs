using UnityEngine;
using FiniteStateMachine;

public class AI_Impress : State<AI>
{

    private static AI_Impress _instance;
    private static string _name = "impress";
    private AI_Impress()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Impress instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Impress();     //create one
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
        Debug.Log("Entering Impress State");
        _owner.animator.Play("ShowOff");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Impress State");
    }

    public override void UpdateState(AI _owner)
    {

    }
}