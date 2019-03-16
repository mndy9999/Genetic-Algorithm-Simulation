using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Impress : State<AI>
{
    private static AI_Impress _instance;
    private static string _name = "impress";

    private AI_Impress()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get instance of the state
    public static AI_Impress instance
    {
        get
        {
            //if there is no insteance, create one
            if (_instance == null)
                new AI_Impress();     //create one
            return _instance;
        }
    }

    public static string name { get { return _name; } }

    public override float GetWeight(AI _owner) { return _owner.critter.critterTraitsDict[Critter.Trait.Beauty]; }

    public override void EnterState(AI _owner)
    {

        Debug.Log("Entering Impress State");
        _owner.animator.Play("ShowOff");      //play animation when entering state
       
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Impress State");
        _owner.critter.canBreed = false;
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {
        _owner.StartCoroutine(WaitForAnimation(_owner));

    }

    IEnumerator WaitForAnimation(AI _owner)
    {
        yield return new WaitForSeconds(3);
        if (Random.Range(0, 10) < _owner.critter.critterTraitsDict[Critter.Trait.Beauty])
        {
            Debug.Log("Success!");
            _owner.seek.Target.GetComponent<Seek>().Mate = _owner.gameObject;
            _owner.stateMachine.ChangeState(AI_Breed.instance);
        }
        else {            
            if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
            else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
            else if (_owner.CanSeeEnemy()) { _owner.stateMachine.ChangeState(AI_Evade.instance); }
            else if (_owner.CanSeeTarget()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
            else { _owner.stateMachine.ChangeState(AI_Idle.instance); }
        }
    }
}