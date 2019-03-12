using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialRankManager : MonoBehaviour {

    Critter critter;

	// Use this for initialization
	void Start () {
        critter = GetComponent<Critter>();
        CalculateFitness();
    }

    void CalculateFitness()
    {
        critter.fitnessScore = critter.Age + critter.walkSpeed + critter.runSpeed + critter.threatPoints + critter.availableBehaviours.Count + (int)critter.gender + (int)critter.lifeStage;
        critter.fitnessScore /= 7;
    }

}
