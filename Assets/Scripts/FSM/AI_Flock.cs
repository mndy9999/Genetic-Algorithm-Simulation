using UnityEngine;
using FiniteStateMachine;
using System.Collections.Generic;

public class AI_Flock : State<AI>
{
    Flock flock;  

    private static AI_Flock _instance;
    private static string _name = "flock";
    private AI_Flock()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
    //get the instance of the state
    public static AI_Flock instance
    {
        get
        {
            //if there is no instance, create one
            if (_instance == null)
                new AI_Flock();
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
        Debug.Log("Entering Flock State");
        _owner.animator.Play("Walk");    //start playing animation when entering state
        flock = _owner.GetComponent<Flock>();
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting Flock State");
    }

    public override void UpdateState(AI _owner)
    {

    }

    Vector3 Flock(AI _owner, List<Transform> context, Flock flock)
    {
        Vector3 move = Vector3.zero;

        Vector3 partialMove = Cohision(_owner, context, flock) * flock.weights[0];
        if (partialMove != Vector3.zero)
        {
            if (partialMove.sqrMagnitude > flock.weights[0] * flock.weights[0])
            {
                partialMove.Normalize();
                partialMove *= flock.weights[0];
            }
            move += partialMove;
        }

        partialMove = Align(_owner, context, flock) * flock.weights[1];
        if (partialMove != Vector3.zero)
        {
            if (partialMove.sqrMagnitude > flock.weights[1] * flock.weights[1])
            {
                partialMove.Normalize();
                partialMove *= flock.weights[1];
            }
            move += partialMove;
        }

        partialMove = Avoid(_owner, context, flock) * flock.weights[2];
        if (partialMove != Vector3.zero)
        {
            if (partialMove.sqrMagnitude > flock.weights[2] * flock.weights[2])
            {
                partialMove.Normalize();
                partialMove *= flock.weights[2];
            }
            move += partialMove;
        }

        move.y = _owner.transform.position.y;
        return move;
    }


    Vector3 Cohision(AI _owner, List<Transform> context, Flock flock)
    {
        if (context.Count == 0) { return Vector3.zero; }

        Vector3 cohesionMove = Vector3.zero;
        foreach (Transform item in context)
        {
            cohesionMove += item.position;
        }

        cohesionMove /= context.Count;
        cohesionMove -= _owner.transform.position;
        return cohesionMove;

    }

    Vector3 Align(AI _owner, List<Transform> context, Flock flock)
    {
        if (context.Count == 0) { return _owner.transform.forward; }

        Vector3 alignmentMove = Vector3.zero;
        foreach (Transform item in context)
        {
            alignmentMove += item.transform.forward;
        }

        alignmentMove /= context.Count;
        return alignmentMove;
    }

    Vector3 Avoid(AI _owner, List<Transform> context, Flock flock)
    {
        if (context.Count == 0) { return Vector3.zero; }

        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0;
        foreach (Transform item in context)
        {
            if (Vector3.SqrMagnitude(item.position - _owner.transform.position) < flock.SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += _owner.transform.position - item.position;
            }
        }
        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }
        return avoidanceMove;

    }

}