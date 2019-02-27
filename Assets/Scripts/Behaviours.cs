using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class Behaviours
{
    public static List<string> behaviours = new List<string>()
    {
        AI_Idle.name, 
        AI_Wander.name,
        AI_Chase.name,
        AI_Attack.name,
        AI_Eat.name,
        AI_Evade.name,
        AI_Breed.name,
        AI_Dead.name
    };

}

