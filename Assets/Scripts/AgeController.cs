using UnityEngine;

public class AgeController : MonoBehaviour {

    float bornTime;
    public float age;

    private void Start()
    {
        age = 0;
        bornTime = Time.time;

    }

    private void Update()
    {
        age += 0.01f;
        ageUp();
    }

    void ageUp()
    {
        if(age < 3)
        {
            gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else if(age > 2 && age < 6)
        {
            gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

}
