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
    public Vector3 targetposition;
    private bool hasTargetPos = false;

    public bool GetPlayerPost = false;

    public float AttackCounter = 0f;

    public int health = 10;
    protected int maxHealth;

    public bool PlayerHit = false;
    public bool spawn = true;



    void Start()
    {

        pointA = waypoints[0].transform.position;
        pointB = waypoints[1].transform.position;
        //pointC = waypoints[2].transform.position;
        this.transform.position = pointA;
        moveA = true;
        this.maxHealth = this.health;

        foreach (Rigidbody2D rb2d in GetComponentsInChildren<Rigidbody2D>())
        {
            if (rb2d.name != name)
            {
                rb2d.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;

            }
        }
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
            SettingTarget();
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
            foreach (Rigidbody2D rb2d in GetComponentsInChildren<Rigidbody2D>())
            {
                if (rb2d.name != name)
                {
                    rb2d.gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                    rb2d.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    rb2d.transform.parent = null;
                    Vector2 direction = rb2d.position - (Vector2)transform.position;
                    direction.Normalize();
                    rb2d.AddForce(direction * 8.0f, ForceMode2D.Impulse);
                    
                    Destroy(rb2d.gameObject,5.0f);
                    if(player && rb2d.gameObject.GetComponent<PolygonCollider2D>())
                        Physics2D.IgnoreCollision(rb2d.gameObject.GetComponent<PolygonCollider2D>(),player.GetComponent<CapsuleCollider2D>());

                }
            }

            events.OnDeath.Invoke();
            Destroy(this.gameObject);
        }

    }


    public void SettingTarget()
    {
        if (!hasTargetPos)
        {
            targetposition = player.transform.position;
            hasTargetPos = true;
        }
    }
    public void AttackBat()
    {

      

        if (IsAttacking)
        {

           {
                if (!hasTargetPos)
                {
                    targetposition = player.transform.position;
                    hasTargetPos = true;
                }


            }

            if (hasTargetPos)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, targetposition, attackspeed * Time.deltaTime);

                if (Vector2.Distance(this.transform.position, targetposition) < 1f)
                {

                    IsAttacking = false;
                    AttackCounter = 8f;
                    targetposition = Vector3.zero;
                    hasTargetPos = false;
                    moveA = true;
                }
            }
        }


     

    


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
                hasTargetPos = false;
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
                hasTargetPos = false;
            }
        }



    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //attack player
            collision.gameObject.SendMessage("GetHit", -15);
          
            PlayerHit = true;
            IsAttacking = false;
        }

       
    }


    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            
            player = collider.gameObject;

        }

        //if ((collider.gameObject.tag == "projectile") && Vector2.Distance(collider.gameObject.transform.position, this.transform.position) < 1f)
        //{
        //    applyDamage(5);
        //}



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

    public void applyDamage(int damage)
    {
        //events.OnDeath.Invoke();
        this.health = this.health - damage;
        events.OnTakeDamage.Invoke((float)this.health / (float)maxHealth);
    }


    public void EnableLifebar(bool enable)
    {
        enableLifebar = enable;
    }

    public void Spawn()
    {
        spawn = true;
    }
}
