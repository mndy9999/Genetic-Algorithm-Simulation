using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class Critter : MonoBehaviour {

    public float health = 100f;
    public float energy = 100f;
    public float resource = 100f;

    public float age = 0;

    public float energyPerSecond = 1f;

    public float runSpeed = 5f;
    public float walkSpeed = 2f;
    public float speed;

    public float viewRadius = 10f;
    public float viewAngle = 90f;

    public string critterType = "Vegetable";

    static public Dictionary<string, List<Critter>> crittersDict;
    public List<string> availableBehaviours;

    public Vector3 initialSize;
    public enum Gender { Male, Female};
    public Gender gender;
    public enum Stage { Baby, Teen, Adult, Elder};
    public Stage lifeStage;

    public bool isChased;

    public bool canBreed;
    public bool breedTimer;
    public float time;

    Animator animator;

    // Use this for initialization
    void Awake () {
        initialSize = new Vector3(0.2f, 0.2f, 0.2f);
        if (crittersDict == null) { crittersDict = new Dictionary<string, List<Critter>>(); }
        if(!crittersDict.ContainsKey(critterType)) { crittersDict[critterType] = new List<Critter>(); }
        crittersDict[critterType].Add(this);
        animator = this.GetComponent<Animator>();
        speed = runSpeed;
        time = Time.time;
        PopulateAvailableBehaviours();
        IsChased = false;
	}

    private void OnDestroy()
    {       
        crittersDict[critterType].Remove(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateFOV();
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

        if (breedTimer)
        {
            time = Time.time;
            canBreed = false;
            breedTimer = false;
        }
        if (Time.time >= time + 5)
        {
            canBreed = lifeStage >= Stage.Teen && lifeStage < Stage.Elder;
        }
        else
        {
            canBreed = false;
        }
    }

    void PopulateAvailableBehaviours()
    {
        for(int i = 0; i < Behaviours.behaviours.Count; i++)
        {
            int r = Random.Range(0, 10);
            availableBehaviours.Add(Behaviours.behaviours[i]);
            Debug.Log(Behaviours.behaviours[i]);
        }
    }
    void changeSpeed()
    {
        if (energy < 10)
        {
            while (speed > walkSpeed) { speed -= 0.2f; }
        }
    }
    public bool IsAlive() { return health > 0 && age < 15; }
    public bool BreedTimer
    {
        get { return breedTimer; }
        set { breedTimer = value; }
    }
    public bool CanBreed
    {
        get { return canBreed; }
        set { canBreed = value; }
    }
    public bool IsChased
    {
        get { return isChased; }
        set { isChased = value; }
    }
    public bool IsAttacked;
    public void KillSelf() { Destroy(gameObject); }
    public void UpdateFOV()
    {
        if (isChased) { viewAngle = 360; }
        else { viewAngle = 90; }
    }
    void UpdateLifeStage()
    {
        if(age < 3) { lifeStage = Stage.Baby; }
        else if(age < 6 && age > 2) { lifeStage = Stage.Teen; }
        else if(age < 10 && age > 5) { lifeStage = Stage.Adult; }
        else { lifeStage = Stage.Elder; }
    }
}
