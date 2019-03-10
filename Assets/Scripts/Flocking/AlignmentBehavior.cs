using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Alignemnt")]
public class AlignmentBehavior : FlockBehaviour {

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (context.Count == 0) { return agent.transform.forward; }

        Vector3 alignmentMove = Vector3.zero;
        foreach (Transform item in context)
        {
            alignmentMove += item.transform.forward;
        }

        alignmentMove /= context.Count;
        alignmentMove.y = agent.transform.position.y;
        return alignmentMove;
    }
}
