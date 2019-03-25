using UnityEngine;

public class AgeController : MonoBehaviour {

    Critter critter;

    private void Start()
    {
        critter = GetComponent<Critter>();
    }

    private void Update()
    {
        critter.Age += Time.deltaTime;
        ageUp();
        UpdateLifeStage();
    }

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

    void UpdateLifeStage()
    {
        if (critter.Age < 20) { critter.lifeStage = Stage.Baby; }
        else if (critter.Age < 50 && critter.Age >= 20) { critter.lifeStage = Stage.Teen; }
        else if (critter.Age < 120 && critter.Age >= 50) { critter.lifeStage = Stage.Adult; }
        else { critter.lifeStage = Stage.Elder; }
    }

}
