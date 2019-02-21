using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Evade : MonoBehaviour {

    public string critterType = "Carnivore";
    Critter myCritter;
	// Use this for initialization
	void Start () {
        myCritter = this.GetComponent<Critter>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DoAIBehaviour()
    {
        if (!Critter.crittersDict.ContainsKey(critterType)) { return; }

        //find closest target
        Critter closest = null;
        float dist = Mathf.Infinity;
        foreach (Critter c in Critter.crittersDict[critterType])
        {
            float d = Vector3.Distance(this.transform.position, c.transform.position);
            if (closest == null || d < dist)
            {
                closest = c;
                dist = d;
            }
        }

        //there is no target
        if (closest == null) { return; }
        if(dist < 1f) { return; }

        float weight = 100 / (dist * dist);

        Vector3 dir = closest.transform.position - this.transform.position;
        dir *= -1;

    }
}
