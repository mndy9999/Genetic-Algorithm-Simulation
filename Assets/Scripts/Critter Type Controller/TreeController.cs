using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour {

    Critter critter;
    public GameObject apple;

    List<Vector3> applePos;

    bool harvested = false;

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
    // Update is called once per frame
    void Update () {
        if(critter.Health <= 10) { critter.isVisible = false; }
        if(!critter.isVisible && harvested && critter.Health < 20) { critter.Health += 0.0001f; } //if the critter's health is low and it is not currently attacked, start regenerating
        if (critter.Health >= 20) { critter.isVisible = true; } //when regenerated enough, make it visible again
        SpawnApples();
    }

    void SpawnApples()
    {   
        //when it's harvested, detach all children
        if (!critter.isVisible && !harvested)
        {
            critter.Health = 5;
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject currentChild = transform.GetChild(i).gameObject;
                currentChild.AddComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
            
            transform.DetachChildren();
            harvested = true;
        }
        //when it's harvestable again, create new children
        else if (critter.isVisible && transform.childCount <= 0)
        {
            for (int i = 0; i < applePos.Count; i++)
            {
                Instantiate(apple, applePos[i], Quaternion.identity, transform);
            }
            harvested = false;
        }

    }
}
