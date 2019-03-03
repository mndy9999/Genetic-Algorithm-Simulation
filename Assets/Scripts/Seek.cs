using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    private void Start()
    {
        critter = GetComponent<Critter>();
        availableTargetsType = critter.availableTargetTypes;

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

        if(!critter.IsAttacked) target = GetTarget();
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
            if (target2.transform.root.GetComponent<Critter>())
            {
                if ((Vector3.Angle(transform.forward, direction) < viewAngle / 2 || Vector3.Distance(transform.position, target2.transform.position) < 1.0f) 
                    && target2.transform.root.gameObject.GetComponent<Critter>().isVisible)
                {
                    visibleTargets.Add(target2.transform.root.gameObject);
                }
            }
        }
    }

    public GameObject GetTarget()
    {
        tempTarget = null;
        float dist = Mathf.Infinity;
        for (int i = 0; i < visibleTargets.Count; i++)
        {
            float d = Vector3.Distance(transform.position, visibleTargets[i].transform.position);
            if(d < dist && availableTargetsType.Contains(visibleTargets[i].GetComponent<Critter>().critterType))
            {
                dist = d;
                tempTarget = visibleTargets[i];
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
                if (visibleTargets.Contains(c.gameObject) && c.GetComponent<Critter>().IsAlive)
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
