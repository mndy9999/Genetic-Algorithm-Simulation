using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    Vector3 direction;
    Vector3 target;

    public Animator animator;

    private void Start()
    {
        chooseDirection();       
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown("c")) { chooseDirection(); }
        moveRandomly();        
    }


    void chooseDirection()
    {
        direction = new Vector3(0, Random.Range(0, 360), 0);
        transform.Rotate(direction);
    }

    void moveRandomly()
    {
        target = new Vector3(0, 0, direction.y);
        transform.Translate(target*0.0006f);
        animator.SetFloat("Idle", 0.5f);
    }

}
