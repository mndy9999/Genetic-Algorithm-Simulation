using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSourceController : MonoBehaviour{

    Critter critter;

    private void Awake()
    {
        critter = GetComponent<Critter>();
        critter.availableBehaviours = null;
        critter.Energy = 0;
        critter.Health = 20;
        critter.Resource = 1;
    }
}

