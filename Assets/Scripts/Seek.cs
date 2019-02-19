using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour {

    public string targetType = "Vegetable";
    public string enemyType = "Carnivore";

    public float viewRadius = 10f;
    public float viewAngle = 90f;

    public List<GameObject> visibleTargets = new List<GameObject>();

    private void Start()
    {
        FindVisibleTargets();
    }
    private void Update()
    {
        FindVisibleTargets();
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true){
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius);
        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            GameObject target = targetInViewRadius[i].gameObject;
            Vector3 direction = (target.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, direction) < viewAngle / 2)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (Physics.Raycast(transform.position, direction, distance))
                {
                    visibleTargets.Add(target.transform.root.gameObject);
                }
            }
        }
    }

    public GameObject Target
    {
        get
        {
            if (Critter.crittersDict.ContainsKey(targetType))
            {
                //find closest target
                GameObject target = null;
                float dist = Mathf.Infinity;
                foreach (Critter c in Critter.crittersDict[targetType])
                {
                    if (visibleTargets.Contains(c.gameObject))
                    {
                        float d = Vector3.Distance(this.transform.position, c.transform.position);
                        if (target == null || d < dist)
                        {
                            target = c.gameObject;
                            dist = d;
                        }                       
                    }
                }
                return target;
            }
            return null;
        }
        set { }
    }

    public GameObject Enemy
    {
        get
        {
            if (Critter.crittersDict.ContainsKey(enemyType))
            {
                //find closest enemy
                GameObject enemy = null;
                float dist = Mathf.Infinity;
                foreach (Critter c in Critter.crittersDict[enemyType])
                {
                    if (visibleTargets.Contains(c.gameObject))
                    {
                        float d = Vector3.Distance(this.transform.position, c.transform.position);
                        if (enemy == null || d < dist)
                        {
                            enemy = c.gameObject;
                            dist = d;
                        }
                    }
                }
                return enemy;
            }
            return null;
        }
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
