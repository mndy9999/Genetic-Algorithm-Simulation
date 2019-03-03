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
        AI_Knock.name,
        AI_Dig.name,
        AI_Evade.name,
        AI_Breed.name,
        AI_Dead.name
    };

    public static List<string> herbivoreTargets = new List<string>()
    {
        "Vegetable", "Tree"
    };

    public static List<string> carnivoreTargets = new List<string>()
    {
        "Herbivore"
    };
}

