using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Startle : State<AI>
{
    private static AI_Startle _instance;
    private static string _name = "startle";
    private AI_Startle()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Startle instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Startle();     //create one
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
        Debug.Log("Entering Starle State");
        _owner.animator.Play("ShowOff");      //play animation when entering state
        if (Random.Range(0, 10) < _owner.critter.critterTraitsDict[Critter.Trait.ThreatPoints])
        {
            _owner.seek.Enemy.GetComponent<Seek>().Enemy = _owner.gameObject;
            _owner.critter.critterTraitsDict[Critter.Trait.ThreatPoints] += 0.5f;
        }
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Starlte State");
        _owner.critter.IsAlarmed = false;
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if(!_owner.CanSeeEnemy()){ _owner.stateMachine.ChangeState(AI_Wander.instance); }
        else if (_owner.CanSeeEnemy())
        {
            var direction = _owner.seek.Enemy.transform.position - _owner.transform.position;
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
                                Quaternion.LookRotation(direction),
                                _owner.critter.speed * Time.deltaTime);
        }
        
    }

}
