using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class Critter : MonoBehaviour {

    public string critterType = "Vegetable";

    public enum Gender { Male, Female };
    public Gender gender;
    public enum Stage { Baby, Teen, Adult, Elder };
    public Stage lifeStage;

    [SerializeField] float health = 100f;
    [SerializeField] float energy = 100f;
    [SerializeField] float resource = 100f;

    public float age = 0;

    [HideInInspector] public float energyPerSecond = 1f;

    [HideInInspector] public float runSpeed = 5f;
    [HideInInspector] public float walkSpeed = 2f;
    public float speed;


    
    [HideInInspector] public float viewRadius = 10f;
    [HideInInspector] public float defaultViewAngle = 90f;
    [Range(0, 360)] public float viewAngle;
   

    static public Dictionary<string, List<Critter>> crittersDict;
    public List<string> availableBehaviours;
    public List<string> availableTargetTypes;

    [HideInInspector] public Vector3 initialSize;

    public bool isVisible;
    public bool isChased;
    public bool isAttacked;
    

    public bool canBreed;
    [HideInInspector] public bool breedTimer;
    [HideInInspector] public float time;

    // Use this for initialization
    void Awake () {
        if (crittersDict == null) { crittersDict = new Dictionary<string, List<Critter>>(); }
        if (!crittersDict.ContainsKey(critterType)) { crittersDict[critterType] = new List<Critter>(); }
        crittersDict[critterType].Add(this);

        isVisible = true;
        isChased = false;
        isAttacked = false;

        initialSize = new Vector3(0.2f, 0.2f, 0.2f);
        time = Time.time;

        speed = runSpeed;
        viewAngle = defaultViewAngle;

       // PopulateAvailableBehaviours();               
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
        UpdateSpeed();

        energy = Mathf.Clamp(energy - Time.deltaTime * energyPerSecond, 0, 100);
        if (energy <= 0) { health = Mathf.Clamp(health - Time.deltaTime, 0, 100); }
        if (!IsAlive) { energy = 0; health = 0; resource = Mathf.Clamp(resource - Time.deltaTime, 0, 100); }
        if (resource <= 0) { KillSelf(); }

        if (breedTimer)
        {
            time = Time.time;
            canBreed = false;
            breedTimer = false;
        }
        if (Time.time >= time + 5) { canBreed = lifeStage >= Stage.Teen && lifeStage < Stage.Elder; }
        else { canBreed = false; }
    }

    void PopulateAvailableBehaviours()
    {
        for (int i = 0; i < Behaviours.behaviours.Count; i++)
        {
            availableBehaviours.Add(Behaviours.behaviours[i]);
        }
    }

    void UpdateSpeed()
    {
        if (energy < 10)
        {
            while (speed > walkSpeed) { speed -= 0.2f; }
        }
        else
        {
            while (speed < runSpeed) { speed += 0.2f; }
        }
    }   
    void UpdateFOV()
    {
        if (isChased) { viewAngle = 360; }
        else { viewAngle = defaultViewAngle; }
    }
    void UpdateLifeStage()
    {
        if(age < 3) { lifeStage = Stage.Baby; }
        else if(age < 6 && age > 2) { lifeStage = Stage.Teen; }
        else if(age < 10 && age > 5) { lifeStage = Stage.Adult; }
        else { lifeStage = Stage.Elder; }
    }

    public void KillSelf() { Destroy(gameObject); }

    #region Getters And Setters
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
    public bool IsAttacked
    {
        get { return isAttacked; }
        set { isAttacked = value; }
    }
    public bool IsAlive
    {
        get { return health > 0 && age < 15; }
        set { }
    }
    public float Energy
    {
        get { return energy; }
        set { energy = value; }
    }
    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    public float Resource
    {
        get { return resource; }
        set { resource = value; }
    }
    #endregion
}
