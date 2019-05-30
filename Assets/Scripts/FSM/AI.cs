using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine;

public class AI : MonoBehaviour {

    public bool switchState = false;    //bool used to switch between idle and wander
    public float timer;                 
    public int seconds = 0;
    public Animator animator;           //used to set up the animations
    public Critter critter;
    public Seek seek;
    public SocialRankManager srm;
    public NavMeshAgent agent;

    [SerializeField] string CurrentState;

    //instance of the state machine as auto property
    public StateMachine<AI> stateMachine { get; set; }

    private void Start()
    {
        critter = GetComponent<Critter>();
        seek = GetComponent<Seek>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine = new StateMachine<AI>(this);      //pass the gameobject into the state machine
        stateMachine.ChangeState(AI_Idle.instance);     //set default state to idle
        timer = Time.time;        
    }

    private void Update()
    {
        if(stateMachine.currentState != null) CurrentState = stateMachine.currentState.ToString();
        //timer for changing between the idle and wander states
        if(Time.time > timer + 1)
        {
            timer = Time.time;
            seconds++;
        }
        if(seconds > 3)
        {
            seconds = 0;
            switchState = !switchState;
        }
        stateMachine.Update();      //check for changes in states
    }

    //Roulette Wheel selection for behaviours
    public State<AI> BestState(List<State<AI>> behaviours)
    {
        float sum = 0;      //create initial sum 0
        for (int i = 0; i < behaviours.Count; i++)
        {
            if (critter.availableBehaviours.Contains(behaviours[i]))
            {
                sum += behaviours[i].GetWeight(this);           //add up all the viable behaviours weights
            }
        }
        float r = Random.Range(0, sum);     //generate random number between 0 and initial sum
        float newSum = 0;       //create new sum
        for (int i = 0; i < behaviours.Count; i++)
        {
            if (critter.availableBehaviours.Contains(behaviours[i]))
            {   
                newSum += behaviours[i].GetWeight(this);        //add up all the viable behaviours weights
                if (newSum >= r) { return behaviours[i]; }      //if the new sum is higher than the random number generated, return the last behaviour added
            }
        }
        return null;    //if there's no behaviours, return null
    }



    //events
    public bool CanSeeTarget()
    {
        return seek.Target;
    }

    public bool IsAttacked()
    {
        return critter.IsAttacked;
    }

    public bool IsChallenged()
    {
        return critter.isChallenged;
    }

    public bool CanSeeEnemy()
    {
        return seek.Enemy;
    }

    public bool TargetIsDead()
    {
        return seek.Target.GetComponent<Critter>().Health <= 0;
    }

    public bool TargetIsFood()
    {
        return  seek.Target == seek.Food;
    }

    public bool TargetIsMate()
    {
        return seek.Target == seek.Mate;
    }

    public bool TargetIsPotentialMate()
    {
        return seek.Target == seek.PotentialMate;
    }

    public bool TargetIsEnemy()
    {
        return seek.Target == seek.Enemy;
    }

    public bool TargetIsOpponent()
    {
        return seek.Target == seek.Opponent;
    }

    public bool TargetIsChallenger()
    {
        return seek.Target == seek.Challenger;
    }

    public bool TargetIsCourter()
    {
        return seek.Target == seek.Courter;
    }

    public bool IsCloseEnough()
    {
        return Vector3.Distance(transform.position, seek.Target.transform.position) <= 1.0f;
    }

    public bool IsDead()
    {
        return !critter.IsAlive;
    }

    public bool IsCourted()
    {
        return critter.isCourted;
    }

    public bool InWater()
    {
        return GetComponent<CheckEnvironment>().InWater;
    }

    public bool CanSeeWater()
    {
        return seek.water;
    }

    public string currentState
    {
        get { return CurrentState; }
    }

}
