using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialRankManager : MonoBehaviour {

    Critter critter;

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
                Debug.Log(c.name + "   " + c.FitnessScore);
            }
        }
    }

    void CalculateFitness()
    {
        foreach (string critterType in Critter.crittersDict.Keys)
        {
            if (critterType == "Herbivore" || critterType == "Carnivore")
            {
                foreach (Critter c in Critter.crittersDict[critterType])
                {
                    c.FitnessScore = 0;
                    foreach (Trait trait in c.critterTraitsDict.Keys)
                    {
                        c.FitnessScore += c.critterTraitsDict[trait];
                    }
                    c.FitnessScore += c.age;
                    c.FitnessScore += c.availableBehaviours.Count;
                    c.FitnessScore /= (c.critterTraitsDict.Count + 2);

                }
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
                    if (Critter.crittersDict[critterType][j+1].GetComponent<Critter>().FitnessScore > Critter.crittersDict[critterType][j].GetComponent<Critter>().FitnessScore)
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
