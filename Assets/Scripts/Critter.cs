using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter : MonoBehaviour {

    public float health = 100f;
    public float energy = 100f;

    public float energyPerSecond = 5f;

    public float runSpeed = 5f;
    public float walkSpeed = 2f;
    public float speed;
    public float sight = 10f;

    public string critterType = "Vegetable";
    public string targetType = "Vegetable";
    public string enemyType = "Carnivore";

    public GameObject target;
    public GameObject enemy;

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
        FindClosestTarget();
        FindClosestEnemy();
        energy = Mathf.Clamp(energy - Time.deltaTime * energyPerSecond, 0, 100);
        if (energy <= 0) { health = Mathf.Clamp(health - Time.deltaTime * 5f, 0, 100); }
        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        if (energy > 10) { speed = runSpeed; }
        desiredDirections = new List<WeightedDirection>();
    }

    void FindClosestTarget()
    {
        if (crittersDict.ContainsKey(targetType))
        {
            //find closest target
            target = null;
            float dist = Mathf.Infinity;
            foreach (Critter c in Critter.crittersDict[targetType])
            {
                float d = Vector3.Distance(this.transform.position, c.transform.position);
                if (target == null || d < dist)
                {
                    target = c.gameObject;
                    dist = d;
                }
            }
        }
    }

    void FindClosestEnemy()
    {
        if (crittersDict.ContainsKey(enemyType))
        {
            //find closest enemy
            enemy = null;
            float dist = Mathf.Infinity;
            foreach (Critter c in Critter.crittersDict[enemyType])
            {
                float d = Vector3.Distance(this.transform.position, c.transform.position);
                if (enemy == null || d < dist)
                {
                    enemy = c.gameObject;
                    dist = d;
                }
            }
        }
    }

    void changeSpeed()
    {
        if (energy < 10)
        {
            while (speed > walkSpeed) { speed -= 0.2f; }
        }
    }

    private void OnDrawGizmos()
    {
        if(critterType != "Vegetable")
            Gizmos.DrawWireCube(transform.position, new Vector3(sight, 0.0f, sight));
    }

}
