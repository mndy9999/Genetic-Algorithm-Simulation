using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCreature : MonoBehaviour {

    public GameObject[] objects;

    public InventoryButtonsController buttons;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "ground")
                {
                    for (int i = 0; i < objects.Length; i++)
                    {
                        if (buttons.GetActiveButton == buttons.buttons[i]) { Instantiate(objects[i], hit.point, Quaternion.identity); }
                    }
                }
            }
        }
    }



}
