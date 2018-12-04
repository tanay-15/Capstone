using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BasicEnemy {

    // Use this for initialization

    //components used
    protected Animator anim;
    protected Rigidbody rigi;

    public bool LookingLeft;
    private bool shouldLookLeft;
    private bool shouldLookRight;
    private bool checkingFlips;

    protected int maxHealth;

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
    public bool IsAlive ;
    public bool movway1 = true;
    public bool movway2;

    public bool checkforAttackHit;
    public GameObject MovePoint1;
    public GameObject MovePoint2;
    private Vector3 waypoint1;
    private Vector3 waypoint2;
    
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
    private RaycastHit2D vishit;


    public bool CollidedWithPlayer;
    Component[] bones;

   public GameObject[] compobones;

   public override Vector3 lifebarOffset
   {
       get
       {
           return Vector3.up * 2f;
       }
   }



    void Start () {
		
        anim = this.GetComponent<Animator>();
        rigi = this.GetComponent<Rigidbody>();
        IsAlive = true;

        bones = gameObject.transform.GetComponentsInChildren<Rigidbody2D>();


        waypoint1 = MovePoint1.transform.position;
        waypoint2 = MovePoint2.transform.position;

        maxHealth = health;
    }

    // Update is called once per frame
    void Update () {

        if (IsAlive)
        {
            DetectingPlayer();
            Patrol();
            Pursuit();
            DirectAttack();
            Attack();

            //when player is respawned
            //if(Vector2.Distance(target.transform.position,this.transform.position)> 5f)
            //{
            //    target = null;
            //    currentstate = States.Idle;
            //    anim.SetBool("Attack", false);
            //}
        }
        else
        {
            Death();
        }
      
	}

    void DirectAttack()
    {
        if(target)
        {
            if (Vector2.Distance(target.transform.position, this.transform.position)<1f) {

                currentstate = States.Attack;
                anim.SetBool("Attack", true);
            }
        }
    }

    public void Death()
    {


        target = null;
        movway1 = false;
        movway2 = false;
             currentstate = States.Dead;
            anim.SetTrigger("Death");
        

      
    }

    public virtual void DetectingPlayer()
    {




        if (movway1)
        {


            if (vishit = Physics2D.Raycast(vision.transform.position, -transform.right, losrange))
            {
                if (vishit.collider.gameObject.tag == "Player")
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

        if (movway2)
        {
            if (vishit = Physics2D.Raycast(vision.transform.position, transform.right, losrange))
            {
                if (vishit.collider.gameObject.tag == "Player")
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


    }

    public virtual void Patrol()
    {


        anim.SetBool("Walking", true);
        anim.SetBool("Attack", false);
        if (IsAlive)
        {
            if (target == null)
            {
                // anim.SetTrigger("StartPatrol");
                currentstate = States.Patrol;
            }


            if (currentstate == States.Patrol)
            {
                if (movway1)
                {
                    CheckForFlip(waypoint1);
                    this.transform.position = Vector2.MoveTowards(this.transform.position, waypoint1, movspeed * Time.deltaTime);
                    if (Vector2.Distance(waypoint1, this.transform.position) <= 1f)
                    {
                        flip();
                        //move to point two
                        movway1 = false;
                        movway2 = true;
                    }
                }



                if (movway2)
                {
                    CheckForFlip(waypoint2);
                    this.transform.position = Vector2.MoveTowards(this.transform.position, waypoint2, movspeed * Time.deltaTime);
                    if (Vector2.Distance(waypoint2, this.transform.position) <= 1f)
                    {
                        flip();
                        movway1 = true;
                        movway2 = false;
                    }
                }

            }

          
        }
        
    }

    public virtual void Pursuit()
    {
        anim.SetBool("Walking", true);
        anim.SetBool("Attack", false);

        if (IsAlive)
        {

        
            if (target && !AttackReady)
            {
                currentstate = States.Pursuit;
                CheckForFlip(target.transform.position);
                anim.SetBool("ShouldPursuit", true);
                if(Mathf.Abs(this.transform.position.y - target.transform.position.y) > 3f)
                {
                    currentstate = States.Patrol;
                    
                }
                else
                {
                    this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, movspeed * Time.deltaTime);
                }
               
            }
        }
    }

    public void CheckForFlip(Vector3 _targetpos)
    {
        if (!checkingFlips)
        {
            if (_targetpos.x < this.transform.position.x)
            {
                shouldLookLeft = true;
                shouldLookRight = false;
                if (LookingLeft && shouldLookLeft)
                {
                    //ignore
                    checkingFlips = true;
                }

               else if(shouldLookLeft && !LookingLeft)
                {
                    flip();
                    checkingFlips = true;
                }


                
            }

          else if (_targetpos.x > this.transform.position.x)
            {
                shouldLookRight = true;
                shouldLookLeft = false;
                if(shouldLookRight && !LookingLeft)
                {
                    checkingFlips = true;
                }
                else if(shouldLookRight && LookingLeft)
                {
                    flip();
                    checkingFlips = true;
                }
               
            }
        }

        checkingFlips = false;
       
    }


    public virtual void Attack()
    {
        //Each type of enemy will have its seperate attack

        if (AttackReady)
        {
            
            rateofattack -= Time.deltaTime;
            if(rateofattack <= 0)
            {
                anim.SetBool("Attack", true);
                currentstate = States.Attack;
                
                //l.Log("Enemy now attacks the player");
               
            }
           
        }
    }


    private void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawWireSphere(this.transform.position,losrange);
        
    }
    public void applyDamage(int damage)
    {
        Debug.Log("Hit by arrow~!!");
        this.health = this.health -  damage;

        events.OnTakeDamage.Invoke((float)this.health / (float)maxHealth);
        //Temporary; health bar does not update properly
        if (this.health < 0) { events.OnDeath.Invoke(); }
      
    }

    public void AttackHit()
    {
        if (CollidedWithPlayer)
        {
            target.gameObject.SendMessage("GetHit", -10f);
            rateofattack = 2f;
        }
  
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Temporary. Let the Player Weapon GameObjects kill the enemy
        if (collision.gameObject.tag == "Player Weapon")
        {
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("GetHit", -5);
            CollidedWithPlayer = true;
            AttackReady = true;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.tag == "projectile" || collision.gameObject.tag == "Grabbable") && Vector3.Distance(collision.transform.position,transform.position) < 2f )
        {
            if(health <=0)
                IsAlive = false;
            else
            {
                applyDamage(5);
                Destroy(collision.gameObject);
            }
           

            //health = 0;
          
        }

        if(collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
            targetpos = collision.gameObject.transform.position;
           
        }
    }


    public void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            CollidedWithPlayer = false;
            AttackReady = false;
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            target = null;
            currentstate = States.Idle;
           
        }
    }

    public int GetHealth()
    {
        return health;
    }


    public bool GetAlive()
    {
        return IsAlive;
    }

  
    
  public  void flip()
    {

        LookingLeft = !LookingLeft;

        this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
      

      
        
    }
}
