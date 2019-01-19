using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : AI_BaseFSM {

    Vector3 waypoint;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        genWaypoint();
	}

	 // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(Vector3.Distance(waypoint, NPC.transform.position) < accuracy)
        {
            genWaypoint();
        }

        var direction = waypoint - NPC.transform.position;
        NPC.transform.rotation = Quaternion.Slerp(NPC.transform.rotation,
                                    Quaternion.LookRotation(direction),
                                    rotSpeed * Time.deltaTime);
        NPC.transform.Translate(0, 0, Time.deltaTime * speed);

    }

    void genWaypoint()
    {
        float x = Random.Range(NPC.transform.position.x + 20, NPC.transform.position.x - 20);
        float y = NPC.transform.position.y;
        float z = Random.Range(NPC.transform.position.z + 20, NPC.transform.position.z - 20);
        waypoint = new Vector3(x, y, z);
    }


	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}

}
