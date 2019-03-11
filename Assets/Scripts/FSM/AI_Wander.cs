using UnityEngine;
using FiniteStateMachine;
using UnityEngine.AI;

public class AI_Wander : State<AI>
{
    Vector3 targetPos;

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

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Wander State");
        _owner.animator.Play("Wander");  //start playing the animation when entering state
        Wander(_owner);
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Wander State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy()) { _owner.stateMachine.ChangeState(AI_Evade.instance); }
        else if (_owner.CanSeeTarget()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        else if (_owner.agent.remainingDistance<=_owner.agent.stoppingDistance) { _owner.stateMachine.ChangeState(AI_Idle.instance); }
        

    }

    void Wander(AI _owner)
    {
        targetPos = RandomNavSphere(_owner.transform.position, 20f, _owner.agent.areaMask);
        _owner.agent.SetDestination(targetPos);


        //var direction = _owner.waypoint - _owner.transform.position;
        //_owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation,
        //                            Quaternion.LookRotation(direction),
        //                            _owner.critter.speed * Time.deltaTime);
        //_owner.transform.Translate(0, 0, Time.deltaTime * _owner.critter.speed);
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
