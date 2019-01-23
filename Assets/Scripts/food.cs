using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food : MonoBehaviour {

    public float level = 20;

    public void lowerFoodLevel(float x)
    {
        level -= x;
    }

    private void Update() { if(level < 0)   Destroy(gameObject); }
}
