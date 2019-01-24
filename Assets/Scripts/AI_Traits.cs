using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Traits : MonoBehaviour {

    public int sight = 10;
    public GameObject foodSource;
    public GameObject target;

    private void Update()
    {
        findTarget();
        sight = 10;
    }

    void findTarget()
    {
        float temp = 0;
        float distance = 999;
        for(int i = 0; i < foodSource.transform.childCount; i++)
        {
            temp = Vector3.Distance(transform.position, foodSource.transform.GetChild(i).position);
            if (temp < distance)
            {
                distance = temp;
                target = foodSource.transform.GetChild(i).gameObject;
            }
        }
    }
	
}
