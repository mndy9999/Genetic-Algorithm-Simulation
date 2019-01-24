using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class AI : MonoBehaviour {

    public bool switchState = false;    //bool used to switch between idle and wander
    public float timer;                 
    public int seconds = 0;
    public Animator animator;           //used to set up the animations
    public AI_Traits traits;            //the traits and characteristics of the individual

    //instance of the state machine as auto property
    public StateMachine<AI> stateMachine { get; set; }

    private void Start()
    {
        traits = GetComponent<AI_Traits>();
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

}
