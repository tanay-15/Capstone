using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSpider : MonoBehaviour {

    // Use this for initialization
    public GameObject player;
    private Animator anim;

    public GameObject position1;
    public GameObject position2;
    public GameObject position3;
    public GameObject position4;
    public GameObject position5;

    private Vector3 positionA;
    private Vector3 positionB;

    public Slider bossHealthBar;

    public enum States
    {
        Idle,
        GoToStart,
        DecidingAttack,
        Moving,
        Attack1,
        Attack2,
        Attack3,
        Dead

    }

    public States currentState;
    public float moveSpeed;
    public float attackdoneCounter;
    public bool AttackDone;

    public GameObject slingshotPrefab;

    public GameObject spiderlingPrefab;
    public GameObject location1;
    public GameObject location2;

    public Vector3 positionC;
    public Vector3 positionD;

    public bool testfac;
    private bool isAttacking;       // is currently using an attack
    public bool attackCounter;      // attack when counter hits 0
    public bool startCounter;

    public GameObject movepoint1;
    public GameObject movepoint2;
    private bool moveA = true;
    private bool moveB;

    private int attackvalue;
    public float attack2Counter = 0f;
    public int health;

    private bool Attack1Done;
 
    
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        anim = this.GetComponent<Animator>();
        positionA = position1.transform.position;
        positionB = position2.transform.position;
        this.transform.position = positionA;
        currentState = States.Moving;
	}
	
	// Update is called once per frame
	void Update () {

        Brain();
      
      Attack1();
        //Attack2();
        AttackDecider();
        Attack3();
        IsAttackDone();
        Movement();
        GoBackToPosition();

        if(health <= 0)
        {
            currentState = States.Dead;
            Destroy(this.gameObject);
        }

        bossHealthBar.value = health;
	}

    void Brain()
    {
        //attack 1 and movement keeps updating no matter what till death


        if (health <= 75)
        {
            //hp falls below 75
            // do all 3 attacks one after other --> attack2 first then attack3.
            //attack 3 spawn them once.
            //attack2 counter 5secs
            attack2Counter = attack2Counter - Time.deltaTime;
            if(attack2Counter <= 0)
            {
                if (currentState == States.Moving)
                {
                    currentState = States.Attack2;
                }
                Attack2();
                attack2Counter = 5f;
            }
            
        }


        if(health <= 40)
        {
            //hp falls below 40
            // do all 3 attacks
            //attack 2 counter 3 secs
            //attack 3 spawn them once
            attack2Counter = attack2Counter - Time.deltaTime;
            if (attack2Counter <= 0)
            {
                if (currentState == States.Moving)
                {
                    currentState = States.Attack2;
                }
                Attack2();
                attack2Counter = 3f;
            }
        }
    }
    void AttackDecider()
    {

        //Decide attack
        
    }
    void IsAttackDone()
    {
        if (AttackDone)
        {
            attackdoneCounter = attackdoneCounter - Time.deltaTime;
            if(attackdoneCounter <= 0)
            {
                AttackDone = false;
                currentState = States.Moving;
            }
        }

        if (Attack1Done)
        {
            attackdoneCounter = attackdoneCounter - Time.deltaTime;
            if (attackdoneCounter <= 0)
            {
                Attack1Done = false;
                currentState = States.GoToStart;
            }
        }
    }

    void GoBackToPosition()
    {
        if(currentState == States.GoToStart)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, positionA, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(this.transform.position, positionA) < 0.3f)
            {
                currentState = States.Moving;
                attackdoneCounter = 2.5f;
            }
        }
    }
    
    void Movement()
    {
        if(currentState == States.Moving)
        {
            attackdoneCounter = 2.5f;
            if (moveA)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, movepoint2.transform.position, 5f * Time.deltaTime);
                if(Vector2.Distance(this.transform.position,movepoint2.transform.position)< 0.7f)
                {
                    moveA = false;
                    moveB = true;
                }
            }

            if (moveB)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, movepoint1.transform.position, 5f* Time.deltaTime);
                if (Vector2.Distance(this.transform.position, movepoint1.transform.position) < 0.7f)
                {
                    moveA = true;
                    moveB = false;
                }
            }
        }
    }
    void Attack1()
    {   
        if(currentState == States.Attack1)
        {

            this.transform.position = Vector2.MoveTowards(this.transform.position, positionC, moveSpeed * Time.deltaTime);

            if(Vector2.Distance(this.transform.position,positionC)< 0.6f)
            {
                Attack1Done = true;
            
            }
        }
        
    }

    void Attack2()
    {
        if(currentState == States.Attack2 && AttackDone == false)
        {
            Instantiate(slingshotPrefab, position3.transform.position, position3.transform.rotation);
            Instantiate(slingshotPrefab, position5.transform.position, position5.transform.rotation);
            Instantiate(slingshotPrefab, position4.transform.position, position4.transform.rotation);
            AttackDone = true;
        }
    }


    void Attack3()
    {
        if (currentState == States.Attack3 && AttackDone == false){

            Instantiate(spiderlingPrefab, location1.transform.position, location1.transform.rotation);
            Instantiate(spiderlingPrefab, location2.transform.position, location2.transform.rotation);
            AttackDone = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)



    {
        if(collider.gameObject.tag == "Player")
        {
            //perform attack1
            Debug.Log("Player in attack range");
            if(attackdoneCounter > 0f)
            {
                positionC = position2.transform.position;
                currentState = States.Attack1;

            }

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "projectile")
        {
            health = health - 5;
        }

        if(collision.gameObject.tag == "Grabbable")
        {
            health = health - 10;
        }

        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("GetHit", -20);
        }
    }
}
