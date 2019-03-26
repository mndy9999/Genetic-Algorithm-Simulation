using Boo.Lang.Environments;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TestingInfoController : MonoBehaviour {

    int[] behavioursCount;
    int[] herbivoreBehavioursCount;
    int[] carnivoreBehavioursCount;

    public TimerController time;

	// Use this for initialization
	void Start () {
        behavioursCount = new int[Behaviours.allPossibleBehaviours.Count];
        herbivoreBehavioursCount = new int[Behaviours.allPossibleBehaviours.Count];
        carnivoreBehavioursCount = new int[Behaviours.allPossibleBehaviours.Count];
    }
	
	// Update is called once per frame
	void Update () {
        GetBehavioursAverage();
        if((int)time.timer == 1 || (int)time.timer == 60 || (int)time.timer == 180 || (int)time.timer == 300)
            OutputResults();
    }

    void GetBehavioursAverage()
    {
        behavioursCount = new int[Behaviours.allPossibleBehaviours.Count];
        herbivoreBehavioursCount = new int[Behaviours.allPossibleBehaviours.Count];
        carnivoreBehavioursCount = new int[Behaviours.allPossibleBehaviours.Count];
        foreach (string t in Critter.crittersDict.Keys)
        {
            foreach (Critter c in Critter.crittersDict[t])
            {
                for (int i = 0; i < Behaviours.allPossibleBehaviours.Count; i++)
                {
                    if (c.availableBehaviours != null)
                    {
                        if (c.availableBehaviours.Contains(Behaviours.allPossibleBehaviours[i]))
                        {
                            behavioursCount[i]++;
                            if (t == "Herbivore") { herbivoreBehavioursCount[i]++; }
                            if (t == "Carnivore") { carnivoreBehavioursCount[i]++; }
                        }
                    }
                    
                }
            }
        }
    }


    void OutputResults()
    {
        StreamWriter file = new StreamWriter(@"Assets\Resources\Results"+(int)time.timer+".xls", false);


        file.WriteLine("Total herbivores: " + "\t" + Critter.crittersDict["Herbivore"].Count);
        for (int i = 0; i < Behaviours.allPossibleBehaviours.Count; i++)
        {
            file.WriteLine(Behaviours.allPossibleBehaviours[i].ToString() + "\t" + herbivoreBehavioursCount[i]);
        }


        file.WriteLine("\n\n");


        file.WriteLine("Total carnivores: " + "\t" + Critter.crittersDict["Carnivore"].Count);
        for (int i = 0; i < Behaviours.allPossibleBehaviours.Count; i++)
        {
            file.WriteLine(Behaviours.allPossibleBehaviours[i].ToString() + "\t" + carnivoreBehavioursCount[i]);
        }


        file.WriteLine("\n\n");

        int totalCreatures = Critter.crittersDict["Herbivore"].Count + Critter.crittersDict["Carnivore"].Count;
        file.WriteLine("Total creatures: " + "\t" + totalCreatures);
        for (int i = 0; i < Behaviours.allPossibleBehaviours.Count; i++)
        {
            file.WriteLine(Behaviours.allPossibleBehaviours[i].ToString() + "\t" + behavioursCount[i]);
        }


        file.Close();
    }

}
