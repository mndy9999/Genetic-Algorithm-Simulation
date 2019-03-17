using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine;
using System.Collections;

public class AI_Evade : State<AI>
{
    private static AI_Evade _instance;
    private static string _name = "evade";
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

    public static string name
    {
        get { return _name; }
        set { _name = value; }
    }

    private float weight = 1;
    public override float GetWeight(AI _owner) { return weight; }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Evade State");
        //_owner.agent.speed = _owner.critter.critterTraitsDict[Critter.Trait.RunSpeed];
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Evade State");
        _owner.StopAllCoroutines();
        _owner.agent.ResetPath();
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy()) {
            State<AI> bestState = _owner.BestState(Behaviours.EnemyEncounterBehaviours);
            if(bestState != null)
                _owner.stateMachine.ChangeState(bestState);
            else
                GenerateNewDirection(_owner);
        }       
        _owner.StartCoroutine(Resume(_owner));
    }

    IEnumerator Resume(AI _owner)
    {
        GenerateNewTargetPos(_owner);
        yield return new WaitForSeconds(3);
        _owner.critter.IsAlarmed = false;
        _owner.stateMachine.ChangeState(AI_Wander.instance);
    }

    void GenerateNewDirection(AI _owner)
    {
        Vector3 direction = _owner.seek.Enemy.transform.position - _owner.transform.position;
        _owner.transform.rotation = Quaternion.Lerp(_owner.transform.rotation,
                                    Quaternion.LookRotation(-direction),
                                    _owner.critter.critterTraitsDict[Critter.Trait.WalkSpeed] * Time.deltaTime);
    }

    void GenerateNewTargetPos(AI _owner)
    {
        Vector3 newPos = _owner.transform.position + _owner.transform.forward * 5f;
        _owner.agent.SetDestination(newPos);
    }

}