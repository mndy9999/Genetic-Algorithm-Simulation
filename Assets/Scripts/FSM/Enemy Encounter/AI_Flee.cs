using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine;
using System.Collections;

public class AI_Flee : State<AI>
{
    private static AI_Flee _instance;
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

    public override float GetWeight(AI _owner) { return Vector3.Distance(_owner.transform.position, _owner.seek.Enemy.transform.position)+_owner.critter.critterTraitsDict[Trait.RunSpeed]; }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Flee State");
        _owner.animator.Play("Run");      //play animation when entering state       
        _owner.agent.speed = _owner.critter.critterTraitsDict[Trait.RunSpeed];
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Flee State");       
        _owner.agent.ResetPath();
        _owner.agent.speed = _owner.critter.critterTraitsDict[Trait.WalkSpeed];
        _owner.seek.enemyType = _owner.seek.defaultEnemyType;
        _owner.critter.IsAlarmed = false;
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.critter.IsAttacked && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        if (!_owner.CanSeeEnemy() && _owner.critter.availableBehaviours.Contains(AI_Wander.instance)) { _owner.stateMachine.ChangeState(AI_Wander.instance); }
        if (!_owner.CanSeeEnemy() && _owner.critter.availableBehaviours.Contains(AI_Idle.instance)) { _owner.stateMachine.ChangeState(AI_Idle.instance); }       
        if(_owner.agent.remainingDistance <= _owner.agent.stoppingDistance) { GenerateNewDirection(_owner); }

    }

    void GenerateNewDirection(AI _owner)
    {
        if (_owner.seek.Target)
        {
            Vector3 direction = _owner.transform.position - _owner.seek.Target.transform.position;
            _owner.transform.Rotate(direction);
        }

        Vector3 newPos = _owner.transform.position + _owner.transform.forward * 10f;
        NavMeshHit hit;
        NavMesh.SamplePosition(newPos, out hit, 5f, _owner.agent.areaMask);
        _owner.agent.SetDestination(hit.position);
    }

}
