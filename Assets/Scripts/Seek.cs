using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Seek : MonoBehaviour {

    public string enemyType = "Carnivore";
    public string defaultEnemyType;

    public float viewRadius;
    public float viewAngle;   

    [SerializeField]
    GameObject target = null;
    [SerializeField]
    GameObject challenger = null;
    [SerializeField]
    GameObject food = null;
    [SerializeField]
    GameObject enemy = null;   
    [SerializeField]
    GameObject potentialMate = null;
    [SerializeField]
    GameObject mate = null;
    [SerializeField]
    GameObject courter = null;
    [SerializeField]
    GameObject opponent = null;

    GameObject lastKnownTarget = null;
    GameObject lastKnownFood = null;
    GameObject lastKnownEnemy= null;
    GameObject lastKnownMate = null;
    GameObject lastKnownPotentialMate = null;
    GameObject lastKnownCourter = null;
    GameObject lastKnownOpponent = null;
    GameObject lastKnownChallenger = null;

    GameObject temp = null;

    public List<GameObject> visibleTargets = new List<GameObject>();
    public List<string> availableTargetsType;

    public GameObject[] potentialTargets = new GameObject[6];

    Critter critter;

    public GameObject water;

    private void Start()
    {

        defaultEnemyType = enemyType;
        critter = GetComponent<Critter>();
        availableTargetsType = critter.availableTargetTypes;

        viewRadius = critter.critterTraitsDict[Critter.Trait.ViewRadius];
        viewAngle = critter.viewAngle;

        FindVisibleTargets();

        food = GetFood();
        enemy = GetEnemy();
        potentialMate = GetPotantialMate();

        potentialTargets[0] = enemy;
        potentialTargets[1] = challenger;
        potentialTargets[2] = mate;
        potentialTargets[3] = courter;       
        potentialTargets[4] = food;
        potentialTargets[5] = potentialMate;

        target = null;
        for (int i = 0; i < potentialTargets.Length; i++)
        {
            if (potentialTargets[i] != null) { target = potentialTargets[i]; break; }
        }

        if (target) { lastKnownTarget = target; }
        if (food) { lastKnownFood = food; }
        if (enemy) { lastKnownEnemy = enemy; }
        if (mate) { lastKnownMate = mate; }
        if (potentialMate) { lastKnownMate = mate; }
        if (courter) { lastKnownCourter = mate; }
        if (opponent) { lastKnownOpponent = opponent; }

    }
    private void Update()
    {
        viewAngle = critter.viewAngle;
        FindVisibleTargets();

        food = GetFood();
        enemy = GetEnemy();
        potentialMate = GetPotantialMate();

        potentialTargets[0] = enemy;
        potentialTargets[1] = challenger;
        potentialTargets[2] = courter;
        potentialTargets[3] = mate;
        potentialTargets[4] = food;
        potentialTargets[5] = potentialMate;

        target = null;
        for (int i = 0; i < potentialTargets.Length; i++)
        {
            if(potentialTargets[i] != null) { target = potentialTargets[i]; break; }
        }

        if (target) { lastKnownTarget = target; }
        if (food) { lastKnownFood = food; }
        if (enemy) { lastKnownEnemy = enemy; }
        if (mate) { lastKnownMate = mate; }
        if (potentialMate) { lastKnownPotentialMate = potentialMate; }
        if (courter) { lastKnownCourter = courter; }
        if (opponent) { lastKnownOpponent = opponent; }

    }

    void FindVisibleTargets()
    {
        water = null;
        visibleTargets.Clear();
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius);
        for (int i = 0; i < targetInViewRadius.Length; i++)
        {            
            GameObject target2 = targetInViewRadius[i].gameObject;
            Vector3 direction = (target2.transform.position - transform.position).normalized;
            if (target2.transform.root.GetComponent<Critter>())
            {
                if ((Vector3.Angle(transform.forward, direction) < viewAngle / 2 || Vector3.Distance(transform.position, target2.transform.position) < 3.0f) 
                    && target2.transform.root.gameObject.GetComponent<Critter>().isVisible)
                {
                    visibleTargets.Add(target2.transform.root.gameObject);
                }
            }
            if(target2.layer == LayerMask.NameToLayer("Water")) { water = target2; }
        }
    }

    public GameObject GetFood()
    {
        temp = null;
        float dist = Mathf.Infinity;
        for (int i = 0; i < visibleTargets.Count; i++)
        {
            float d = Vector3.Distance(transform.position, visibleTargets[i].transform.position);
            if(d < dist && availableTargetsType.Contains(visibleTargets[i].GetComponent<Critter>().critterType))
            {
                dist = d;
                temp = visibleTargets[i];
            }
        }
        return temp;
    }
    public GameObject GetEnemy()
    {
        temp = null;
        float dist = Mathf.Infinity;
        for (int i = 0; i < visibleTargets.Count; i++)
        {
            float d = Vector3.Distance(transform.position, visibleTargets[i].transform.position);
            if (d < dist && visibleTargets[i].GetComponent<Critter>().critterType == enemyType && visibleTargets[i].GetComponent<Critter>().IsAlive)
            {
                dist = d;
                temp = visibleTargets[i];
            }
        }
        return temp;
    }
    public GameObject GetPotantialMate()
    {
        temp = null;
        float dist = Mathf.Infinity;
        for (int i = 0; i < visibleTargets.Count; i++)
        {
            float d = Vector3.Distance(transform.position, visibleTargets[i].transform.position);
            if (d < dist && critter.critterType == visibleTargets[i].GetComponent<Critter>().critterType && critter.gender != visibleTargets[i].GetComponent<Critter>().gender && critter.canBreed && visibleTargets[i].GetComponent<Critter>().canBreed)
            {
                dist = d;
                temp = visibleTargets[i];
            }
        }
        return temp;
    }
    public GameObject GetOpponent()
    {
        temp = null;
        float dist = Mathf.Infinity;
        for (int i = 0; i < visibleTargets.Count; i++)
        {
            float d = Vector3.Distance(transform.position, visibleTargets[i].transform.position);
            if (d < dist && critter.critterType == visibleTargets[i].GetComponent<Critter>().critterType && critter.gender == visibleTargets[i].GetComponent<Critter>().gender && critter.canChallenge && visibleTargets[i].GetComponent<Critter>().canChallenge && critter.gameObject != visibleTargets[i].gameObject && !visibleTargets[i].GetComponent<Critter>().IsAlarmed && !critter.IsAlarmed)
            {
                dist = d;
                temp = visibleTargets[i];
            }
        }
        return temp;
    }



    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }
    public GameObject Food
    {
        get { return food; }
        set { food = value; }
    }
    public GameObject Enemy
    {
        get { return enemy; }
        set { enemy = value; }
    }
    public GameObject PotentialMate
    {
        get { return potentialMate; }
        set { potentialMate = value; }
    }
    public GameObject Mate
    {
        get { return mate; }
        set { mate = value; }
    }
    public GameObject Courter
    {
        get { return  courter; }
        set { courter = value; }
    }
    public GameObject Opponent
    {
        get { return opponent; }
        set { opponent = value; }
    }
    public GameObject Challenger
    {
        get { return challenger; }
        set { challenger = value; }
    }




    public GameObject LastKnownTarget
    {
        get { return lastKnownTarget; }
        set { lastKnownTarget = value; }
    }
    public GameObject LastKnownfood
    {
        get { return lastKnownFood; }
        set { lastKnownFood = value; }
    }
    public GameObject LastKnownEnemy
    {
        get { return lastKnownEnemy; }
        set { lastKnownEnemy = value; }
    }
    public GameObject LastKnownPotentialMate
    {
        get { return lastKnownPotentialMate; }
        set { lastKnownPotentialMate = value; }
    }
    public GameObject LastKnownMate
    {
        get { return lastKnownMate; }
        set { lastKnownMate = value; }
    }
    public GameObject LastKnownCourter
    {
        get { return lastKnownCourter; }
        set { lastKnownCourter = value; }
    }
    public GameObject LastKnownOpponent
    {
        get { return lastKnownOpponent; }
        set { lastKnownOpponent = value; }
    }
    public GameObject LastKnownChallenger
    {
        get { return lastKnownChallenger; }
        set { lastKnownChallenger = value; }
    }



    public Vector3 DirFromAngle(float angleDegrees, bool isGlobal)
    {
        if (!isGlobal) { angleDegrees += transform.eulerAngles.y; }
        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }

    private void OnDrawGizmos()
    {
        if (GetComponent<Critter>().critterType != "Vegetable")
        {
            Gizmos.DrawWireSphere(transform.position, viewRadius);

            Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
            Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

            Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
            Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
        }
    }


}
