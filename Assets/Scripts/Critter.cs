using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter : MonoBehaviour {

    public float health = 100f;
    public float energy = 100f;

    public float energyPerSecond = 1f;

    public float runSpeed = 5f;
    public float walkSpeed = 2f;
    public float speed;

    public string critterType = "Vegetable";

    static public Dictionary<string, List<Critter>> crittersDict;

    public List<WeightedDirection> desiredDirections;

    Animator animator;



    // Use this for initialization
    void Awake () {
		if(crittersDict == null) { crittersDict = new Dictionary<string, List<Critter>>(); }
        if(!crittersDict.ContainsKey(critterType)) { crittersDict[critterType] = new List<Critter>(); }
        crittersDict[critterType].Add(this);
        animator = this.GetComponent<Animator>();
        speed = runSpeed;
	}

    private void OnDestroy()
    {       
        crittersDict[critterType].Remove(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        energy = Mathf.Clamp(energy - Time.deltaTime * energyPerSecond, 0, 100);
        if (energy <= 0) { health = Mathf.Clamp(health - Time.deltaTime, 0, 100); }
        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        if (energy > 10) { speed = runSpeed; }
        desiredDirections = new List<WeightedDirection>();
    }

    void changeSpeed()
    {
        if (energy < 10)
        {
            while (speed > walkSpeed) { speed -= 0.2f; }
        }
    }
    public bool IsAlive() { return health > 0; }
    public bool IsAttacked { get; set; }

}
