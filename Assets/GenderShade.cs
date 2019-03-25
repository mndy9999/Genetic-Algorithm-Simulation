using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenderShade : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (transform.parent.GetComponent<Critter>().gender == Gender.Female)
            GetComponent<SkinnedMeshRenderer>().material.color = new Color32(255, 232,229, 0);
        else
            GetComponent<SkinnedMeshRenderer>().material.color = new Color32(189, 237, 255, 0);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
