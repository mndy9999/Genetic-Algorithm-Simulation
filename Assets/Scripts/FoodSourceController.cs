using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSourceController : MonoBehaviour{

    Critter critter;
    public GameObject apple;

    List <Vector3> applePos;
    bool appled = false;

    private void Awake()
    {
        critter = GetComponent<Critter>();
        critter.availableBehaviours = null;
        critter.Energy = 0;
        critter.Health = 20;
        critter.Resource = 1;
        applePos = new List<Vector3>();
        for (int i = 0; i < transform.childCount; i++)
            applePos.Add(transform.GetChild(i).transform.position);
    }

    private void Update()
    {
        critter.isVisible = critter.Health > 10;
        if (critter.Health > 15) { appled = false; }
        SpawnApples();
    }

    void SpawnApples()
    {
        if (appled) return;

        if (!critter.isVisible)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject currentChild = transform.GetChild(i).gameObject;
                Destroy(currentChild);

                float offsetX = Random.Range(-1.0f, 1.0f);
                float offsetZ = Random.Range(-1.0f, 1.0f);


                //currentChild.transform.Translate(new Vector3(currentChild.transform.position.x, 0.0f, currentChild.transform.position.z));

                Instantiate(apple, new Vector3(transform.position.x + offsetX, 0.0f, transform.position.z + offsetZ), Quaternion.identity);
            }
            appled = true;
        }

        else if(transform.childCount <= 0)
        {
            for(int i=0;i < applePos.Count; i++)
            {
                Instantiate(apple, applePos[i], Quaternion.identity, transform);
            }
            
        }
        
    }
}

