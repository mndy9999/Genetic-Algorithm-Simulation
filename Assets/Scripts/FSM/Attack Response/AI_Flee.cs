using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine;
using System.Collections;

public class AI_Flee : State<AI>
{
    private static AI_Flee _instance;
    private static string _name = "fee";
    private AI_Flee()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Flee instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Flee();     //create one
            return _instance;
        }
    }

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    public override float GetWeight(AI _owner) { return Vector3.Distance(_owner.transform.position, _owner.seek.Enemy.transform.position)+_owner.critter.critterTraitsDict[Critter.Trait.RunSpeed]; }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Flee State");
        _owner.animator.Play("Run");      //play animation when entering state       
        
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Flee State");
        
        _owner.agent.ResetPath();
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy()) { GenerateNewDirection(_owner); }
        else { Resume(_owner); }

    }

    void GenerateNewDirection(AI _owner)
    {
        Vector3 direction = _owner.seek.Enemy.transform.position - _owner.transform.position;

        _owner.transform.rotation = Quaternion.Lerp(_owner.transform.rotation,
                                    Quaternion.LookRotation(-direction),
                                    _owner.critter.critterTraitsDict[Critter.Trait.WalkSpeed] * Time.deltaTime);

        Vector3 newPos = _owner.transform.position + _owner.transform.forward * 10f;
        NavMeshHit hit;
        NavMesh.SamplePosition(newPos, out hit, 5f, _owner.agent.areaMask);
        _owner.agent.SetDestination(hit.position);

    }


    void Resume(AI _owner)
    { 
        _owner.seek.enemyType = _owner.seek.defaultEnemyType;
        _owner.critter.IsAlarmed = false;
        _owner.stateMachine.ChangeState(AI_Wander.instance);
    }
}
