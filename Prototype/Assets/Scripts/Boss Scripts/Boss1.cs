using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour {

    // Use this for initialization

    [Header("Player Details")]
    public GameObject player;

    [Header("Level Objects")]
    public GameObject plat1;
    public GameObject plat2;

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
    public float health;
    public bool hasBarrier = true;
    public int barrCounter = 3;
    public float barrierback = 10;
    private int tempvalue;
    public bool deciderparameter = false;
    private GameObject alertobject;
    public GameObject stunpref;
    private GameObject stunobj;
    public bool moveback = false;
    public Vector2 lowposition = new Vector2(0,1);


    [Header("Attack Dash")]
    private float dashattackcounter = 3f;
    public Vector3 targetposition;
    public GameObject alertpref;
    public GameObject alertpos;
    public bool shoulddash;
    public float afterattackcounter = 0;
    private float stuncounter = 5f;
    public bool isStunned;
    private bool indash;

 

    [Header("Attack Health Down")]
    public GameObject orbprefab;
    public GameObject orbloc1;
    public GameObject orbloc2;
    public GameObject orbloc3;
    public GameObject orbloc4;
    public GameObject orbloc5;
    public GameObject orbloc6;
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
    public bool playerdetected;

	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");

       
      
	}
	
	// Update is called once per frame
	void Update () {

        if(health > 50)
        {
            if ((!playerdetected) && (!isStunned))
            {
                Movement();
            }


            if (shoulddash)
            {
                Dash();
            }
        }

        if(health <= 0)
        {
            Death();
        }

        if(health <= 50 && !isStunned && !moveback)
        {
            FiftyMovement();
        }


        if (moveback)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, lowposition, 4f * Time.deltaTime);

            if (Vector2.Distance(this.transform.position, lowposition) < 0.4f)
            {
                moveback = false;
            }
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

        BelowFiFty();
       
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
                targetposition = player.transform.position;
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
            stuncounter = 3.5f;
        }
    }

    private void ThrowAttackAlert()
    {
        if (playerdetected)
        {
            
           
            if(health > 50)
            {

           
          
                //Throw attack alert
                Debug.Log("Dashing!");
              
                
                   alertobject = Instantiate(alertpref, alertpos.transform.position, alertpos.transform.rotation);
                   
                    shoulddash = true;

            }


        }
    }


    public void Death()
    {
        Destroy(this.gameObject);
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
               
                
                dashattackcounter = 1f;
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
            applyDamage(10);
        }

        if(collision.gameObject.tag == "Grabbable")
        {
            isStunned = true;
            applyDamage(15);
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "Player")
        {
            HitBack();
        }
    }

    private void HitBack()
    {
        if ((health <= 50) && (moveback == false))
        {

            if (this.transform.position.x <= player.transform.position.x)
            {
                Debug.Log("Hit left");
                moveback = true;
                lowposition.x = this.transform.position.x - 7f;
            }
            if (this.transform.position.x > player.transform.position.x)
            {
                Debug.Log("Hit right");
                moveback = true;
                lowposition.x = this.transform.position.x + 7f;
            }
        }
    }

    public void ShieldDownAttack()
    {
        //attack when shield is down

        Instantiate(orbprefab, orbloc1.transform.position, orbloc1.transform.rotation);
        Instantiate(orbprefab, orbloc2.transform.position, orbloc2.transform.rotation);
        Instantiate(orbprefab, orbloc3.transform.position,orbloc3.transform.rotation);
        Instantiate(orbprefab, orbloc4.transform.position, orbloc4.transform.rotation);
        Instantiate(orbprefab, orbloc5.transform.position, orbloc5.transform.rotation);
        Instantiate(orbprefab, orbloc6.transform.position, orbloc6.transform.rotation);
        
    }

    //Movement for boss below 50hp
    private void FiftyMovement()
    {
       if(health <= 50)
        {
            Vector2 bigpos = new Vector2(0,1);
            bigpos.x = player.transform.position.x;
            this.transform.position = Vector2.MoveTowards(this.transform.position, bigpos, 3f * Time.deltaTime);

           

            //attackdashcounter
            dashattackcounter = dashattackcounter - Time.deltaTime;
            indash = true;

            if (dashattackcounter <= 0)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, bigpos, 10 * Time.deltaTime);

                if (Vector2.Distance(bigpos, this.transform.position) < 0.3f)
                {
                    Debug.Log("Reached dash position");
                    shoulddash = false;
                    targetposition = Vector3.zero;


                    dashattackcounter = 3f;
                }
            }
        }
    }

   

    public void applyDamage(int damage)
    {
        Debug.Log("Boss has been attacked!");

        health = health - damage;
       
    }

    private void flip()
    {
        this.transform.RotateAround(transform.position, transform.up, 180f);
    }

    private void BelowFiFty()
    {
        //break the two platforms

        if(health <= 50)
        {
            attack2counter = attack2counter - Time.deltaTime;
            if(attack2counter <= 0)
            {
                ShieldDownAttack();
                Destroy(plat1);
                Destroy(plat2);
                attack2counter = 5f;
            }
       
        }

    }
}
