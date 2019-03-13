using UnityEngine;
using FiniteStateMachine;

public class AI_PairUp : State<AI>
{

    private static AI_PairUp _instance;
    private static string _name = "pair_up";
    private AI_PairUp()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_PairUp instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_PairUp();     //create one
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
        Debug.Log("Entering Pair Up State");
        _owner.animator.Play("Jump");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Pair Up State");
    }

    public override void UpdateState(AI _owner)
    {

    }
}