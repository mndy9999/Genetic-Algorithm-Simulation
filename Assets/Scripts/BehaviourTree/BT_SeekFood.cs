using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_SeekFood : MonoBehaviour {

    public string critterType = "Vegetable";

    public float eatingRange = 1.0f;
    public float eatHPPerSecond = 5f;
    public float eatHPToEnergy = 2f;

    Critter myCritter;

    private void Start()
    {
        myCritter = GetComponent<Critter>();
    }

    void DoAIBehaviour()
    {
        if (!Critter.crittersDict.ContainsKey(critterType)){ return; }

        //find closest target
        Critter closest = null;
        float dist = Mathf.Infinity;
        foreach(Critter c in Critter.crittersDict[critterType])
        {
            float d = Vector3.Distance(this.transform.position, c.transform.position);
            if(closest == null || d < dist)
            {
                closest = c;
                dist = d;
            }
        }

        //there is no target
        if (closest == null) { return; }

        if (dist < eatingRange) {
            float hpEaten = Mathf.Clamp(eatHPPerSecond, 0, closest.health) * Time.deltaTime;
            closest.health -= hpEaten;
            myCritter.energy += hpEaten*eatHPToEnergy;
        }
        else
        {
            Vector3 dir = closest.transform.position - this.transform.position;

        }
        
        
    }
}
