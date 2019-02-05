using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonsHandler : SpawnCreature {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            genSheep();
        }

    }
}
