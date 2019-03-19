using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        int rand = Random.Range(0, 1);
        offspring = Instantiate(model[rand], pos, Quaternion.identity);

        offspring.GetComponent<Critter>().isChild = true;
        offspring.GetComponent<Critter>().name = offspring.GetComponent<Critter>().gender.ToString() + " Sheep";
    }

    public void Crossover()
    {
        for (int i = 0; i < Behaviours.behaviours.Count; i++)
        {
            float rand = (Random.Range(0, 130));
            if (rand < 50 && mother.availableBehaviours.Contains(Behaviours.behaviours[i]))
            {
                Debug.Log("mother");
                offspring.GetComponent<Critter>().availableBehaviours.Add(Behaviours.behaviours[i]);
            }
            else if (rand >= 50 && rand < 100 && father.availableBehaviours.Contains(Behaviours.behaviours[i]))
            {
                Debug.Log("father");
                offspring.GetComponent<Critter>().availableBehaviours.Add(Behaviours.behaviours[i]);
            }
            else if(rand >=100 && rand < 130)
            {                
                if (!mother.availableBehaviours.Contains(Behaviours.behaviours[i]) && !father.availableBehaviours.Contains(Behaviours.behaviours[i]))
                {
                    Debug.Log("mutation");
                    offspring.GetComponent<Critter>().availableBehaviours.Add(Behaviours.behaviours[i]);
                }
            }
        }
    }

}
