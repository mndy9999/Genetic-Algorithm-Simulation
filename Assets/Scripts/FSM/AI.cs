using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class AI : MonoBehaviour {

    public Vector3 waypoint;

    public bool switchState = false;    //bool used to switch between idle and wander
    public float timer;                 
    public int seconds = 0;
    public Animator animator;           //used to set up the animations
    public Critter critter;

    //instance of the state machine as auto property
    public StateMachine<AI> stateMachine { get; set; }

    private void Start()
    {
        critter = GetComponent<Critter>();
        stateMachine = new StateMachine<AI>(this);      //pass the gameobject into the state machine
        stateMachine.ChangeState(AI_Idle.instance);     //set default state to idle
        timer = Time.time;        
    }

    private void Update()
    {
        //timer for changing between the idle and wander states
        if(Time.time > timer + 1)
        {
            timer = Time.time;
            seconds++;
        }
        if(seconds > 5)
        {
            seconds = 0;
            switchState = !switchState;
        }
        stateMachine.Update();      //check for changes in states
    }

    public void genWaypoint()
    {
        float x = Random.Range(transform.position.x + 180, transform.position.x - 180);
        float y = transform.position.y;
        float z = Random.Range(transform.position.z + 180, transform.position.z - 180);
        waypoint = new Vector3(x, y, z);
    }

    public bool CanSeeTarget()
    {
        if (critter.target)
            return Vector3.Distance(this.transform.position, critter.target.transform.position) < critter.sight;
        else
            return false;
    }

    public bool CanSeeEnemy()
    {
        if (critter.enemy)
            return Vector3.Distance(this.transform.position, critter.enemy.transform.position) < critter.sight;
        else
            return false;
    }

    public bool IsCloseEnoughToEat()
    {
        if (critter.target)
            return Vector3.Distance(this.transform.position, critter.target.transform.position) < 0.3f;
        else
            return false;
    }

}
