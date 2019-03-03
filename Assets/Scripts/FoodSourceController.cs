using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSourceController : MonoBehaviour{

    Critter critter;
    public GameObject apple;

    bool hasFood;

    private void Awake()
    {
        critter = GetComponent<Critter>();
        critter.availableBehaviours = null;
        critter.Energy = 0;
        critter.Health = 20;
        critter.Resource = 1;
        hasFood = true;
    }

    private void Update()
    {
        hasFood = critter.Health > 10 ? true : false;
        SpawnApples();
    }

    void SpawnApples()
    {
        if (critter.isVisible && !hasFood)
        {
            for (int i = 0; i < 5; i++)
            {
                float offsetX = Random.Range(-1, 1);
                float offsetZ = Random.Range(-1, 1);

                Instantiate(apple, new Vector3(transform.position.x + offsetX, 0.0f, transform.position.z + offsetZ), Quaternion.identity);
            }        
        }
    }
}

