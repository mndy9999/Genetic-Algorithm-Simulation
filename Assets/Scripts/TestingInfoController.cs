using Boo.Lang.Environments;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TestingInfoController : MonoBehaviour {

    int[] behavioursCount;
    int[] herbivoreBehavioursCount;
    int[] carnivoreBehavioursCount;

	// Use this for initialization
	void Start () {
        behavioursCount = new int[Behaviours.allPossibleBehaviours.Count];
        herbivoreBehavioursCount = new int[Behaviours.allPossibleBehaviours.Count];
        carnivoreBehavioursCount = new int[Behaviours.allPossibleBehaviours.Count];
	}
	
	// Update is called once per frame
	void Update () {
        GetBehavioursAverage();
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
        StreamWriter file = new StreamWriter(@"Assets\Resources\Results.txt", false);
        for (int i = 0; i < Behaviours.allPossibleBehaviours.Count; i++)
        {
            file.WriteLine(Behaviours.allPossibleBehaviours[i].ToString() + "   " + behavioursCount[i]);
        }
        file.Close();
    }

}
