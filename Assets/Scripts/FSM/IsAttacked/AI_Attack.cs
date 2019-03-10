using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Attack : State<AI>
{
    private static AI_Attack _instance;
    public static string _name = "attack";
    private AI_Attack()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get the instance of the state
    public static AI_Attack instance
    {
        get
        {
            //if there is no instance
            if (_instance == null)
                new AI_Attack();      //create one
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
        Debug.Log("Entering Attack State");
        _owner.seek.Target.GetComponent<Critter>().IsChased = false;
        _owner.seek.Target.GetComponent<Critter>().IsAttacked = true;
        if(_owner.seek.Target.GetComponent<Seek>()) _owner.seek.Target.GetComponent<Seek>().Target = _owner.gameObject;
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Attack State");
        if(_owner.seek.Target) _owner.seek.Target.GetComponent<Critter>().IsAttacked = false;
    }

    public override void UpdateState(AI _owner)
    {
        
        if (_owner.IsDead()) {  _owner.stateMachine.ChangeState(AI_Dead.instance); }
       // else if (_owner.CanSeeEnemy() && _owner.critter.Health < 40) { _owner.stateMachine.ChangeState(AI_Evade.instance); }
        else if (_owner.CanSeeTarget())
        {
            if (_owner.IsCloseEnough())
            {
                if (_owner.TargetIsDead())
                {
                    if (_owner.TargetIsFood()) { _owner.stateMachine.ChangeState(AI_Eat.instance); }
                    else { _owner.seek.Target = null; _owner.stateMachine.ChangeState(AI_Idle.instance); }
                }
                else {
                    if (_owner.seek.Target.GetComponent<Critter>().critterType == "Tree" && _owner.critter.availableBehaviours.Contains(AI_Knock.name)) { _owner.stateMachine.ChangeState(AI_Knock.instance); }
                    else if (_owner.seek.Target.GetComponent<Critter>().critterType == "Dirt" && _owner.critter.availableBehaviours.Contains(AI_Dig.name)) { _owner.stateMachine.ChangeState(AI_Dig.instance); }
                    else { Attack(_owner); }
                }

            }
            else { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        }
        else { _owner.stateMachine.ChangeState(AI_Idle.instance); }

    }

    void Attack(AI _owner)
    {        
        float attackPower = 0.05f;
        var direction = _owner.seek.Target.transform.position - _owner.transform.position;
        _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                                    Quaternion.LookRotation(direction),
                                    _owner.critter.speed * Time.deltaTime);

        //start playing the animation
        _owner.animator.Play("Attack");
        _owner.seek.Target.GetComponent<Critter>().Health -= attackPower;
        
        
    }

}