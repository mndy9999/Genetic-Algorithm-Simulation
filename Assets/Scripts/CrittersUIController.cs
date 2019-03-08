using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrittersUIController : MonoBehaviour
{

    public Canvas canvas;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Critters")))
            {
                canvas.gameObject.SetActive(true);
            }
            else { canvas.gameObject.SetActive(false); }
        }
    }
}
