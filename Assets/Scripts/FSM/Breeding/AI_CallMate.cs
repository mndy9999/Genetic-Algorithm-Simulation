using UnityEngine;
using FiniteStateMachine;

public class AI_CallMate : State<AI>
{
    private static AI_CallMate _instance;
    private static string _name = "call_mate";
    public static float weight;
    private AI_CallMate()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_CallMate instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_CallMate();     //create one
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
        weight = _owner.critter.critterTraitsDict[Critter.Trait.VoiceStrenght];
        Debug.Log("Entering Call Mate State");
        _owner.animator.Play("Bee");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Call Mate State");
    }

    public override void UpdateState(AI _owner)
    {

    }
}