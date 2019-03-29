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

    //output the fitness score of each creature to the debugger
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
            if (critterType == "Herbivore" || critterType == "Carnivore")   //go through all the carnivores and herbivores in the dictionary
            {
                foreach (Critter c in Critter.crittersDict[critterType])    //for each creature in the species
                {
                    c.FitnessScore = 0;     //reset fitness score
                    foreach (Trait trait in c.critterTraitsDict.Keys)
                    {
                        c.FitnessScore += c.critterTraitsDict[trait];   //add up all the traits values
                    }
                    c.FitnessScore += c.age;                            //and the age
                    c.FitnessScore += c.availableBehaviours.Count;      //and the number of available behaviours
                    c.FitnessScore /= (c.critterTraitsDict.Count + 2);  //then average the value to get the fitness score

                }
            }
        }
    }
    
    //bubble sort algorithm to sort the dictionary based on the critter's fitness score
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
