using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine;

public class Critter : MonoBehaviour {

    public string critterType = "Vegetable";
    public string name;

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

    public bool isAlarmed;
    public bool isAttacked;
    public bool isVisible;

    public bool canBreed;
    [HideInInspector] public bool breedTimer;
    [HideInInspector] public float time;

    // Use this for initialization
    void Awake () {
        name = transform.name;
        if (crittersDict == null) { crittersDict = new Dictionary<string, List<Critter>>(); }
        if (!crittersDict.ContainsKey(critterType)) { crittersDict[critterType] = new List<Critter>(); }
        crittersDict[critterType].Add(this);
        availableBehaviours = new List<string>();
        isAlarmed = false;
        isAttacked = false;
        isVisible = true;

        initialSize = new Vector3(0.2f, 0.2f, 0.2f);
        time = Time.time;

        speed = runSpeed;
        viewAngle = defaultViewAngle;
        if (critterType == "Herbivore") PopulateAvailableBehaviours();

        canBreed = true;
    }

    private void OnDestroy()
    {       
        crittersDict[critterType].Remove(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (resource <= 0) { KillSelf(); }

        UpdateFOV();
       // UpdateLifeStage();
        UpdateSpeed();

        if(lifeStage == Stage.Elder) { IsAlive = Random.Range(0, 10) < 3; }    
        //canBreed = lifeStage >= Stage.Teen && lifeStage < Stage.Elder;
    }

    public void PopulateAvailableBehaviours()
    {
        for (int i = 0; i < Behaviours.behaviours.Count; i++)
        {
            availableBehaviours.Add(Behaviours.behaviours[i]);
        }       
    }
    public void SetupCritter()
    {
        if (availableBehaviours.Contains(AI_Swim.name)) { GetComponent<NavMeshAgent>().areaMask += LayerMask.NameToLayer("Water"); }
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
        if (isAlarmed) { viewAngle = 360; }
        else { viewAngle = defaultViewAngle; }
    }
    void UpdateLifeStage()
    {
        if(age < 20) { lifeStage = Stage.Baby; }
        else if(age < 50 && age >= 20) { lifeStage = Stage.Teen; }
        else if(age < 120 && age >= 50) { lifeStage = Stage.Adult; }
        else { lifeStage = Stage.Elder; }
    }

    public void UpdateStats()
    {
        energy = Mathf.Clamp(energy - Time.deltaTime * energyPerSecond, 0, 100);
        if (energy <= 0) { health = Mathf.Clamp(health - Time.deltaTime, 0, 100); }
        if (!IsAlive) { energy = 0; health = 0; resource = Mathf.Clamp(resource - Time.deltaTime, 0, 100); }        
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
    public bool IsAlarmed
    {
        get { return isAlarmed; }
        set { isAlarmed = value; }
    }
    public bool IsAttacked
    {
        get { return isAttacked; }
        set { isAttacked = value; }
    }
    public bool IsAlive
    {
        get { return health > 0; }
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
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public int Age
    {
        get { return (int)age; }
        set { age = value; }
    }
    #endregion
}
