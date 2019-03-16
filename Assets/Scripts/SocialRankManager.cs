using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialRankManager : MonoBehaviour {

    Critter critter;

	// Use this for initialization
	void Start () {
        CalculateFitness();
        UpdateRanks();
    }

    private void Update()
    {
        CalculateFitness();
        UpdateRanks();
        //DebugRanks();
    }

    void DebugRanks()
    {
        foreach (string critterType in Critter.crittersDict.Keys)
        {
            Debug.Log(critterType);
            foreach (Critter c in Critter.crittersDict[critterType])
            {
                Debug.Log(c.name + "   " + c.fitnessScore);
            }
        }
    }

    void CalculateFitness()
    {      
        foreach (string critterType in Critter.crittersDict.Keys)
        {
            foreach (Critter c in Critter.crittersDict[critterType])
            {
                critter = c.GetComponent<Critter>();
                critter.fitnessScore = 0;
                foreach (Critter.Trait trait in c.critterTraitsDict.Keys)
                {
                    critter.fitnessScore += c.critterTraitsDict[trait];
                }
                critter.fitnessScore += critter.Age;
                critter.fitnessScore /= (c.critterTraitsDict.Count+1);
            }
        }
    }

    void UpdateRanks()
    {
        foreach (string critterType in Critter.crittersDict.Keys) {
            Critter temp;
            for (int i = 0; i < Critter.crittersDict[critterType].Count; i++)
            {
                for (int j = 0; j < Critter.crittersDict[critterType].Count - 1; j++)
                {
                    if (Critter.crittersDict[critterType][j+1].GetComponent<Critter>().fitnessScore > Critter.crittersDict[critterType][j].GetComponent<Critter>().fitnessScore)
                    {
                        temp = Critter.crittersDict[critterType][j];
                        Critter.crittersDict[critterType][j] = Critter.crittersDict[critterType][j + 1];
                        Critter.crittersDict[critterType][j + 1] = temp;
                    }
            }
            }
        }
    }
    
}
