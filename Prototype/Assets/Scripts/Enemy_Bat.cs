using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : BasicEnemy
{

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

    public bool IsAttacking = false;
    private Vector3 targetposition;

    public bool GetPlayerPost = false;

    public float AttackCounter = 0f;

    public float health = 10f;

    public bool PlayerHit = false;




    void Start()
    {

        pointA = waypoints[0].transform.position;
        pointB = waypoints[1].transform.position;
        //pointC = waypoints[2].transform.position;
        this.transform.position = pointA;
        moveA = true;
    }

    // Update is called once per frame
    void Update()
    {



        if (!IsAttacking)
        {
            Movement();
        }




        if (player != null && AttackCounter <= 0f && !PlayerHit)
        {
            //attack the player
            IsAttacking = true;
            AttackBat();
        }


        AttackCounter = AttackCounter - Time.deltaTime;

        if (PlayerHit)
        {
            moveA = true;
            moveB = false;
            AttackCounter = 8f;
        }

        if (health <= 0)
        {
            events.OnDeath.Invoke();
            Destroy(this.gameObject);
        }

    }

    public void AttackBat()
    {
        //if (!AttackDone)
        //{


        //    if (Attack)
        //    {
        //        if (!GetPlayerPost)
        //        {
        //            targetposition = player.transform.position;
        //            GetPlayerPost = true;
        //        }


        //    }

        //    if (GetPlayerPost)
        //    {
        //        this.transform.position = Vector2.MoveTowards(this.transform.position, targetposition, attackspeed * Time.deltaTime);

        //        if (Vector2.Distance(this.transform.position, targetposition) < 0.4f)
        //        {
        //            Attack = false;
        //            AttackDone = true;
        //            targetposition = Vector3.zero;
        //            GetPlayerPost = false;
        //        }
        //    }

        //}


        this.transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, attackspeed * Time.deltaTime);


    }

    public void SkullMovement()
    {


        if (moveA)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, pointA, movspeed * Time.deltaTime);
            if (Vector2.Distance(this.transform.position, pointA) < 0.3f)
            {
                moveA = false;
                moveB = true;
                PlayerHit = false;
            }
        }

        if (moveB)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, pointB, movspeed * Time.deltaTime);
            if (Vector2.Distance(this.transform.position, pointB) < 0.3f)
            {
                moveB = false;
                moveA = true;
                PlayerHit = false;
            }
        }


    }

    public void Movement()
    {
        if (moveA)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, pointA, movspeed * Time.deltaTime);
            if (Vector2.Distance(this.transform.position, pointA) < 0.3f)
            {
                moveA = false;
                moveB = true;
                PlayerHit = false;
            }
        }

        if (moveB)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, pointB, movspeed * Time.deltaTime);
            if (Vector2.Distance(this.transform.position, pointB) < 0.3f)
            {
                moveB = false;
                moveA = true;
                PlayerHit = false;
            }
        }



    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //attack player
            collision.gameObject.SendMessage("GetHit", -15);
            //events.OnDeath.Invoke();
            //Destroy(this.gameObject);

            PlayerHit = true;
            IsAttacking = false;
        }

        //if ((collision.gameObject.tag == "projectile" || collision.gameObject.tag == "Grabbable") && Vector2.Distance(collision.gameObject.transform.position, this.transform.position) < 2f)
        //{
        //    events.OnDeath.Invoke();
        //    Destroy(this.gameObject);
        //}

    }


    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            
            player = collider.gameObject;

        }

        if ((collider.gameObject.tag == "projectile") && Vector2.Distance(collider.gameObject.transform.position, this.transform.position) < 1f)
        {
            this.health = this.health - 5f;
        }



    }

    public void OnTriggerStay2D(Collider2D collider)
    {


    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {

            player = null;
        }
    }

    //just for testing purpose
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, Range);
    }

    public void applyDamage()
    {
        //events.OnDeath.Invoke();

    }
}
