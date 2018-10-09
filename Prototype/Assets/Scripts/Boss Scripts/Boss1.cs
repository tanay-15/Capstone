using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour {

    // Use this for initialization

    [Header("Player Details")]
    public GameObject player;
    public LayerMask playerlayer;

    public Camera cam;

 

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
   
    public bool deciderparameter = false;
    private GameObject alertobject;
    public GameObject stunpref;
    private GameObject stunobj;
    public bool moveback = false;
    public Vector2 lowposition = new Vector2(0,0.67f);


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


    [Header("Attack Ground Smash")]
    public GameObject gsm_prefab;
    public GameObject gsm_loc;
    private float gsmattackcounter = 1.5f;

    //Decides which attack to use
    [Header("Attack Decider")]
    public int attackdecider;
    private bool should_attack_dash;
    private bool should_attack_gsm;

  
   

    [Header("Boss movement")]
    public GameObject waypoint1;
    public GameObject waypoint2;
    private bool movetoA = true;
    private bool movetoB;
 
    [Header("Boss vision")]
    public GameObject visionpoint;
    public RaycastHit2D vishit;
    public bool playerdetected;


    [Header("Destroy Platforms")]
    public GameObject hp_ninety;
    public GameObject hp_eighty;
    public GameObject hp_seventy;
    public GameObject hp_sixty;
    public GameObject hp_fifty;
 


    private bool facingLeft;
    public bool knocked;

    public GameObject dashpt1;
    public GameObject dashpt2;

    public bool Attackdone = true;

    private float nextattackcd = 4f;

	void Start () {

       // player = GameObject.FindGameObjectWithTag("Player");

        facingLeft = true;

      

       
      
	}
	
	// Update is called once per frame
	void Update () {

        //health is above 50
        if (health > 50)
        {
            //its alive
            if (!shoulddash && !playerdetected && !isStunned)
            {
                Movement(1.5f);
            }

            if (isStunned)
            {
                Stunned();
            }
            if (nextattackcd <= 0)
            {

       
                if (!isStunned)
                {
                    DetectPlayer();
                    MechDash(8f);
                }

            }
        }

            //Health falls below 50

            if(health <= 50)
            {
                if (!shoulddash && !playerdetected && !isStunned && Attackdone)
                {
                    Movement(4f);
                }

                if (isStunned)
                {
                    Stunned();
                }

                if (!isStunned)
                {
                    DetectPlayer();


                if (nextattackcd <= 0)
                {


                if (Attackdone)
                {
                    CheckForPlayer();
                    AttackDecider();
                }

                if (should_attack_dash)
                {
                    
                    MechDash(13f);
                }

                }
            }

           

            }


            if (health <= 0)
            {
                Death();

            }

        nextattackcd = nextattackcd - Time.deltaTime;

        
    }

   

    private void MechDash(float speed)
    {
        if (shoulddash)
        {
            if (facingLeft)
            {
                //charge at dash pt 1
                targetposition = dashpt1.transform.position;
               targetposition.y = 0.67f;
                this.transform.position = Vector2.MoveTowards(this.transform.position, targetposition, speed * Time.deltaTime);

                if(Vector2.Distance(this.transform.position,targetposition) < 0.08f)
                {
                    shoulddash = false;
                    Attackdone = true;
                    should_attack_dash = false;
                    nextattackcd = 4f;
                }
            }

            if (!facingLeft)
            {
                targetposition = dashpt2.transform.position;
                targetposition.y = 0.67f;
                this.transform.position = Vector2.MoveTowards(this.transform.position, targetposition, speed * Time.deltaTime);

                if (Vector2.Distance(this.transform.position, targetposition) < 0.08f)
                {
                    shoulddash = false;
                    Attackdone = true;
                    should_attack_dash = false;
                    nextattackcd = 4f;
                }
            }
        }
    }

    private void PreviousUpdate()
    {
        //if alive do something

        /*
        if (health > 50)
        {
            if(!isStunned)
              
            Movement();
            ThrowAttackAlert();



            if (shoulddash)
            {
                Dash(7f);
            }

        }


        if (health < 50)
        {
            //start using second attack with first

          

            if (!isStunned)
            {
                Movement();
              
                //decide what to use
                //AttackDecider();
                Invoke("AttackDecider", 2f);
                if (shoulddash)
                {
                    Dash(12f);
                }

                if (should_attack_gsm)
                {
                    GroundSmash();
                    cam.GetComponent<Boss1Camera>().SmashCamRotate();
                }
            }
        }

        //Destroy objects depending upon health

        if (health <= 90)
        {
            Destroy(hp_ninety);

        }

        if(health <= 80)
        {
            Destroy(hp_eighty);
        }

        if(health <= 70)
        {
            Destroy(hp_seventy);
        }

        if(health <= 60)
        {
            Destroy(hp_sixty);
        }

        if(health <= 50)
        {
            Destroy(hp_fifty);
        }


        if(health <= 0)
        {
            Death();
        }



        if (!isStunned)
        {
            DetectPlayer();
        }

      


        if (isStunned)
        {
            playerdetected = false;
            shoulddash = false;
            Stunned();
        }

   

        /* if (targetposition == Vector3.zero)
         {
             if (playerdetected)
             {
                 targetposition = player.transform.position;
             }
         }*/

    }

    private void GroundSmash()
    {
        Instantiate(gsm_prefab, gsm_loc.transform.position, gsm_loc.transform.rotation);
        should_attack_gsm = false;
    }


    private void AttackDecider()
    {

        if (Attackdone)
        {


            attackdecider = (int)Random.Range(1, 3);

            if (attackdecider <= 1)
            {
                //dash attack
                should_attack_gsm = false;
                Debug.Log("Chose to dash");
                should_attack_dash = true;
                Attackdone = false;
                
                
            }

            if (attackdecider > 1)
            {
                should_attack_gsm = true;
                shoulddash = false;
                //groundsmash
                Attackdone = false;
                GroundSmash();

            }

        }
       
      

    }

    private void DetectPlayer()
    {
        vishit = Physics2D.Raycast(visionpoint.transform.position, -transform.right, 10f);
        
        Debug.DrawRay(visionpoint.transform.position, -transform.right,Color.red);

        if (vishit == null)
        {
            playerdetected = false;
        

        }

      

        if(vishit.collider.gameObject.CompareTag("Player"))
        {
            playerdetected = true;
            targetposition = player.transform.position;
            shoulddash = true;
        }

        else
        {
            playerdetected = false;
         
        }
        

        

    }

    private void CheckForPlayer()
    {
        if (facingLeft)
        {
            if(player.transform.position.x > this.transform.position.x)
            {
                flip();
            }
        }

        if (!facingLeft)
        {
            if(player.transform.position.x < this.transform.position.x)
            {
                flip();
            }
        }
    }

  

    private void Stunned()
    {

        Attackdone = true;
        if(stunobj == null)
        {
            stunobj = Instantiate(stunpref,alertpos.transform.position,alertpos.transform.rotation);

        }

        if (health <= 0)
        {
            Destroy(stunobj);
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
            
           
           

           
          
                //Throw attack alert
                Debug.Log("Dashing!");
              
                
                   alertobject = Instantiate(alertpref, alertpos.transform.position, alertpos.transform.rotation);
                   
                    shoulddash = true;

            


        }
    }


    public void Death()
    {
        Destroy(this.gameObject);
    }

   
    public void Dash(float speed)
    { 

        //detect player
        Destroy(alertobject);
       // dashattackcounter = dashattackcounter - Time.deltaTime;
        indash = true;

       // if(dashattackcounter <= 0 && !isStunned)
       // {
            targetposition.y = 0.67f;
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetposition, speed * Time.deltaTime);

            if (Vector3.Distance(targetposition, this.transform.position) < 0.3f)
            {
                Debug.Log("Reached dash position");
                shoulddash = false;
                targetposition = Vector3.zero;
               
                
                dashattackcounter = 1f;
            }
       // }
      
    }

  

    public void Movement(float speed)
    {

       
        if (movetoA)
        {
           
            this.transform.position = Vector2.MoveTowards(this.transform.position, waypoint1.transform.position, speed * Time.deltaTime);

            if (Vector2.Distance(waypoint1.transform.position, this.transform.position) <= 1f)
            {
               flip();
                movetoA = false;
                movetoB = true;
            }
        }

        if (movetoB)
        {
       
            this.transform.position = Vector2.MoveTowards(this.transform.position, waypoint2.transform.position, speed * Time.deltaTime);

            if (Vector2.Distance(waypoint2.transform.position, this.transform.position) <= 1f)
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
            shoulddash = false;
            if (should_attack_dash)
            {
                Attackdone = true;
            }
          
            
        }

        if(collision.gameObject.tag == "projectile")
        {
            
            float hitFrom = collision.gameObject.GetComponent<playerKnife2D>().GetPosX();
          
            health = health - 2f;
            knocked = true;
          
            KnockBack(hitFrom);
        }

        if(collision.gameObject.tag == "Wall")
        {
            shoulddash = false;
            Attackdone = true;
        }
    }

    private void KnockBack(float hitFrom)
    {
        Vector2 knockbackpos;

        if(hitFrom < this.transform.position.x)
        {
            //Hit from left,,,move to right

            knockbackpos = new Vector2(this.transform.position.x + 0.8f,0.67f);

            this.transform.position = Vector3.MoveTowards(this.transform.position, knockbackpos, 1.5f );
            Invoke("KnockedChange", 1f);

        }

        if (hitFrom > this.transform.position.x)
        {
            //hit from right, move to left

            knockbackpos = new Vector2(this.transform.position.x - 0.8f, 0.64f);
            this.transform.position = Vector2.MoveTowards(this.transform.position, knockbackpos, 1.5f);
            Invoke("KnockedChange", 1f);

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

    private void KnockedChange()
    {
        knocked = false;
    }

  
    public float GetHealth()
    {
        return health;
    }
   

    public void applyDamage(int damage)
    {
        Debug.Log("Boss has been attacked!");

        health = health - damage;
       
    }


    public void SetAttackDone(bool attdone)
    {
        Attackdone = attdone;
        nextattackcd = 2f;
    }
    private void flip()
    {
        facingLeft = !facingLeft;
        this.transform.RotateAround(transform.position, transform.up, 180f);
    }

}
