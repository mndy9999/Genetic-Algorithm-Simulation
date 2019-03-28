using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtController : MonoBehaviour {

    Critter critter;
    public GameObject peanut;

    List<Vector3> peanutPos;

    bool harvested = false;

    private void Awake()
    {
        critter = GetComponent<Critter>();
        critter.availableBehaviours = null;
        critter.Energy = 0;
        critter.Health = 20;
        critter.Resource = 1;

        SetupPeanuts();
        peanutPos = new List<Vector3>();
        for (int i = 0; i < transform.childCount; i++)
            peanutPos.Add(transform.GetChild(i).transform.position);

    }

    // Update is called once per frame
    void Update () {
        if (critter.Health <= 10) { critter.isVisible = false; }
        if (!critter.isVisible && harvested && critter.Health < 20) { critter.Health += 0.0001f; }
        if (critter.Health >= 20) { critter.isVisible = true; }
        SpawnPeanuts();
    }

    void SpawnPeanuts()
    {
        if (!critter.isVisible && !harvested)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            transform.DetachChildren();
            harvested = true;
        }

        else if (critter.isVisible && transform.childCount <= 0)
        {
            for (int i = 0; i < peanutPos.Count; i++)
            {
                Instantiate(peanut, peanutPos[i], Quaternion.identity, transform).SetActive(false);
            }
            harvested = false;
        }
    }

    void SetupPeanuts()
    {
        for(int i=0;i<Random.Range(1, 5); i++)
        {
            Instantiate(peanut, transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0.2f, Random.Range(-1.0f, 1.0f)), Quaternion.identity, transform).SetActive(false);
        }
    }

}
