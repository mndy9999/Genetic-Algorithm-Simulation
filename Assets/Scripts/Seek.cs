using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour {

    public string targetType = "Vegetable";
    public string enemyType = "Carnivore";

    public float viewRadius;
    public float viewAngle;

    

    [SerializeField]
    GameObject target = null;
    [SerializeField]
    GameObject enemy = null;
    [SerializeField]
    GameObject mate = null;

    public List<GameObject> visibleTargets = new List<GameObject>();

    Critter critter;

    private void Start()
    {
        critter = GetComponent<Critter>();
        viewRadius = critter.viewRadius;
        viewAngle = critter.viewAngle;
        FindVisibleTargets();
        target = GetMate();
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
    }

    //void FindVisibleTargets()
    //{
    //    visibleTargets.Clear();
    //    Collider[] targetInViewRadius = Physics.OverlapSphere(transform.Find("Eyes").gameObject.transform.position, viewRadius);
    //    for (int i = 0; i < targetInViewRadius.Length; i++)
    //    {
    //        GameObject target2 = targetInViewRadius[i].gameObject;
    //        Vector3 direction = (target2.transform.position - transform.position).normalized;
    //        if (Vector3.Angle(transform.forward, direction) < viewAngle / 2)
    //        {
    //            float distance = Vector3.Distance(transform.position, target2.transform.position);
    //            if (!Physics.Raycast(transform.Find("Eyes").gameObject.transform.position, direction, distance))
    //            {
    //                visibleTargets.Add(target2.transform.root.gameObject);
    //            }
    //        }
    //    }
    //}

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius);
        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            GameObject target2 = targetInViewRadius[i].gameObject;
            Vector3 direction = (target2.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, direction) < viewAngle / 2)
            {
                float distance = Vector3.Distance(transform.position, target2.transform.position);

                visibleTargets.Add(target2.transform.root.gameObject);

            }
        }
    }

    public GameObject GetTarget()
    {
        GameObject temp = null;
        if (!GetComponent<Critter>().IsAttacked && Critter.crittersDict.ContainsKey(targetType))
        {            
            //find closest target               
            float dist = Mathf.Infinity;
            foreach (Critter c in Critter.crittersDict[targetType])
            {
                if (visibleTargets.Contains(c.gameObject))
                {
                    float d = Vector3.Distance(this.transform.position, c.transform.position);
                    if (temp == null || d < dist)
                    {
                        temp = c.gameObject;
                        dist = d;
                    }
                }
            }
        }
        return temp;
    }
    public GameObject GetEnemy()
    {
        GameObject temp = null;
        if (Critter.crittersDict.ContainsKey(enemyType))
        {
            //find closest enemy
            float dist = Mathf.Infinity;
            foreach (Critter c in Critter.crittersDict[enemyType])
            {
                if (visibleTargets.Contains(c.gameObject))
                {
                    float d = Vector3.Distance(this.transform.position, c.transform.position);
                    if (temp == null || d < dist)
                    {
                        temp = c.gameObject;
                        dist = d;
                    }
                }
            }           
        }
        return temp;
    }
    public GameObject GetMate()
    {
        GameObject temp = null;
        if (!GetComponent<Critter>().IsAttacked && Critter.crittersDict.ContainsKey(critter.critterType))
        {
            //find closest target               
            float dist = Mathf.Infinity;
            foreach (Critter c in Critter.crittersDict[critter.critterType])
            {
                if (visibleTargets.Contains(c.gameObject) && critter.gender != c.gender && critter.CanBreed && c.CanBreed)
                {
                    float d = Vector3.Distance(this.transform.position, c.transform.position);
                    if (temp == null || d < dist)
                    {
                        temp = c.gameObject;
                        dist = d;
                    }
                }
            }
        }
        return temp;
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
