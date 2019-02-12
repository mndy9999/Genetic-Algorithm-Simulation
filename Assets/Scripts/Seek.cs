using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour {

    public string targetType = "Vegetable";
    public string enemyType = "Carnivore";

    public GameObject Target
    {
        get
        {
            if (Critter.crittersDict.ContainsKey(targetType))
            {
                //find closest target
                GameObject target = null;
                float dist = Mathf.Infinity;
                foreach (Critter c in Critter.crittersDict[targetType])
                {
                    float d = Vector3.Distance(this.transform.position, c.transform.position);
                    if (target == null || d < dist)
                    {
                        target = c.gameObject;
                        dist = d;
                    }
                }
                return target;
            }
            return null;
        }
    }

    public GameObject Enemy
    {
        get
        {
            if (Critter.crittersDict.ContainsKey(enemyType))
            {
                //find closest enemy
                GameObject enemy = null;
                float dist = Mathf.Infinity;
                foreach (Critter c in Critter.crittersDict[enemyType])
                {
                    float d = Vector3.Distance(this.transform.position, c.transform.position);
                    if (enemy == null || d < dist)
                    {
                        enemy = c.gameObject;
                        dist = d;
                    }
                }
                return enemy;
            }
            return null;
        }
    }

}
