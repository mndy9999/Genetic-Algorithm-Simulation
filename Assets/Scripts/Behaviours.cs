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
        AI_Swim.name,
        AI_Threat.name,
        AI_Startle.name,
        AI_PlayDead.name,
        AI_Alarm.name,
        AI_Breed.name,
        AI_Getup.name,
        AI_Laydown.name,
        AI_Sleep.name,
        AI_Rest.name,
        AI_Watch.name,
        AI_Dead.name
    };

    public static List<string> EnemyEncounterBehaviours = new List<string>()
    {
        AI_Alarm.name,
        AI_PlayDead.name,
        AI_Startle.name,
        AI_Suicide.name,
        AI_Flock.name
    };

    public static List<string> MateEncounterBehaviours = new List<string>()
    {
        AI_CallMate.name,
        AI_Impress.name,
        AI_PairUp.name,

    };

    public static List<string> herbivoreTargets = new List<string>()
    {
        "Vegetable", "Tree", "Dirt"
    };

    public static List<string> carnivoreTargets = new List<string>()
    {
        "Herbivore"
    };
}

