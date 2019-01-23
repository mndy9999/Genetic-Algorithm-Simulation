using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class AI : MonoBehaviour {

    public bool switchState = false;
    public float timer;
    public int seconds = 0;
    public Animator animator;
    public AI_Traits traits;

    public StateMachine<AI> stateMachine { get; set; }

    private void Start()
    {
        traits = GetComponent<AI_Traits>();
        stateMachine = new StateMachine<AI>(this);
        stateMachine.ChangeState(AI_Idle.instance);
        timer = Time.time;        
    }

    private void Update()
    {
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

        stateMachine.Update();
    }

}
