using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    Critter critter;
    public GameObject apple;


	// Use this for initialization
	void Awake () {
        critter = GetComponent<Critter>();
        critter.critterType = "Tree";
	}
	
	// Update is called once per frame
	void Update () {
        if (critter.health > 10) { critter.isVisible = true; }
        if (critter.health <= 0) {
            SpawnApples();
        }
        
	}

    void SpawnApples()
    {
       
        if (critter.isVisible)
        {
            for (int i = 0; i < 5; i++)
            {
                float offsetX = Random.Range(-1, 1);
                float offsetZ = Random.Range(-1, 1);
                
                Instantiate(apple, new Vector3(transform.position.x + offsetX, 0.0f, transform.position.z + offsetZ), Quaternion.identity);
            }
            critter.isVisible = false;
        }
    }


}
