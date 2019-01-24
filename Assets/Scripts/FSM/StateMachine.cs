using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created as a namespace so it can be used in any script
namespace FiniteStateMachine {
    public class StateMachine<T>{
        public State <T> currentState { get; private set; }     //get the state anywhere, but only set it from withing this class
        public T owner;     //the AI object

        public StateMachine(T _o)
        {
            owner = _o;
            currentState = null;
        }

        public void ChangeState(State<T> _newstate)
        {
            //if there is a current state
            if(currentState != null)
                currentState.ExitState(owner);  //exit it
            currentState = _newstate;           //set current state to the desired one
            currentState.EnterState(owner);     //enter new state
        }

        public void Update()
        {
            //if there is a current state
            if (currentState != null)
                currentState.UpdateState(owner);    //update it
        }
    }

    //State class template
	public abstract class State<T>
    {
        public abstract void EnterState(T _owner);        
        public abstract void UpdateState(T _owner);
        public abstract void ExitState(T _owner);
    }
}
