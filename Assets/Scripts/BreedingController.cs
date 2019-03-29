using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BreedingController : MonoBehaviour {

    //hold references to the mother and father
    //these are set directly from the breeding state
    public Critter mother;
    public Critter father;

    GameObject offspring;
    public GameObject[] model;  //holds prefabs of the male and female model for the species
    [Range(0, 100)] public float mutationRate;  //mutation rate can be set from the inspector for each creature/species

    public void CreateOffspring()
    {
        //generate a random position close to the mother
        Vector3 pos = new Vector3(Random.insideUnitSphere.x, 0.0f, Random.insideUnitSphere.z) + transform.position;
        NavMeshHit navHit;
        while (!NavMesh.SamplePosition(pos, out navHit, 0.1f, 1))   //make sure the position is on the navmesh to avoid spawning the offspring off map
        {
            pos = new Vector3(Random.insideUnitSphere.x, 0.0f, Random.insideUnitSphere.z) + transform.position;
        }

        //randomly generate and spawn the corresponding prefab - gender is set on prefab by default (0 for male, 1 for female)
        int rand = Random.Range(0, 2);
        offspring = Instantiate(model[rand], pos, Quaternion.identity);     //spawn offspring at the generated position

        offspring.GetComponent<Critter>().isChild = true;       //make it a child so that its traits and behaviours are not randomly generated
        offspring.GetComponent<Critter>().name = offspring.GetComponent<Critter>().gender.ToString() + " " + offspring.GetComponent<Critter>().critterType;     //name the offspring to keep track of it
    }

    public void BehavioursCrossover()
    {
        offspring.GetComponent<Critter>().availableBehaviours = new List<FiniteStateMachine.State<AI>>();       //create the list
        for (int i = 0; i < Behaviours.allPossibleBehaviours.Count; i++)    //loop through all the possible behaviours
        {
            float rand = (Random.Range(0, 100));    //generate random number
            if (rand < 100-mutationRate/2 && mother.availableBehaviours.Contains(Behaviours.allPossibleBehaviours[i]))  //if it's in the mother's range and the she has the current behaviour
            {
                offspring.GetComponent<Critter>().availableBehaviours.Add(Behaviours.allPossibleBehaviours[i]);     //give the offspring the behaviour
            }
            else if (rand >= 100 - mutationRate / 2 && rand < 100 - mutationRate && father.availableBehaviours.Contains(Behaviours.allPossibleBehaviours[i]))   // f it's in the father's range and he has the current behaviour
            {
                offspring.GetComponent<Critter>().availableBehaviours.Add(Behaviours.allPossibleBehaviours[i]);     //give the offspring the behaviour
            }
            else if(rand >= 100 - mutationRate && rand < 100)   //if it's in the mutation range
            {                
                if (!mother.availableBehaviours.Contains(Behaviours.allPossibleBehaviours[i]) && !father.availableBehaviours.Contains(Behaviours.allPossibleBehaviours[i]))     //if neither of the parents have the behaviour
                {
                    offspring.GetComponent<Critter>().availableBehaviours.Add(Behaviours.allPossibleBehaviours[i]);     //give the offspring the current behaviour
                }
            }
        }
    }

    public void TraitsCrossover()
    {
        offspring.GetComponent<Critter>().critterTraitsDict = new Dictionary<Trait, float>();       //create the list
        foreach (Trait t in System.Enum.GetValues(typeof(Trait)))       //go through each trait
        {
            float rand = (Random.Range(0, 100));        //generate random number
            if (rand < 100 - mutationRate / 2)          //if it is in the mother's range
            {
                offspring.GetComponent<Critter>().critterTraitsDict[t] = mother.GetComponent<Critter>().critterTraitsDict[t];       //get trait value from mother
            }
            else if (rand >= 100 - mutationRate / 2 && rand < 100 - mutationRate)       //if it's in the father's range
            {
                offspring.GetComponent<Critter>().critterTraitsDict[t] = father.GetComponent<Critter>().critterTraitsDict[t];       //get trat value from father
            }
            else if (rand >= 100 - mutationRate && rand < 100)      //if it's in the mutation range
            {
                //generate a random value and assign it to the critter
                if (t != Trait.ViewAngle)
                    offspring.GetComponent<Critter>().critterTraitsDict[t] = Random.Range(0, 10);
                else
                    offspring.GetComponent<Critter>().critterTraitsDict[t] = Random.Range(0, 360);
            }
        }
    }

}
