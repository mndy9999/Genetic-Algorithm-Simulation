using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Script : MonoBehaviour {

    Animator anim;
    public GameObject foodSource;
    public GameObject target;
    float foodLevel = 0; 

    public GameObject GetTarget()
    {
        return target;
    }

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!target) { anim.SetBool("foundFood", false); setTarget(); Debug.Log("hi"); }
        if (Vector3.Distance(transform.position, target.transform.position) < 10) { anim.SetBool("foundFood", true); }
        anim.SetFloat("distance", Vector3.Distance(transform.position, target.transform.position));
    }

    void setTarget()
    {
        float temp = 999;
        GameObject tempTarget;
        for (int i = 0; i < foodSource.transform.childCount; i++)
        {
            tempTarget = foodSource.transform.GetChild(i).gameObject;
            if(Vector3.Distance(transform.position, tempTarget.transform.position) < temp)
            {
                temp = Vector3.Distance(transform.position, tempTarget.transform.position);
                target = tempTarget;
            }
        }
    }

    void eat()
    {
        foodLevel++;
        target.GetComponent<food>().level--;
        anim.SetFloat("foodLevel", foodLevel);
    }

    public void startEating()
    {
        InvokeRepeating("eat", 0.5f, 0.5f);
    }
    public void stopEating()
    {
        CancelInvoke("eat");
    }
}
