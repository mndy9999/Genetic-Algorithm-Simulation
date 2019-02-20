using UnityEngine;

public class AgeController : MonoBehaviour {

    float bornTime;
    Critter critter;

    private void Start()
    {
        critter = GetComponent<Critter>();
        bornTime = Time.time;
    }

    private void Update()
    {
        critter.age += 0.01f;
        ageUp();
    }

    void ageUp()
    {
        if(critter.age < 3)
        {
            gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else if(critter.age > 2 && critter.age < 6)
        {
            gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

}
