using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbivoreController : MonoBehaviour {

    Critter critter;

    private void Awake()
    {
        critter = GetComponent<Critter>();
        critter.availableTargetTypes = Behaviours.herbivoreTargets;
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
        for (int i = 0; i < Behaviours.EnemyEncounterBehaviours.Count; i++)
        {
            if (Random.Range(0, 10) < 9)
                critter.availableBehaviours.Add(Behaviours.EnemyEncounterBehaviours[i]);
        }
        for (int i = 0; i < Behaviours.MateEncounterBehaviours.Count; i++)
        {
            if (Random.Range(0, 10) < 9)
                critter.availableBehaviours.Add(Behaviours.MateEncounterBehaviours[i]);
        }
        for (int i = 0; i < Behaviours.SocialRankBehaviours.Count; i++)
        {
            if (Random.Range(0, 10) < 9)
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
    }
}
