using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CrittersUIController : MonoBehaviour
{
    public GameObject panel;
    public GameObject currentGO;
    public GameObject currentPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Critters")))
            {
                
                if (hit.transform.GetComponent<Critter>())
                    currentGO = hit.transform.gameObject;
                panel.gameObject.SetActive(true);
            }
            else if (!EventSystem.current.IsPointerOverGameObject(-1)) { panel.gameObject.SetActive(false); }
        }
        UpdateInfo();
    }

    void UpdateInfo()
    {
        if (panel && currentGO)
        {
            panel.transform.Find("Name").GetComponent<Text>().text = currentGO.GetComponent<Critter>().Name;
            panel.transform.Find("Age").GetComponent<Text>().text = currentGO.GetComponent<Critter>().Age.ToString();
            panel.transform.Find("Gender").GetComponent<Text>().text = currentGO.GetComponent<Critter>().gender.ToString();
            panel.transform.Find("LifeStage").GetComponent<Text>().text = currentGO.GetComponent<Critter>().lifeStage.ToString();
            if (panel.transform.GetChild(0).gameObject.activeSelf) { infoPanel(); }
            if (panel.transform.GetChild(1).gameObject.activeSelf) { statsPanel(); }
        }
        
        
    }

    void infoPanel()
    {
        currentPanel = panel.transform.GetChild(0).transform.GetChild(0).gameObject;
        currentPanel.transform.Find("Energy").GetComponent<Text>().text = currentGO.GetComponent<Critter>().Energy.ToString();
        currentPanel.transform.Find("Health").GetComponent<Text>().text = ((int)currentGO.GetComponent<Critter>().Health).ToString();
        currentPanel.transform.Find("Resources").GetComponent<Text>().text = currentGO.GetComponent<Critter>().Resource.ToString();
    }

    void statsPanel()
    {
        currentPanel = panel.transform.GetChild(1).transform.GetChild(0).gameObject;
        currentPanel.transform.Find("WalkSpeed").GetComponent<Text>().text = currentGO.GetComponent<Critter>().walkSpeed.ToString();
        currentPanel.transform.Find("RunSpeed").GetComponent<Text>().text = currentGO.GetComponent<Critter>().runSpeed.ToString();
        currentPanel.transform.Find("Speed").GetComponent<Text>().text = currentGO.GetComponent<Critter>().speed.ToString();
        currentPanel.transform.Find("Sight").GetComponent<Text>().text = currentGO.GetComponent<Critter>().viewRadius.ToString();
        currentPanel.transform.Find("SightAngle").GetComponent<Text>().text = currentGO.GetComponent<Critter>().viewAngle.ToString();
    }

}
