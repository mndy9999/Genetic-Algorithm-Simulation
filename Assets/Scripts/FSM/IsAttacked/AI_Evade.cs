using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine;

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

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Evade State");
        _owner.animator.Play("Run");      //play animation when entering state
        Evade(_owner);
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Evade State");
        _owner.agent.Stop();
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy()) {
            if (_owner.CanSeeWater() && _owner.critter.availableBehaviours.Contains(AI_Swim.name) && !_owner.seek.Enemy.GetComponent<CheckEnvironment>().InWater){ _owner.stateMachine.ChangeState(AI_Swim.instance); }
        }
        else if(!_owner.CanSeeEnemy()) { _owner.critter.IsAlarmed = false; _owner.stateMachine.ChangeState(AI_Wander.instance); }
    }

    void Evade(AI _owner)
    {
        Vector3 targetPos = RandomNavSphere(_owner.transform.position, 20f, _owner.agent.areaMask);
        _owner.agent.SetDestination(targetPos);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection.z = dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}