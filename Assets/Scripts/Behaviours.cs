using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class Behaviours
{
    //hold all possible behaviours kept in a single array for easy looping through
    public static List<State<AI>> allPossibleBehaviours = new List<State<AI>>()
    {
        AI_Idle.instance,
        AI_Wander.instance,
        AI_Chase.instance,
        AI_Attack.instance,
        AI_Eat.instance,
        AI_Swim.instance,
        AI_Breed.instance,
        AI_Getup.instance,
        AI_Laydown.instance,
        AI_Dead.instance,
        AI_Threat.instance,
        AI_Aggress.instance,
        AI_Fight.instance,
        AI_Sleep.instance,
        AI_Rest.instance,
        AI_Knock.instance,
        AI_Dig.instance,
        AI_Alarm.instance,
        AI_PlayDead.instance,
        AI_Startle.instance,
        AI_Flee.instance,
        AI_CallMate.instance,
        AI_Impress.instance,
        AI_Watch.instance,
        AI_Submit.instance
    };

    //basic behaviours
    public static List<State<AI>> behaviours = new List<State<AI>>()
    {
        AI_Idle.instance, 
        AI_Wander.instance,
        AI_Chase.instance,
        AI_Attack.instance,
        AI_Eat.instance,
        AI_Breed.instance,
        AI_Getup.instance,
        AI_Laydown.instance,
        AI_Dead.instance
    };

    public static List<State<AI>> SocialRankBehaviours = new List<State<AI>>()
    {
        AI_Threat.instance,
        AI_Aggress.instance,
        AI_Fight.instance
    };

    public static List<State<AI>> LowEnergyBehaviours = new List<State<AI>>()
    {
        AI_Sleep.instance,
        AI_Rest.instance
    };

    public static List<State<AI>> FoodSourceBehaviours = new List<State<AI>>()
    {
        AI_Knock.instance,
        AI_Dig.instance
        //AI_Fish.name
    };

    public static List<State<AI>> EnemyEncounterBehaviours = new List<State<AI>>()
    {
        AI_Alarm.instance,
        AI_PlayDead.instance,
        AI_Startle.instance,
        AI_Flee.instance,
        AI_Swim.instance
        //AI_Suicide.name,
        //AI_Flock.name
    };

    public static List<State<AI>> MateEncounterBehaviours = new List<State<AI>>()
    {
        AI_CallMate.instance,
        AI_Impress.instance
        //AI_PairUp.name
    };

    public static List<State<AI>> ChallengerEncounterBehaviours = new List<State<AI>>()
    {
        AI_Watch.instance,
        AI_Submit.instance
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

