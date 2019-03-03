using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour {

    public string enemyType = "Carnivore";

    public float viewRadius;
    public float viewAngle;   

    [SerializeField]
    GameObject target = null;
    [SerializeField]
    GameObject enemy = null;
    [SerializeField]
    GameObject mate = null;

    GameObject lastKnownTarget = null;
    GameObject lastKnownEnemy= null;
    GameObject lastKnownMate = null;

    GameObject tempTarget = null;
    GameObject tempEnemy = null;
    GameObject tempMate = null;

    public List<GameObject> visibleTargets = new List<GameObject>();
    public List<string> availableTargetsType;

    Critter critter;

    //GameObject temp = null;

    private void Start()
    {
        critter = GetComponent<Critter>();

        availableTargetsType = new List<string>() { "Vegetable", "Tree", "Dirt" };

        viewRadius = critter.viewRadius;
        viewAngle = critter.viewAngle;
        FindVisibleTargets();
        target = GetTarget();
        enemy = GetEnemy();
        mate = GetMate();
        
    }
    private void Update()
    {
        viewAngle = critter.viewAngle;
        FindVisibleTargets();
        target = GetTarget();
        enemy = GetEnemy();
        mate = GetMate();
        if(mate != null) { target = mate; }

        if (target) { lastKnownTarget = target; }
        if (enemy) { lastKnownEnemy = enemy; }
        if (mate) { lastKnownMate = mate; }

    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius);
        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            GameObject target2 = targetInViewRadius[i].gameObject;
            Vector3 direction = (target2.transform.position - transform.position).normalized;
            if (target2.GetComponent<Critter>())
            {
                if (Vector3.Angle(transform.forward, direction) < viewAngle / 2 && target2.transform.root.gameObject.GetComponent<Critter>().isVisible)
                {
                    float distance = Vector3.Distance(transform.position, target2.transform.position);

                    visibleTargets.Add(target2.transform.root.gameObject);

                }
            }
        }
    }


    //function keeps going for the trees first becuase that's the first element in the availableTargets List
    public GameObject GetTarget()
    {
        tempTarget = null;
        foreach (string targetType in availableTargetsType)
        {
            if (!GetComponent<Critter>().IsAttacked && Critter.crittersDict.ContainsKey(targetType))
            {
                //find closest target               
                float dist = Mathf.Infinity;
                foreach (Critter c in Critter.crittersDict[targetType])
                {                   
                    if (visibleTargets.Contains(c.gameObject))
                    {
                        Debug.Log("hi");
                        Debug.Log(c.critterType);
                        float d = Vector3.Distance(this.transform.position, c.transform.position);
                        if (tempTarget == null || d < dist)
                        {
                            tempTarget = c.gameObject;
                            dist = d;
                        }
                    }
                }
            }
        }
        return tempTarget;
    }
    public GameObject GetEnemy()
    {       
        if (Critter.crittersDict.ContainsKey(enemyType))
        {
            //find closest enemy
            float dist = Mathf.Infinity;
            foreach (Critter c in Critter.crittersDict[enemyType])
            {
                if (visibleTargets.Contains(c.gameObject))
                {
                    float d = Vector3.Distance(this.transform.position, c.transform.position);
                    if (tempEnemy == null || d < dist)
                    {
                        tempEnemy = c.gameObject;
                        dist = d;
                    }
                }
                else { tempEnemy = null; }
            }           
        }
        return tempEnemy;
    }
    public GameObject GetMate()
    {
        if (!GetComponent<Critter>().IsAttacked && Critter.crittersDict.ContainsKey(critter.critterType))
        {
            //find closest target               
            float dist = Mathf.Infinity;
            foreach (Critter c in Critter.crittersDict[critter.critterType])
            {
                if (visibleTargets.Contains(c.gameObject) && critter.gender != c.gender && critter.CanBreed && c.CanBreed)
                {
                    float d = Vector3.Distance(this.transform.position, c.transform.position);
                    if (tempMate == null || d < dist)
                    {
                        tempMate = c.gameObject;
                        dist = d;
                    }
                }
                else { tempMate = null; }
            }
        }
        return tempMate;
    }

    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }
    public GameObject Enemy
    {
        get { return enemy; }
        set { enemy = value; }
    }
    public GameObject Mate
    {
        get { return mate; }
        set { mate = value; }
    }

    public GameObject LastKnownTarget
    {
        get { return lastKnownTarget; }
        set { lastKnownTarget = value; }
    }
    public GameObject LastKnownEnemy
    {
        get { return lastKnownEnemy; }
        set { lastKnownEnemy = value; }
    }
    public GameObject LastKnownMate
    {
        get { return lastKnownMate; }
        set { lastKnownMate = value; }
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
