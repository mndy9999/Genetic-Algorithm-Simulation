using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnvironment : MonoBehaviour {

    //bool to keep track if the critter is in the water
    [SerializeField] bool inWater = false;
    public bool InWater { get { return inWater; } }

    private void OnTriggerEnter(Collider col)
    {
        //if the critter's collider is touching an object from the Water layer
        if(col.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            //set it to true
            inWater = true;
        }
    }
}
