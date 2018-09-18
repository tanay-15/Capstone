using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour {

    // Use this for initialization

    [Header("Player Details")]
    public GameObject player;

    public enum State
    {
        Idle,
        Walk,
        AttackAlert,
        GetHit,
        Dead,
        Dash,
        Stun
        
    }
    [Header("Boss Details")]
    public State currentState;
    public bool hasBarrier = true;
    public int barrCounter = 3;
    public float barrierback = 10;
    private int tempvalue;
    public bool deciderparameter = false;
    private GameObject alertobject;
    public GameObject stunpref;
    private GameObject stunobj;


    [Header("Attack Dash")]
    private float dashattackcounter = 3f;
    public Vector3 targetposition;
    public GameObject alertpref;
    public GameObject alertpos;
    public bool shoulddash;
    private float afterattackcounter = 0;
    private float stuncounter = 5f;
    public bool isStunned;
    private bool indash;

 

    [Header("Attack Shield Down")]
    public GameObject orbprefab;
    public GameObject orbloc1;
    public GameObject orbloc2;
    public GameObject orbloc3;
    public GameObject orbloc4;
    public bool shouldattack2;
    public float attack2counter = 0;

    [Header("Boss movement")]
    public GameObject waypoint1;
    public GameObject waypoint2;
    private bool movetoA = true;
    private bool movetoB;
    public bool nearby;

    [Header("Boss vision")]
    public GameObject visionpoint;
    public RaycastHit2D vishit;
    private bool playerdetected;

	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");

       
      
	}
	
	// Update is called once per frame
	void Update () {


        if ((!playerdetected) && (!isStunned))
        {
            Movement();
        }


        if (shoulddash)
        {
            Dash();
        }

        if (isStunned)
        {
            Stunned();
        }

        if(targetposition == Vector3.zero)
        {
            if (playerdetected)
            {
                targetposition = player.transform.position;
            }
        }
            DetectPlayer();
        ThrowAttackAlert();
       
	}

    private void DetectPlayer()
    {
        vishit = Physics2D.Raycast(visionpoint.transform.position, transform.right, 10f);
        if(vishit == null)
        {
            playerdetected = false;
            Debug.Log("Player not in sight");
        }
        if(vishit   != null){
            if (vishit.collider.gameObject.tag == "Player")
            {
                Debug.Log("Player in sight");
                playerdetected = true;
            }
            else
            {
                playerdetected = false;
            }
        }
      
    }

    private void Stunned()
    {
        if(stunobj == null)
        {
            stunobj = Instantiate(stunpref,alertpos.transform.position,alertpos.transform.rotation);

        }
       

        stuncounter = stuncounter - Time.deltaTime;

        if(stuncounter <= 0)
        {
            isStunned = false;
            Destroy(stunobj);
            stuncounter = 5f;
        }
    }

    private void ThrowAttackAlert()
    {
        if (playerdetected)
        {
            
           
            afterattackcounter = afterattackcounter - Time.deltaTime;

          
                //Throw attack alert
                Debug.Log("Dashing!");
                if(afterattackcounter <= 0)
                {
                   alertobject = Instantiate(alertpref, alertpos.transform.position, alertpos.transform.rotation);
                   
                    shoulddash = true;
                  
                    afterattackcounter = 6f;
                
                
            }
        }
    }

private void Decider()
{


    //placeholder update method

    BarrierLogic();

    if (shouldattack2)
    {

        //shield is down, shadow orb attack
        attack2counter = attack2counter - Time.deltaTime;
        if (attack2counter <= 0)
        {
            ShieldDownAttack();
            attack2counter = 4;
        }


    }

    if (hasBarrier)
    {
        /* PlayerNearby();
         //shield is up, dark axe attack
         if (!nearby)
         {
             Movement();
         }

         else
         {
             Debug.Log("Player is nearby! check for his moment!");

             //Decider();

         }*/


    }
}

    public void PlayerNearby()
    {
        if(Vector2.Distance(player.transform.position,this.transform.position) < 7f)
        {
            nearby = true;
          
        }

        else
        {
            //flip to player's side.
            //take player position.
            //attack ready
            //dash
            //or aoe slash on both sides
            nearby = false;
        }
    }
    public void Dash()
    { 

        //detect player
        Destroy(alertobject);
        dashattackcounter = dashattackcounter - Time.deltaTime;
        indash = true;

        if(dashattackcounter <= 0)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetposition, 7 * Time.deltaTime);

            if (Vector2.Distance(targetposition, this.transform.position) < 0.3f)
            {
                Debug.Log("Reached dash position");
                shoulddash = false;
                targetposition = Vector3.zero;
               
                
                dashattackcounter = 6f;
            }
        }
      
    }

  

    public void Movement()
    {
       

        if (movetoA)
        {
          
            this.transform.position = Vector3.MoveTowards(this.transform.position, waypoint1.transform.position, 1f * Time.deltaTime);

            if (Vector3.Distance(waypoint1.transform.position, this.transform.position) <= 2f)
            {
               flip();
                movetoA = false;
                movetoB = true;
            }
        }

        if (movetoB)
        {
        
            this.transform.position = Vector3.MoveTowards(this.transform.position, waypoint2.transform.position, 1f * Time.deltaTime);

            if (Vector3.Distance(waypoint2.transform.position, this.transform.position) <= 2f)
            {
                flip();
                movetoA = true;
                movetoB = false;
            }
        }
    }
        
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "BreakablePlatform")
        {
            Debug.Log("Hit breakable platform");
            isStunned = true;
        }
    }


    public void ShieldDownAttack()
    {
        //attack when shield is down

        Instantiate(orbprefab, orbloc1.transform.position, orbloc1.transform.rotation);
        Instantiate(orbprefab, orbloc2.transform.position, orbloc2.transform.rotation);
        Instantiate(orbprefab, orbloc3.transform.position,orbloc3.transform.rotation);
        Instantiate(orbprefab, orbloc4.transform.position, orbloc4.transform.rotation);
        
    }



    public void BarrierLogic()
    {

        //logic for barrier
        if (!hasBarrier)
        {
            shouldattack2 = true;
            barrierback = barrierback - Time.deltaTime;
          

            if (barrierback <= 0)
            {
                barrCounter = 2;
                hasBarrier = true;
                barrierback = 10;
                Debug.Log("Barrier is back up!");
                shouldattack2 = false;
            }
        }
    }

    public void applyDamage(int damage)
    {
        Debug.Log("Boss has been attacked!");
        if (barrCounter >= 0)
        {
           
            Debug.Log("Barrier strength is : " + barrCounter);
            barrCounter--;
        }

        if(barrCounter < 0)
        {
            Debug.Log("Barrier down!");
            hasBarrier = false;
        }
       
    }

    private void flip()
    {
        this.transform.RotateAround(transform.position, transform.up, 180f);
    }
}
