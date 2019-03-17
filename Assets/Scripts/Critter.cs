using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine;

public class Critter : MonoBehaviour {

    public string critterType = "Vegetable";
    public string name;

    [SerializeField] string rank;

    public enum Gender { Female, Male };
    public Gender gender;
    public enum Stage { Baby, Teen, Adult, Elder };
    public Stage lifeStage;
    public enum Trait { WalkSpeed, RunSpeed, ViewRadius, ViewAngle, AttackPoints, ThreatPoints, RankPoints, VoiceStrenght, Beauty, Acting };

    [SerializeField] float health = 100f;
    [SerializeField] float energy = 100f;
    [SerializeField] float resource = 100f;
   
    [HideInInspector] public float energyPerSecond = 0.5f;

    public float age;
    public float speed;

    public float fitnessScore;

    [Range(0, 360)] public float viewAngle;
   

    static public Dictionary<string, List<Critter>> crittersDict;
    public Dictionary <Trait, float> critterTraitsDict;

    public List<State<AI>> availableBehaviours;
    public List<string> availableTargetTypes;

    [HideInInspector] public Vector3 initialSize;

    public bool isAlarmed;
    public bool isAttacked;
    public bool isVisible;
    public bool isChallenged;

    public bool canAlarm;
    public bool canBreed;
    public bool canChallenge;

    public bool isChild;

    [HideInInspector] public bool breedTimer;
    [HideInInspector] public float challengeTimer;
    [HideInInspector] public float time;

    // Use this for initialization
    void Awake () {
        name = transform.name;
        if (crittersDict == null) { crittersDict = new Dictionary<string, List<Critter>>(); }
        if (!crittersDict.ContainsKey(critterType)) { crittersDict[critterType] = new List<Critter>(); }
        crittersDict[critterType].Add(this);
        critterTraitsDict = new Dictionary<Trait, float>();
        availableBehaviours = new List<State<AI>>();

        EncodeTraits();
        if (!isChild) { PopulateAvailableBehaviours(); GenerateRandomTraits(); }
        DecodeTraits();
        
        
        isAlarmed = false;
        isAttacked = false;
        isVisible = true;
        canChallenge = true;

        initialSize = new Vector3(0.2f, 0.2f, 0.2f);
        time = Time.time;

        viewAngle = critterTraitsDict[Trait.ViewAngle];

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
        if (isChallenged) canChallenge = false;
        if(challengeTimer > 100 && !isChallenged) { canChallenge = true; }
        else { challengeTimer += Time.deltaTime; }
        
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
            if (Random.Range(0, 10) < 9)
                availableBehaviours.Add(Behaviours.behaviours[i]);
        }
        for (int i = 0; i < Behaviours.EnemyEncounterBehaviours.Count; i++)
        {
            if (Random.Range(0, 10) < 9)
                availableBehaviours.Add(Behaviours.behaviours[i]);
        }
        for (int i = 0; i < Behaviours.MateEncounterBehaviours.Count; i++)
        {
            if (Random.Range(0, 10) < 9)
                availableBehaviours.Add(Behaviours.MateEncounterBehaviours[i]);
        }
        for (int i = 0; i < Behaviours.SocialRankBehaviours.Count; i++)
        {
            if (Random.Range(0, 10) < 9)
                availableBehaviours.Add(Behaviours.SocialRankBehaviours[i]);
        }


    }
    public void GenerateRandomTraits()
    {
        foreach (Trait t in System.Enum.GetValues(typeof(Trait)))
        {
            if (t != Trait.ViewAngle)
                critterTraitsDict[t] = Random.Range(0, 10);
            else
                critterTraitsDict[t] = Random.Range(0, 180);
        }
        critterTraitsDict[Trait.ViewRadius] = 10f;  
        critterTraitsDict[Trait.RankPoints] = 0.0f;  
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

    void UpdateSpeed()
    {
        if (energy < 10)
        {
            while (speed > critterTraitsDict[Trait.WalkSpeed]) { speed -= 0.2f; }
        }
        else
        {
            while (speed < critterTraitsDict[Trait.RunSpeed]) { speed += 0.2f; }
        }
    }   
    void UpdateFOV()
    {
        if (isAlarmed) { viewAngle = 360; }
        else { viewAngle = critterTraitsDict[Trait.ViewAngle]; }
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
    public string Rank
    {
        get { return rank; }
        set { rank = value; }
    }
    #endregion

}
