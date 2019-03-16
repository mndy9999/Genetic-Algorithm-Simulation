using UnityEngine;
using FiniteStateMachine;

public class AI_PlayDead : State<AI>
{
    private static AI_PlayDead _instance;
    public static string _name = "playDead";
    private AI_PlayDead()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_PlayDead instance
    {
        get
        {
            //if there is no instance
            if (_instance == null)
                new AI_PlayDead();      //create one
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
        Debug.Log("Entering PlayDead State");
        _owner.animator.Play("Dead");       //start playing animation when entering state
        _owner.critter.isAlarmed = true;
        _owner.critter.isVisible = Random.Range(0, 10)<3;
    }


    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting PlayDead State");
        _owner.animator.Play("Dead");
        _owner.critter.isVisible = true;
        _owner.critter.IsAlarmed = false;
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (!_owner.CanSeeEnemy()) { _owner.stateMachine.ChangeState(AI_Idle.instance); }
    }
}
