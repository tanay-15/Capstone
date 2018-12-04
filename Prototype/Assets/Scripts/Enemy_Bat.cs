using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : BasicEnemy {

    // Use this for initialization

    public float Range;
    public GameObject player;

    public float attackspeed;
    public float movspeed;

    public GameObject[] waypoints;
    public Vector3 pointA;
    public Vector3 pointB;
    public Vector3 pointC;

    public bool moveA;
    public bool moveB;
    public bool moveC;
    public bool moveD;

    public bool Attack;
   

	void Start () {

        pointA = waypoints[0].transform.position;
        pointB = waypoints[1].transform.position;
        pointC = waypoints[2].transform.position;
        this.transform.position = pointA;
        moveA = true;
	}
	
	// Update is called once per frame
	void Update () {
		

        if(player == null)
        {
            //Keep moving around the fix path and detecting  the player 
            Movement();
          
        }

        else if (player != null)
        {
            //attack the player
            AttackBat();
        }


	}

    public void AttackBat()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, attackspeed * Time.deltaTime);
    }

    public void Movement()
    {
        if (moveA)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, pointB, movspeed * Time.deltaTime);
            if(Vector2.Distance(this.transform.position,pointB) < 0.3f){
                moveA = false;
                moveB = true;
            }
        }

        if (moveB)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, pointC, movspeed * Time.deltaTime);
            if (Vector2.Distance(this.transform.position, pointC )< 0.3f)
            {
                moveB = false;
                moveC = true;
            }
        }

        if (moveC)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, pointB, movspeed * Time.deltaTime);
            if(Vector2.Distance(this.transform.position,pointB)< 0.3f)
            {
                moveC = false;
                moveD = true;

            }
        }

        if (moveD)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, pointA, movspeed * Time.deltaTime);
            if (Vector2.Distance(this.transform.position, pointA) < 0.3f)
            {
                moveD = false;
                moveA = true;
            }
        }

    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //attack player
            collision.gameObject.SendMessage("GetHit", -15);
            events.OnDeath.Invoke();
            Destroy(this.gameObject);
        }

        if(collision.gameObject.tag == "projectile" || collision.gameObject.tag == "Grabbable")
        {
            events.OnDeath.Invoke();
            Destroy(this.gameObject);
        }
    }


    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            Debug.Log("Player entered in range");
            player = collider.gameObject;
            Attack = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            Attack = false;
            player = null;
        }
    }

    //just for testing purpose
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, Range);
    }
}
