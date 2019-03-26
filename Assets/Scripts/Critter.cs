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


    public Dictionary <Trait, float> critterTraitsDict;
    static public Dictionary<string, List<Critter>> crittersDict;

    public List<State<AI>> availableBehaviours;
    public List<string> availableTargetTypes;

    [HideInInspector] public Vector3 initialSize;

    [HideInInspector] public bool isAlarmed;
    [HideInInspector] public bool isAttacked;
    [HideInInspector] public bool isVisible;
    [HideInInspector] public bool isChallenged;
    [HideInInspector] public bool isScared;
    [HideInInspector] public bool isCourted;

    [HideInInspector] public bool canAlarm;
    [HideInInspector] public bool canBreed;
    [HideInInspector] public bool canChallenge;

    [HideInInspector] public bool isChild;

    [HideInInspector] public float breedTime;
    [HideInInspector] public float challengeTime;
    [HideInInspector] public float alarmTime;

    // Use this for initialization
    void Awake () {
        age = 0.0f;
        name = transform.name;
        if (crittersDict == null) { crittersDict = new Dictionary<string, List<Critter>>(); }
        if (!crittersDict.ContainsKey(critterType)) { crittersDict[critterType] = new List<Critter>(); }
        crittersDict[critterType].Add(this);
        critterTraitsDict = new Dictionary<Trait, float>();

        EncodeTraits();
        if (!isChild) { GenerateRandomTraits(); }
        //DecodeTraits();


        ResetAlarm();
        ResetBreed();
        ResetChallenge();

        
        isAlarmed = false;
        isAttacked = false;
        isVisible = true;
        canChallenge = true;

        initialSize = new Vector3(0.2f, 0.2f, 0.2f);

        viewAngle = critterTraitsDict[Trait.ViewAngle];
    }

    private void OnDestroy()
    {       
        if(crittersDict[critterType].Contains(this))
            crittersDict[critterType].Remove(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (resource <= 0) { Destroy(gameObject); }
        if (isChallenged) canChallenge = false;
        
        UpdateFOV();

        if(lifeStage == Stage.Elder) { IsAlive = Random.Range(0, 10) < 3; }         

        canBreed = breedTime > 10 ? (lifeStage >= Stage.Teen && lifeStage < Stage.Elder) : false;
        breedTime += Time.deltaTime;

        canAlarm = alarmTime > 10 ? true : false;
        alarmTime += Time.deltaTime;

        canChallenge = challengeTime > 10 ? !isChallenged : false;
        challengeTime += Time.deltaTime;

    }

    public void GenerateRandomTraits()
    {
        foreach (Trait t in System.Enum.GetValues(typeof(Trait)))
        {
            if (t != Trait.ViewAngle)
                critterTraitsDict[t] = Random.Range(1, 10);
            else
                critterTraitsDict[t] = Random.Range(1, 360);
        }
        critterTraitsDict[Trait.ViewRadius] = 10f;  
        critterTraitsDict[Trait.RankPoints] = 0.0f;  
        if(critterTraitsDict[Trait.RunSpeed] < critterTraitsDict[Trait.WalkSpeed])
        {
            float temp = critterTraitsDict[Trait.RunSpeed];
            critterTraitsDict[Trait.RunSpeed] = critterTraitsDict[Trait.RunSpeed];
            critterTraitsDict[Trait.RunSpeed] = temp;
        }
    }
    public void SetupCritter()
    {
        if (availableBehaviours.Contains(AI_Swim.instance)) { GetComponent<NavMeshAgent>().areaMask += LayerMask.NameToLayer("Water"); }
    }

    void EncodeTraits()
    {

        foreach(Trait t in System.Enum.GetValues(typeof(Trait))){
            critterTraitsDict.Add(t, 0.0f);
        }

    }
    void DecodeTraits()
    {
        Debug.Log(name);
        foreach (Trait t in critterTraitsDict.Keys)
            Debug.Log(t.ToString() + ":  " + critterTraitsDict[t]);
    }

    void UpdateFOV()
    {
        if (isAlarmed) { viewAngle = 360; }
        else { viewAngle = critterTraitsDict[Trait.ViewAngle]; }
    }

    public void UpdateStats()
    {
        energy = Mathf.Clamp(energy - Time.deltaTime * energyPerSecond, 0, 100);
        if (energy <= 0) { health = Mathf.Clamp(health - Time.deltaTime, 0, 100); }
        if (!IsAlive) { energy = 0; health = 0; resource = Mathf.Clamp(resource - Time.deltaTime, 0, 100); }        
    }

    public void ResetAlarm() { alarmTime = 0; }
    public void ResetBreed() { breedTime = 0; }
    public void ResetChallenge() { challengeTime = 0; }

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
