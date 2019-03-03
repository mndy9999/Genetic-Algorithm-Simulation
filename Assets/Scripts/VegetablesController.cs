using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetablesController : MonoBehaviour {

    Critter critter;
   
    private void Awake()
    {
        critter = GetComponent<Critter>();
        critter.availableBehaviours = null;
        critter.Energy = 0;
        critter.Health = 0;
        critter.Resource = 20;
    }
}
