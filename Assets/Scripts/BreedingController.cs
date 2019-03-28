using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BreedingController : MonoBehaviour {

    public Critter mother;
    public Critter father;

    GameObject offspring;
    public GameObject[] model;
    [Range(0, 100)] public float mutationRate;

    public void CreateOffspring()
    {
        Debug.Log("creating offspring");
        Vector3 pos = new Vector3(Random.insideUnitSphere.x, 0.0f, Random.insideUnitSphere.z) + transform.position;
        NavMeshHit navHit;
        while (!NavMesh.SamplePosition(pos, out navHit, 1, 1))
        {
            pos = new Vector3(Random.insideUnitSphere.x, 0.0f, Random.insideUnitSphere.z) + transform.position;
        }

        int rand = Random.Range(0, 2);
        offspring = Instantiate(model[rand], pos, Quaternion.identity);

        offspring.GetComponent<Critter>().isChild = true;
        offspring.GetComponent<Critter>().name = offspring.GetComponent<Critter>().gender.ToString() + " " + offspring.GetComponent<Critter>().critterType;
    }

    public void BehavioursCrossover()
    {
        offspring.GetComponent<Critter>().availableBehaviours = new List<FiniteStateMachine.State<AI>>();
        for (int i = 0; i < Behaviours.allPossibleBehaviours.Count; i++)
        {
            float rand = (Random.Range(0, 100));
            if (rand < 100-mutationRate/2 && mother.availableBehaviours.Contains(Behaviours.allPossibleBehaviours[i]))
            {
                Debug.Log("mother");
                offspring.GetComponent<Critter>().availableBehaviours.Add(Behaviours.allPossibleBehaviours[i]);
            }
            else if (rand >= 100 - mutationRate / 2 && rand < 100 - mutationRate && father.availableBehaviours.Contains(Behaviours.allPossibleBehaviours[i]))
            {
                Debug.Log("father");
                offspring.GetComponent<Critter>().availableBehaviours.Add(Behaviours.allPossibleBehaviours[i]);
            }
            else if(rand >= 100 - mutationRate && rand < 100)
            {                
                if (!mother.availableBehaviours.Contains(Behaviours.allPossibleBehaviours[i]) && !father.availableBehaviours.Contains(Behaviours.allPossibleBehaviours[i]))
                {
                    Debug.Log("mutation");
                    offspring.GetComponent<Critter>().availableBehaviours.Add(Behaviours.allPossibleBehaviours[i]);
                }
            }
        }
    }

    public void TraitsCrossover()
    {
        offspring.GetComponent<Critter>().critterTraitsDict = new Dictionary<Trait, float>();
        foreach (Trait t in System.Enum.GetValues(typeof(Trait)))
        {
            float rand = (Random.Range(0, 100));
            if (rand < 100 - mutationRate / 2)
            {
                Debug.Log("mother");
                offspring.GetComponent<Critter>().critterTraitsDict[t] = mother.GetComponent<Critter>().critterTraitsDict[t];
            }
            else if (rand >= 100 - mutationRate / 2 && rand < 100 - mutationRate)
            {
                Debug.Log("father");
                offspring.GetComponent<Critter>().critterTraitsDict[t] = father.GetComponent<Critter>().critterTraitsDict[t];
            }
            else if (rand >= 100 - mutationRate && rand < 100)
            {
                Debug.Log("mutation");
                if (t != Trait.ViewAngle)
                    offspring.GetComponent<Critter>().critterTraitsDict[t] = Random.Range(0, 10);
                else
                    offspring.GetComponent<Critter>().critterTraitsDict[t] = Random.Range(0, 360);
            }
        }
    }

}
