using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter : MonoBehaviour {

    public float health = 100f;
    public float energy = 100f;
    public float resource = 100f;

    public float age = 0;

    public float energyPerSecond = 1f;

    public float runSpeed = 5f;
    public float walkSpeed = 2f;
    public float speed;

    public string critterType = "Vegetable";

    static public Dictionary<string, List<Critter>> crittersDict;

    public List<WeightedDirection> desiredDirections;

    Animator animator;

    public Vector3 initialSize = new Vector3(0.2f, 0.2f, 0.2f);
    public enum Stage { Baby, Teen, Adult, Elder};
    public Stage lifeStage = Stage.Baby;

    public bool breed;

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
        UpdateLifeStage();
        energy = Mathf.Clamp(energy - Time.deltaTime * energyPerSecond, 0, 100);
        if (energy <= 0) { health = Mathf.Clamp(health - Time.deltaTime, 0, 100); }
        if (!IsAlive()) { energy = 0; health = 0; resource = Mathf.Clamp(resource - Time.deltaTime, 0, 100); }
        if (resource <= 0)
        {
            Destroy(gameObject);
            return;
        }
        if (energy > 10) { speed = runSpeed; }
        desiredDirections = new List<WeightedDirection>();
        breed = CanBreed();
    }

    void changeSpeed()
    {
        if (energy < 10)
        {
            while (speed > walkSpeed) { speed -= 0.2f; }
        }
    }
    public bool IsAlive() { return health > 0 && age < 15; }
    public bool CanBreed() { return lifeStage >= Stage.Teen && lifeStage < Stage.Elder; }
    public bool IsAttacked;
    public void KillSelf() { Destroy(gameObject); }
    void UpdateLifeStage()
    {
        if(age < 3) { lifeStage = Stage.Baby; }
        else if(age < 6 && age > 2) { lifeStage = Stage.Teen; }
        else if(age < 10 && age > 5) { lifeStage = Stage.Adult; }
        else { lifeStage = Stage.Elder; }
    }
}
