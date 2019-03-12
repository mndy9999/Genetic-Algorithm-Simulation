using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingController : MonoBehaviour {

    public Critter mother;
    public Critter father;

    GameObject offspring;
    public GameObject model;

    public void CreateOffspring()
    {
        Debug.Log("creating offspring");
        Vector3 pos = new Vector3(Random.insideUnitSphere.x, 0.0f, Random.insideUnitSphere.z) + transform.position;
        offspring = Instantiate(gameObject, pos, Quaternion.identity);
        offspring.GetComponent<Critter>().isChild = true;
    }

    public void Crossover()
    {
        for (int i = 0; i < Behaviours.behaviours.Count; i++)
        {
            float rand = (Random.Range(0, 130));
            if (rand < 50 && mother.availableBehaviours.Contains(Behaviours.behaviours[i]))
            {
                offspring.GetComponent<Critter>().availableBehaviours.Add(Behaviours.behaviours[i]);
            }
            else if (rand >= 50 && rand < 100 && father.availableBehaviours.Contains(Behaviours.behaviours[i]))
            {
                offspring.GetComponent<Critter>().availableBehaviours.Add(Behaviours.behaviours[i]);
            }
            else if(rand >=100 && rand < 130)
            {
                if (!mother.availableBehaviours.Contains(Behaviours.behaviours[i]) && !father.availableBehaviours.Contains(Behaviours.behaviours[i]))
                {
                    offspring.GetComponent<Critter>().availableBehaviours.Add(Behaviours.behaviours[i]);
                }
            }
        }
    }

}
