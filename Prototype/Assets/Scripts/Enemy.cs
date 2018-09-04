using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    // Use this for initialization

    //components used
    protected Animator anim;
    protected Rigidbody rigi;

    [Header("Attributes")]
    public int health;
    public float rateofattack = 2f;
    public float range;
    public float damage;
    public float movspeed;
    public enum World
    {
        Hero, Demon
    }
    public World InWorld;
    public float losrange;

    [Header("Brain")]
    public GameObject target;
    protected Vector3 targetpos;
    public bool AttackReady;
    public bool IsAlive = true;
    private bool movway1 = true;
    private bool movway2;
    public GameObject waypoint1;
    public GameObject waypoint2;
    
    public enum States
    {
        Idle,
        Patrol,
        Alert,
        Pursuit,
        Attack,
        GetHit,
        Dead
    }
    [Header("States")]
    public States currentstate;

    [Header("Eyesight")]
    public GameObject vision;
    public RaycastHit eyehit;
    private RaycastHit vishit;


    void Start () {
		
        anim = this.GetComponent<Animator>();
        rigi = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        if (IsAlive)
        {
            DetectingPlayer();
            Patrol();
            Pursuit();
            Attack();
        }
       
        Death();
	}

    public void Death()
    {
        if (AttackReady)
        {
            if (Input.GetMouseButton(0))
            {
                currentstate = States.Dead;
                IsAlive = false;
                anim.SetTrigger("Death");
            }
        }
    }

    public void DetectingPlayer()
    {
        
        if(Physics.Raycast(vision.transform.position,transform.right,out eyehit, range))
        {
            //Debug.Log("Player in range");
        }

        if(Physics.Raycast(vision.transform.position,transform.right,out vishit, losrange))
        {
            if(vishit.collider.gameObject.tag == "Player")
            {
                currentstate = States.Alert;
                anim.SetBool("Alert", true);
                target = vishit.collider.gameObject;
                targetpos = target.transform.position;
                //Debug.Log("Player spotted");
            }

           
           
        }
        else
        {
            target = null;
            currentstate = States.Idle;
            anim.SetBool("Alert", false);
        }

    }

    public virtual void Patrol()
    {
        if(target == null)
        {
            anim.SetTrigger("StartPatrol");
            currentstate = States.Patrol;
        }

        
        if(currentstate == States.Patrol)
        {
            if (movway1)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, waypoint1.transform.position, movspeed * Time.deltaTime);
                if (Vector2.Distance(waypoint1.transform.position, this.transform.position) <= 1f)
                {
                    flip();
                    //move to point two
                    movway1 = false;
                    movway2 = true;
                }
            }
            
           

            if (movway2)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, waypoint2.transform.position, movspeed * Time.deltaTime);
                if(Vector2.Distance(waypoint2.transform.position,this.transform.position) <= 1f)
                {
                    flip();
                    movway1 = true;
                    movway2 = false;
                }
            }
           
        }
        
    }

    public virtual void Pursuit()
    {
        if (target && !AttackReady)
        {
            currentstate = States.Pursuit;
            anim.SetBool("ShouldPursuit", true);
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetpos, movspeed * Time.deltaTime);
        }
    }

    public virtual void Attack()
    {
        //Each type of enemy will have its seperate attack

        if (AttackReady)
        {
            rateofattack -= Time.deltaTime;
            if(rateofattack <= 0)
            {
                currentstate = States.Attack;
                anim.SetBool("ShouldPursuit", false);
                anim.SetTrigger("Attack");
                //Debug.Log("Enemy now attacks the player");
                rateofattack = 2f;
            }
           
        }
    }


    private void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawWireSphere(this.transform.position,losrange);
        
    }
    public void applyDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            //temporary
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Temporary. Let the Player Weapon GameObjects kill the enemy
        if (collision.gameObject.tag == "Player Weapon")
        {
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Player")
        {
            
            AttackReady = true;
           
            
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            AttackReady = false;
        }
    }

    void flip()
    {

        /*Vector3 theScale = transform.localScale;
         theScale.x *= -1;
         transform.localScale = theScale;

         Vector3 visScale = vision.transform.localScale;
         visScale.x *= -1;
         vision.transform.localScale = visScale;
        */

        this.transform.RotateAround(transform.position, transform.up, 180f);

        
    }
}
