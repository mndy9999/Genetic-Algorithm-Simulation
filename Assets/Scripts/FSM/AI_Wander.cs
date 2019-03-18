﻿using UnityEngine;
using FiniteStateMachine;
using UnityEngine.AI;

public class AI_Wander : State<AI>
{
    Vector3 targetPos;
    State<AI> bestState;
    private static AI_Wander _instance;
    private static string _name = "wander";
    private AI_Wander()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get the instance of the state
    public static AI_Wander instance
    {
        get
        {
            //if there is no instance
            if (_instance == null)
                new AI_Wander();      //create one
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
        Debug.Log("Entering Wander State");
        _owner.animator.Play("Wander");  //start playing the animation when entering state
        _owner.agent.ResetPath();
        Wander(_owner);
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Wander State");
        _owner.agent.ResetPath();
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy())
        {
            bestState = _owner.BestState(Behaviours.EnemyEncounterBehaviours);
            if (bestState != null)
                _owner.stateMachine.ChangeState(bestState);
        }
        else if (_owner.critter.isChallenged)
        {
            bestState = _owner.BestState(Behaviours.ChallengerEncounterBehaviours);
            if (bestState != null)
                _owner.stateMachine.ChangeState(bestState);
        }
        else if (_owner.CanSeeOpponent())
        {
            bestState = _owner.BestState(Behaviours.SocialRankBehaviours);
            if (bestState != null)
                _owner.stateMachine.ChangeState(bestState);
        }
        else if (_owner.CanSeeTarget()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        else if (_owner.CanSeeMate())
        {
            bestState = _owner.BestState(Behaviours.MateEncounterBehaviours);
            if (bestState != null)
                _owner.stateMachine.ChangeState(bestState);
        }
        
        else if (_owner.agent.remainingDistance <= _owner.agent.stoppingDistance + 1.0f) { _owner.stateMachine.ChangeState(AI_Idle.instance); }
        //else if(_owner.critter.Energy < 90) { _owner.stateMachine.ChangeState(AI_Laydown.instance); }

    }

    void Wander(AI _owner)
    {
        targetPos = RandomNavSphere(_owner.transform.position, 20f, _owner.agent.areaMask);
        _owner.agent.SetDestination(targetPos);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

}
