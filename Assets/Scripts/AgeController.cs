using UnityEngine;

public class AgeController : MonoBehaviour {

    Critter critter;

    private void Start()
    {
        critter = GetComponent<Critter>();
    }

    private void Update()
    {
        critter.age += 0.01f;
        ageUp();
    }

    void ageUp()
    {
        switch (critter.lifeStage)
        {
            case Critter.Stage.Baby:
                gameObject.transform.localScale = critter.initialSize;
                break;
            case Critter.Stage.Teen:
                gameObject.transform.localScale = critter.initialSize + Vector3.one * 0.4f;
                break;
            case Critter.Stage.Adult:
                gameObject.transform.localScale = critter.initialSize + Vector3.one * 0.8f;
                break;
        }
    }

}
