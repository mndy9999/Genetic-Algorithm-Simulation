using UnityEngine;
using FiniteStateMachine;
using System.Collections;

public class AI_Impress : State<AI>
{

    private static AI_Impress _instance;
    private static string _name = "impress";
    public static float weight;
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

    public override void EnterState(AI _owner)
    {
        weight = _owner.critter.critterTraitsDict[Critter.Trait.Beauty];
        Debug.Log("Entering Impress State");
        _owner.animator.Play("ShowOff");      //play animation when entering state
        _owner.StartCoroutine(WaitForAnimation(_owner));

    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Impress State");
        _owner.StopAllCoroutines();
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.IsDead()) { _owner.stateMachine.ChangeState(AI_Dead.instance); }
        else if (_owner.critter.IsAttacked) { _owner.stateMachine.ChangeState(AI_Attack.instance); }
        else if (_owner.CanSeeEnemy()) { _owner.stateMachine.ChangeState(AI_Evade.instance); }
        else if (_owner.CanSeeTarget()) { _owner.stateMachine.ChangeState(AI_Chase.instance); }
        else { _owner.stateMachine.ChangeState(AI_Idle.instance); }
    }

    IEnumerator WaitForAnimation(AI _owner)
    {
        yield return new WaitForSeconds(5);
        if (Random.Range(0, 10) < _owner.critter.critterTraitsDict[Critter.Trait.Beauty])
        {
            _owner.seek.Target.GetComponent<Seek>().Mate = _owner.gameObject;
            _owner.stateMachine.ChangeState(AI_Breed.instance);
        }
    }
}