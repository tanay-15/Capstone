using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    // Use this for initialization

    //components used
    private Animator anim;
    private Rigidbody rigi;

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
    private Vector3 targetpos;
    public bool AttackReady;
    public bool IsAlive = true;
    
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
            Debug.Log("Player in range");
        }

        if(Physics.Raycast(vision.transform.position,transform.right,out vishit, losrange))
        {
            if(vishit.collider.gameObject.tag == "Player")
            {
                currentstate = States.Alert;
                anim.SetBool("Alert", true);
                target = vishit.collider.gameObject;
                targetpos = target.transform.position;
                Debug.Log("Player spotted");
            }

           
           
        }
        else
        {
            target = null;
            currentstate = States.Idle;
            anim.SetBool("Alert", false);
        }

    }

    public void Pursuit()
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
                Debug.Log("Enemy now attacks the player");
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
        
    }

    public void OnCollisionEnter(Collision collision)
    {
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
}
