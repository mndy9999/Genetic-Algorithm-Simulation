using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnvironment : MonoBehaviour {

    bool inWater = false;
    public bool InWater { get { return inWater; } }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            inWater = true;
        }
    }
}
