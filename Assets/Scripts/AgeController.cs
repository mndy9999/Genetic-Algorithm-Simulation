using UnityEngine;

public class AgeController : MonoBehaviour {

    Critter critter;

    private void Start()
    {
        //get refrence to the critter component
        critter = GetComponent<Critter>();
    }

    private void Update()
    {
        //increase the age based on time
        critter.age += Time.deltaTime;
        ageUp();
        UpdateLifeStage();
    }

    //set the size of the creature depending on its life stage 
    void ageUp()
    {        
        switch (critter.lifeStage)
        {
            case Stage.Baby:
                gameObject.transform.localScale = critter.initialSize;
                break;
            case Stage.Teen:
                gameObject.transform.localScale = critter.initialSize + Vector3.one * 0.4f;
                break;
            case Stage.Adult:
                gameObject.transform.localScale = critter.initialSize + Vector3.one * 1.0f;
                break;
            case Stage.Elder:
                gameObject.transform.localScale = critter.initialSize + Vector3.one * 0.8f;
                break;
        }
    }

    //update the critter's life stage based on its age
    void UpdateLifeStage()
    {
        if (critter.age < 20) { critter.lifeStage = Stage.Baby; }
        else if (critter.age < 50 && critter.age >= 20) { critter.lifeStage = Stage.Teen; }
        else if (critter.age < 120 && critter.age >= 50) { critter.lifeStage = Stage.Adult; }
        else { critter.lifeStage = Stage.Elder; }
    }

}
