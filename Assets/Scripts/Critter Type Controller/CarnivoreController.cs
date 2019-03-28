using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivoreController : MonoBehaviour
{

    Critter critter;

    private void Awake()
    {
        critter = GetComponent<Critter>();
        critter.availableBehaviours = Behaviours.behaviours;
        critter.availableTargetTypes = Behaviours.carnivoreTargets;
        critter.Energy = 100;
        critter.Health = 100;
        critter.Resource = 100;
        if (!critter.isChild) { PopulateAvailableBehaviours(); }
    }

    private void Update()
    {
        critter.UpdateStats();
    }

    private void PopulateAvailableBehaviours()
    {

        critter.availableBehaviours = new List<FiniteStateMachine.State<AI>>();
        for (int i = 0; i < Behaviours.behaviours.Count; i++)
        {
            if (Random.Range(0, 10) < 9)
                critter.availableBehaviours.Add(Behaviours.behaviours[i]);
        }
        for (int i = 0; i < Behaviours.MateEncounterBehaviours.Count; i++)
        {
            if (Random.Range(0, 10) < 5)
                critter.availableBehaviours.Add(Behaviours.MateEncounterBehaviours[i]);
        }
        for (int i = 0; i < Behaviours.SocialRankBehaviours.Count; i++)
        {
            if (Random.Range(0, 10) < 5)
                critter.availableBehaviours.Add(Behaviours.SocialRankBehaviours[i]);
        }
        for (int i = 0; i < Behaviours.ChallengerEncounterBehaviours.Count; i++)
        {
            if (Random.Range(0, 10) < 9)
                critter.availableBehaviours.Add(Behaviours.ChallengerEncounterBehaviours[i]);
        }
        for (int i = 0; i < Behaviours.FoodSourceBehaviours.Count; i++)
        {
            if (Random.Range(0, 10) < 9)
                critter.availableBehaviours.Add(Behaviours.FoodSourceBehaviours[i]);
        }
        critter.availableBehaviours.Add(AI_Flee.instance);
    }
}
