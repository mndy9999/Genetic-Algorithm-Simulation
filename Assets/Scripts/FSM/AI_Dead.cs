using UnityEngine;
using FiniteStateMachine;

public class AI_Dead : State<AI>
{

    public float eatHPPerSecond = 5f;
    public float eatHPToEnergy = 2f;

    private static AI_Dead _instance;
    private static string _name = "dead";
    private AI_Dead()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get the instance of the state
    public static AI_Dead instance
    {
        get
        {
            //if there is no instance, create one
            if (_instance == null)
                new AI_Dead();
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
        Debug.Log("Entering Dead State");
        _owner.animator.Play("Dead");    //start playing animation when entering state
        _owner.critter.IsAlive = false;
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Dead State");
    }

    public override void UpdateState(AI _owner)
    {
        if(_owner.critter.Resource <= 0) { Object.Destroy(_owner.gameObject); }
    }


}