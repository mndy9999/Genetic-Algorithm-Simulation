using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivoreController : MonoBehaviour {

    Critter critter;

    private void Awake()
    {
        critter = GetComponent<Critter>();
        critter.availableBehaviours = Behaviours.behaviours;
        critter.availableTargetTypes = Behaviours.carnivoreTargets;
        critter.Energy = 100;
        critter.Health = 100;
        critter.Resource = 100;
    }
}
