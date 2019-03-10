using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Composite")]
public class CompositBehavior : FlockBehaviour {

    public FlockBehaviour[] behaviors;
    public float[] weights;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if(weights.Length != behaviors.Length) { Debug.LogError("Data Missmatch in " + name, this); return Vector3.zero; }

        Vector3 move = Vector3.zero;
        for(int i = 0; i < behaviors.Length; i++)
        {
            Vector3 partialMove = behaviors[i].CalculateMove(agent, context, flock) * weights[i];
            if (partialMove != Vector3.zero)
            {
                if(partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }
                move += partialMove;
            }
        }
        move.y = agent.transform.position.y;
        return move;
    }


}
