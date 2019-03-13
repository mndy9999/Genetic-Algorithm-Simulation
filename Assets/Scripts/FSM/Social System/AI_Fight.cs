using UnityEngine;
using FiniteStateMachine;

public class AI_Fight : State<AI>
{

    private static AI_Fight _instance;
    private static string _name = "fight";
    private AI_Fight()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Fight instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Fight();     //create one
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
        Debug.Log("Entering Fight State");
        _owner.animator.Play("Attack");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Fight State");
    }

    public override void UpdateState(AI _owner)
    {

    }
}