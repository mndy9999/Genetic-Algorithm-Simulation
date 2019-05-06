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

    float herbivoreFitness = 0;
    float carnivoreFitness = 0;

    public TimerController time;
    int currentTime;
    int temp;
    int genTemp;

    // Use this for initialization
    void Start () {

        temp = -10;
        genTemp = -60;

       // Debug.unityLogger.logEnabled = false;
        behavioursCount = new int[Behaviours.allPossibleBehaviours.Count];
        herbivoreBehavioursCount = new int[Behaviours.allPossibleBehaviours.Count];
        carnivoreBehavioursCount = new int[Behaviours.allPossibleBehaviours.Count];


        StreamWriter file = new StreamWriter(@"Assets\Resources\Fitness.xls", false);
        file.Write("Generation" + "\t" + "Herbivores" + "\t" + "Carnivores" + "\n");
        file.Close();

        file = new StreamWriter(@"Assets\Resources\Behaviours.xls", false);
        file.Write("Generation" + "\t" + "Herbivores" + "\t" + "Carnivores" + "\n");
        file.Close();

        file = new StreamWriter(@"Assets\Resources\Numbers.xls", false);
        file.Write("Generation" + "\t" + "Herbivores" + "\t" + "Carnivores" + "\n");
        file.Close();

    }
	
	// Update is called once per frame
	void Update () {
        GetBehavioursAverage();
        GetFitnessAverage();

        currentTime = (int)time.timer;

        if (currentTime >= temp + 10)
        {
            OutputResultsEveryFrame();
            temp = currentTime;
            
        }
        if (currentTime >= genTemp + 60)
        {
            OutputResultsAtTimes();
            genTemp = currentTime;
        }
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

    void GetFitnessAverage()
    {
        herbivoreFitness = 0;
        carnivoreFitness = 0;
        foreach(string critterType in Critter.crittersDict.Keys)
        {
            if (critterType == "Herbivore")
            {
                foreach (Critter c in Critter.crittersDict[critterType])
                {
                    herbivoreFitness += c.FitnessScore;                   
                }
                herbivoreFitness /= Critter.crittersDict[critterType].Count;
            }
            if (critterType == "Carnivore")
            {
                foreach (Critter c in Critter.crittersDict[critterType])
                {
                    carnivoreFitness += c.FitnessScore;
                }
                carnivoreFitness /= Critter.crittersDict[critterType].Count;
            }
        }
    }

    void OutputResultsEveryFrame()
    {
        temp = currentTime;

        StreamWriter file = new StreamWriter(@"Assets\Resources\Fitness.xls", true);
        file.WriteLine(currentTime/10 + "\t" + herbivoreFitness + "\t" + carnivoreFitness + "\t" + Critter.crittersDict["Herbivore"][0].FitnessScore + "\t" + Critter.crittersDict["Carnivore"][0].FitnessScore);       
        file.Close();

        file = new StreamWriter(@"Assets\Resources\Numbers.xls", true);
        file.WriteLine(currentTime/10 + "\t" + Critter.crittersDict["Herbivore"].Count + "\t" + Critter.crittersDict["Carnivore"].Count);
        file.Close();        
    }

    void OutputResultsAtTimes()
    {        
        StreamWriter file = new StreamWriter(@"Assets\Resources\Behaviours.xls", true);

        for (int i = 0; i < Behaviours.allPossibleBehaviours.Count; i++)
        {
            file.WriteLine(Behaviours.allPossibleBehaviours[i].ToString() + "\t" + herbivoreBehavioursCount[i] + "\t" + carnivoreBehavioursCount[i]);
        }
        file.WriteLine("Generation" + currentTime/10 + "\n\n");
        file.Close();       
    }

}
