using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCreature : MonoBehaviour {

    public GameObject sheep;

    public void genSheep()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag != "Tree")
            {
                Instantiate(sheep, hit.point, Quaternion.identity);
            }

        }
    }
}
