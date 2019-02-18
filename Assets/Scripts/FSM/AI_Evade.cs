using UnityEngine;
using FiniteStateMachine;

public class AI_Evade : State<AI>
{
    private static AI_Evade _instance;
    private AI_Evade()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Evade instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Evade();     //create one
            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Evade State");
        _owner.animator.Play("Run");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Evade State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        if (_owner.CanSeeEnemy())
        {
            //calculate direction, rotation and start moving towards the target
            if (!Critter.crittersDict.ContainsKey(_owner.seek.enemyType)) { return; }
            var direction = _owner.seek.Enemy.transform.position - _owner.transform.position;
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                                        Quaternion.LookRotation(-direction),
                                        _owner.critter.speed * Time.deltaTime);
            _owner.transform.Translate(0, 0, Time.deltaTime * _owner.critter.speed);
        }
        //if the enemy is out of the AI's sight
        else
        {
            _owner.stateMachine.ChangeState(AI_Idle.instance);      //change to idle
        }
    }

}