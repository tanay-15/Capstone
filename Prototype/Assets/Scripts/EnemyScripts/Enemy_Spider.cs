using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spider : MonoBehaviour {

    // Use this for initialization
    public GameObject waypointA;
    public GameObject waypointB;

    private Vector3 positionA;
    private Vector3 positionB;

    public GameObject targetPlayer;
    public bool shouldAttack;
    public float staycounter = 4f;
    private bool atPointB;
    public bool attackdone;

    public float movespeed;

	void Start () {
        this.transform.position = waypointA.transform.position;
        positionA = waypointA.transform.position;
        positionB = waypointB.transform.position;
        targetPlayer = null;
	}
	
	// Update is called once per frame
	void Update () {

        Attack();
        GoBack();

        if (attackdone)
        {
            staycounter = staycounter - Time.deltaTime;
            if(staycounter <= 0)
            {
                attackdone = false;
                staycounter = 4f;
                atPointB = true;
            }
        }
	}

    void Attack()
    {
        if (shouldAttack)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, positionB, movespeed * Time.deltaTime);

            if(Vector2.Distance(this.transform.position,positionB)< 0.3f)
            {
                attackdone = true;
                shouldAttack = false;
                
             
            }
        }
    }

    void GoBack()
    {
        if (atPointB)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, positionA, movespeed * Time.deltaTime);
            if (Vector2.Distance(this.transform.position, positionA) < 0.3f)
            {
                atPointB = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            shouldAttack = true;
            targetPlayer = collider.gameObject;

        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            shouldAttack = false;
            targetPlayer = null;

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "projectile")
        {
            Destroy(this.gameObject);
        }

        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player attacked");
            atPointB = true;
            attackdone = false;
            staycounter = 4f;

        }
    }
}
