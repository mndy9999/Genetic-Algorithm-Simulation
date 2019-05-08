using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Breed : State<AI>
{
    BreedingController breeding;

    float time;
    private static AI_Breed _instance;
    private AI_Breed()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Breed instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Breed();     //create one
            return _instance;
        }
    }

    private float weight = 1;
    public override float GetWeight(AI _owner) { return weight; }

    public override void EnterState(AI _owner)
    {
        //Debug.Log("Entering Breed State");
        _owner.animator.Play("Jump");      //play animation when entering state
        breeding = _owner.GetComponent<BreedingController>();
        if (breeding)
        {
            breeding.mother = _owner.critter;
            breeding.father = _owner.seek.Mate.GetComponent<Critter>();
        }

        time = Time.time;
        _owner.StartCoroutine(WaitForAnimation(_owner));
    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting Breed State");
        
        _owner.seek.Mate = null;
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {
        
    }

    IEnumerator WaitForAnimation(AI _owner)
    {
        yield return new WaitForSeconds(3);
        breeding = _owner.GetComponent<BreedingController>();
        if (breeding)
        {
            breeding.CreateOffspring();
            breeding.BehavioursCrossover();
            breeding.TraitsCrossover();
        }
        _owner.seek.Target.GetComponent<Critter>().ResetBreed();
        _owner.critter.ResetBreed();        
        if (_owner.critter.availableBehaviours.Contains(AI_Wander.instance)) { _owner.stateMachine.ChangeState(AI_Wander.instance); }
        if (_owner.critter.availableBehaviours.Contains(AI_Idle.instance)) { _owner.stateMachine.ChangeState(AI_Idle.instance); }
        
    }
}