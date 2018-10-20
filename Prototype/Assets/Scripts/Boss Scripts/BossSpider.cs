using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
   
 
    
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        anim = this.GetComponent<Animator>();
        positionA = position1.transform.position;
        positionB = position2.transform.position;
        this.transform.position = positionA;
	}
	
	// Update is called once per frame
	void Update () {

      
      Attack1();
        Attack2();
        AttackDecider();
        Attack3();
        IsAttackDone();
        Movement();
        GoBackToPosition();
	}

    void AttackDecider()
    {

        //Decide attack
        if (testfac)
        {
          
            testfac = false;
        }
        

    }
    void IsAttackDone()
    {
        if (AttackDone)
        {
            attackdoneCounter = attackdoneCounter - Time.deltaTime;
            if(attackdoneCounter <= 0)
            {
                AttackDone = false;
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
                currentState = States.Idle;
                attackdoneCounter = 4f;
            }
        }
    }
    
    void Movement()
    {
        if(currentState == States.Moving)
        {
            if (moveA)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, movepoint2.transform.position, 2f * Time.deltaTime);
                if(Vector2.Distance(this.transform.position,movepoint2.transform.position)< 0.7f)
                {
                    moveA = false;
                    moveB = true;
                }
            }

            if (moveB)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, movepoint1.transform.position, 2f * Time.deltaTime);
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
                AttackDone = true;
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
            if(attackdoneCounter == 4)
            {
                positionC = position2.transform.position;
                currentState = States.Attack1;

            }

        }
    }
}
