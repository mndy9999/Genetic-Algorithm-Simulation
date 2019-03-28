using UnityEngine;
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
        _owner.agent.speed = _owner.critter.critterTraitsDict[Trait.WalkSpeed];
        
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Wander State");
        _owner.agent.ResetPath();
    }

    public override void UpdateState(AI _owner)
    {

        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.IsAttacked() && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        if (_owner.CanSeeTarget() && _owner.critter.availableBehaviours.Contains(AI_Chase.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        if (_owner.switchState || _owner.agent.remainingDistance <= _owner.agent.stoppingDistance) {
            if (!_owner.switchState && _owner.critter.availableBehaviours.Contains(AI_Idle.instance)) { _owner.stateMachine.ChangeState(AI_Idle.instance); }
            else { Wander(_owner); }
        }
    }

    void Wander(AI _owner)
    {
        targetPos = RandomNavSphere(_owner.transform.position, 20f, _owner.agent.areaMask);
        _owner.agent.SetDestination(targetPos);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitCircle.normalized * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

}
