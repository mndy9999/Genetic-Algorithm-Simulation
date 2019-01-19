using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Script : MonoBehaviour {

    Animator anim;
    public GameObject player;
    float foodLevel = 0; 

    public GameObject GetPlayer()
    {
        return player;
    }

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (player) 
        anim.SetFloat("distance", Vector3.Distance(transform.position, player.transform.position));
        Debug.Log(foodLevel);
    }

    void eat()
    {
        foodLevel++;
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
