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
        //Debug.Log("Entering Wander State");
        _owner.animator.Play("Wander");  //start playing the animation when entering state
        if (_owner.agent.isActiveAndEnabled) _owner.agent.ResetPath();
        _owner.agent.speed = _owner.critter.critterTraitsDict[Trait.WalkSpeed];
        
    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting Wander State");
        _owner.agent.ResetPath();
    }

    public override void UpdateState(AI _owner)
    {

        if (_owner.IsDead() && _owner.critter.availableBehaviours.Contains(AI_Dead.instance)) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        if (_owner.IsAttacked() && _owner.critter.availableBehaviours.Contains(AI_Attack.instance)) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        if (_owner.CanSeeTarget() && _owner.critter.availableBehaviours.Contains(AI_Chase.instance)) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        if (!_owner.switchState && _owner.critter.availableBehaviours.Contains(AI_Idle.instance)) { _owner.stateMachine.ChangeState(AI_Idle.instance); }
        if (_owner.agent.remainingDistance <= _owner.agent.stoppingDistance) { Wander(_owner); }
    }

    void Wander(AI _owner)
    {
        NavMeshPath path = new NavMeshPath();

        do
        {
            Vector3 direction = new Vector3(Random.value, Random.value, Random.value);
            direction *= 20f;

            //randomly pick a negative value for the x or z
            if (Random.Range(0, 2) == 0) direction.x *= -1;
            if (Random.Range(0, 2) == 0) direction.z *= -1;

            //add the random vector to the current position
            Vector3 targetPos = _owner.transform.position + direction;

            //calculate the path
            _owner.agent.CalculatePath(targetPos, path);
        } while (path.status != NavMeshPathStatus.PathComplete);

        if (_owner.agent.isActiveAndEnabled) _owner.agent.SetPath(path);

    }

}
