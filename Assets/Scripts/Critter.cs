using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine;

public class Critter : MonoBehaviour
{
    public string critterType = "Vegetable";

    [SerializeField] float health = 100f;
    [SerializeField] float energy = 100f;
    [SerializeField] float resource = 100f;
   
    [HideInInspector] public float energyPerSecond = 0.001f;

    public float age;
    float fitnessScore;

    [Range(0, 360)] public float viewAngle;

    public Gender gender;
    public Stage lifeStage;


    public Dictionary <Trait, float> critterTraitsDict;             //holds all the traits and their values
    static public Dictionary<string, List<Critter>> crittersDict;   //holds references to all the critters, sorted depending on the critter types

    public List<State<AI>> availableBehaviours;
    public List<string> availableTargetTypes;

    [HideInInspector] public Vector3 initialSize;   //start as baby - maybe randomize this and use it as a trait???

    //these variables allow or prevent the critter from interacting or being interacted with
    [HideInInspector] public bool isAlarmed;
    [HideInInspector] public bool isAttacked;
    [HideInInspector] public bool isVisible;
    [HideInInspector] public bool isChallenged;
    [HideInInspector] public bool isScared;
    [HideInInspector] public bool isCourted;

    [HideInInspector] public bool canAlarm;
    [HideInInspector] public bool canBreed;
    [HideInInspector] public bool canChallenge;

    [HideInInspector] public bool isChild;      //tells the component if it should generate random traits or not

    //timers to set the corresponding bools 
    [HideInInspector] public float breedTime;
    [HideInInspector] public float challengeTime;
    [HideInInspector] public float alarmTime;

    // Use this for initialization
    void Awake () {
        age = 0.0f;     //set age to default 0 when the critter is created
        name = transform.name;      //set the name variable to the game object's name
        if (crittersDict == null) { crittersDict = new Dictionary<string, List<Critter>>(); }   //if there isn't a dictionary holding the critters, create one
        if (!crittersDict.ContainsKey(critterType)) { crittersDict[critterType] = new List<Critter>(); }    //if the dictionary doesn't contain the critter type, add it in as a list
        crittersDict[critterType].Add(this);                                                                //add the critter to the corresponding critter type list in the dictionary
        critterTraitsDict = new Dictionary<Trait, float>();     //create dictionary to keep track of the traits and their values

        EncodeTraits();    
        if (!isChild) { GenerateRandomTraits(); }   //if the critter is not a child, negerate random traits

        //reset all timers
        ResetAlarm();
        ResetBreed();
        ResetChallenge();

        
        //set all bools to default
        isAlarmed = false;
        isAttacked = false;
        isVisible = true;
        canChallenge = true;

        //set the initial size of the gameobject as a baby
        initialSize = new Vector3(0.2f, 0.2f, 0.2f);

        viewAngle = critterTraitsDict[Trait.ViewAngle];     //keep track of viewAngle - this changes when the critter is alarmed   
    }

    private void OnDestroy()
    {       
        //when the critter is destoryed but still in the dictionary, remove it from the dictionary
        if(crittersDict[critterType].Contains(this))
            crittersDict[critterType].Remove(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if the critter has been dead for a while and lost all its resources, destroy iy
        if (resource <= 0) { Destroy(gameObject); }
        if (isChallenged) canChallenge = false;     //don't let it challenge other critter if it is currently being challenged
        
        UpdateFOV();    //update the view angle to 360 if the critter is alarmed

        //if the critter is an elder, every frame, there is a 30% chance of it dying 
        if (lifeStage == Stage.Elder) { IsAlive = Random.Range(0, 10) < 3; }
        if (!IsAlive) { crittersDict[critterType].Remove(this); }       //remove from dictionary when it goes into the dead state so it doesn't affect the average fitness score of the population
        canBreed = breedTime > 20 ? (lifeStage >= Stage.Teen && lifeStage < Stage.Elder) : false;   //set the breeding ability depending on the critter's breeding timer and its lifestage
        breedTime += Time.deltaTime;        //update breed timer every frame
            
        canAlarm = alarmTime > 20 ? true : false;       //set the alarm ability depending on the alarm timer
        alarmTime += Time.deltaTime;        //update alarm timer every frame

        canChallenge = challengeTime > 20 ? !isChallenged : false;      //set the challenge ability depending on the challenge timer
        challengeTime += Time.deltaTime;    //update challenge timer every frame

    }

    public void GenerateRandomTraits()
    {
        //generate random trait values
        foreach (Trait t in System.Enum.GetValues(typeof(Trait)))
        {
            if (t != Trait.ViewAngle)
                critterTraitsDict[t] = Random.Range(1, 10);
            else
                critterTraitsDict[t] = Random.Range(1, 360);
        }
        
        critterTraitsDict[Trait.RankPoints] = 0.0f;  //start with 0 rank points

        //if the run speed is lower than the walk speed, swap the values arounf
        if(critterTraitsDict[Trait.RunSpeed] < critterTraitsDict[Trait.WalkSpeed])
        {
            float temp = critterTraitsDict[Trait.RunSpeed];
            critterTraitsDict[Trait.RunSpeed] = critterTraitsDict[Trait.RunSpeed];
            critterTraitsDict[Trait.RunSpeed] = temp;
        }
    }
    public void SetupCritter()
    {
        //if the critter has the swim behaviour, let it go into the water
        if (availableBehaviours.Contains(AI_Swim.instance)) { GetComponent<NavMeshAgent>().areaMask += LayerMask.NameToLayer("Water"); }
    }

    void EncodeTraits()
    {
        //add all possible traits to the dictinary and set them to 0 by default
        foreach(Trait t in System.Enum.GetValues(typeof(Trait))){
            critterTraitsDict.Add(t, 0.0f);
        }

    }

    //log the traits for testing purposes
    void DecodeTraits()
    {
        //Debug.Log(name);
        //foreach (Trait t in critterTraitsDict.Keys)
            //Debug.Log(t.ToString() + ":  " + critterTraitsDict[t]);
    }

    //set the view angle to 360 if the critter is alarmed, otherwise set it to the default value
    void UpdateFOV()
    {
        if (isAlarmed) { viewAngle = 360; }
        else { viewAngle = critterTraitsDict[Trait.ViewAngle]; }
    }

    //update the stats every frame
    public void UpdateStats()
    {
        energy = Mathf.Clamp(energy - Time.deltaTime * energyPerSecond, 0, 100);        //lose energy per second but don't let it go over 100
        if (energy <= 0) { health = Mathf.Clamp(health - Time.deltaTime, 0, 100); }     //if the critter is out of energy it starts losing health
        if (!IsAlive) { energy = 0; health = 0; resource = Mathf.Clamp(resource - Time.deltaTime, 0, 100); }            //if it's out of health it goes into the dead state and starts losing resources
    }

    //reset timers
    public void ResetAlarm() { alarmTime = 0; }
    public void ResetBreed() { breedTime = 0; }
    public void ResetChallenge() { challengeTime = 0; }

    //getters and setters
    #region Getters And Setters

    public bool CanBreed
    {
        get { return canBreed; }
        set { canBreed = value; }
    }
    public float FitnessScore
    {
        get { return fitnessScore; }
        set { fitnessScore = value; }
    }
    public bool IsAlarmed
    {
        get { return isAlarmed; }
        set { isAlarmed = value; }
    }
    public bool CanAlarm
    {
        get { return canAlarm; }
        set { canAlarm = value; }
    }
    public bool IsAttacked
    {
        get { return isAttacked; }
        set { isAttacked = value; }
    }
    public bool IsScared
    {
        get { return isScared; }
        set { isScared = value; }
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
    #endregion

}
