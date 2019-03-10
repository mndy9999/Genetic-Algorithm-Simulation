using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour {

    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

	// Use this for initialization
	void Start () {
        agentCollider = GetComponent<Collider>();
	}
	
	public void Move(Vector3 velocity)
    {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime;
    }
}
