using UnityEngine;
using FiniteStateMachine;

public class AI_Look : State<AI>
{

    private static AI_Look _instance;
    private static string _name = "look";
    private AI_Look()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Look instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Look();     //create one
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
        //Debug.Log("Entering Look State");
        _owner.animator.Play("Dig");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting Look State");
    }

    public override void UpdateState(AI _owner)
    {

    }
}