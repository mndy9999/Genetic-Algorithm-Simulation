using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbivoreController : MonoBehaviour {

    Critter critter;

    private void Awake()
    {
        critter = GetComponent<Critter>();
        critter.availableBehaviours = Behaviours.behaviours;
        critter.availableTargetTypes = Behaviours.herbivoreTargets;
        critter.Energy = 100;
        critter.Health = 100;
        critter.Resource = 100;
    }
    private void Update()
    {
        critter.UpdateStats();
    }
}
