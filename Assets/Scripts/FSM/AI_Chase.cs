using UnityEngine;
using FiniteStateMachine;

public class AI_Chase : State<AI>
{
    private static AI_Chase _instance;
    private AI_Chase()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Chase instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Chase();     //create one
            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Chase State");
        _owner.animator.Play("Run");      //play animation when entering state
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Chase State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        //if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        //if the enemy is in the AI's sight
        if (_owner.CanSeeEnemy())
        {
            _owner.stateMachine.ChangeState(AI_Evade.instance);     //change to evade state
        }
        //if the AI is close enough to the target
        if (_owner.IsCloseEnoughToEat())
        {

            _owner.stateMachine.ChangeState(AI_Attack.instance);

            //_owner.stateMachine.ChangeState(AI_Eat.instance);       //change to eating state
        }
        
        else if (_owner.CanSeeTarget())
        {
            //calculate direction, rotation and start moving towards the target
            if (!Critter.crittersDict.ContainsKey(_owner.seek.targetType)) { return; }
            var direction = _owner.seek.Target.transform.position - _owner.transform.position;
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                                        Quaternion.LookRotation(direction),
                                        _owner.critter.speed * Time.deltaTime);
            _owner.transform.Translate(0, 0, Time.deltaTime * _owner.critter.speed);
        }
        //if the target is out of the AI's sight
        else
        {
            _owner.stateMachine.ChangeState(AI_Idle.instance);      //change to idle
        }



    }
    
}