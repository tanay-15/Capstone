using Anima2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BasicEnemy {

    // Use this for initialization

    //components used
    protected Animator anim;
    protected Rigidbody rigi;
    private Rigidbody2D rigidbodyenem;

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
    public float chaseSpeed;
    public bool withinRange;
    
 
    private float losrange;
    public GameObject ImpactAnim;

    [Header("Brain")]
    public GameObject target;
    protected Vector3 targetpos;
    public bool AttackReady;
    public bool IsAlive ;
    bool IsPunched = false;
  
    public bool movway1 = true;

    public bool movway2;

    public bool checkforAttackHit;
    [HideInInspector]
    public GameObject MovePoint1;
    [HideInInspector]
    public GameObject MovePoint2;
    [HideInInspector]
    public Vector3 waypoint1;
    [HideInInspector]
    public Vector3 waypoint2;

    [HideInInspector]
    public LayerMask EnemyIgnoreMask;
    private LayerMask groundMask;
    
    public enum States
    {
        Idle,
        Patrol,
        Alert,
        Pursuit,
        Attack,
        Jumping,
        GetHit,
        Dead
    }
    [Header("States")]
    public States currentstate;

    [Header("Eyesight")]
    [HideInInspector]
    public GameObject vision;
    [HideInInspector]
    public GameObject groundAhead;
    [HideInInspector]
    public GameObject jumpAhead;
    private RaycastHit eyehit;
    [HideInInspector]
    public RaycastHit2D vishit;
    private RaycastHit2D groundHit;
    private RaycastHit2D jumpsideHit;
    private bool hasgroundAhead;
    private bool canJumpAhead;
    private bool m_HasPatrol;

    //Jump positions
    private Vector3 jump_position;
    private GameObject jump_up_origin;
    private Vector3 jump_up_position;
    private RaycastHit2D jump_up_hit;
 

    protected bool CollidedWithPlayer;
    Component[] bones;

   protected GameObject[] compobones;



   public override Vector3 lifebarOffset
   {
       get
       {
           return Vector3.up * 2f;
       }
   }



    void Start () {

        SetupValues();

      
       
    }

 

    public void SetupValues()
    {
        anim = this.GetComponent<Animator>();
        rigi = this.GetComponent<Rigidbody>();
        IsAlive = true;
        rigidbodyenem = this.GetComponent<Rigidbody2D>();
        bones = gameObject.transform.GetComponentsInChildren<Rigidbody2D>();


        chaseSpeed = movspeed + 1f;

        maxHealth = health;
        EnemyIgnoreMask = ~LayerMask.GetMask("Enemy");
        groundMask = LayerMask.GetMask("Ground");


        foreach (Transform child in transform)
        {
            if(child.name == "WaypointA")
            {
                MovePoint1 = child.gameObject;
            }
            if (child.name == "WaypointB")
            {
                MovePoint2 = child.gameObject;
            }
            if (child.name == "Vision")
            {
                vision = child.gameObject;
            }

            if(child.name == "JumpSelection")
            {
                jumpAhead = child.gameObject;
            }
            if(child.name == "GroundAhead")
            {
                groundAhead = child.gameObject;
            }

            if(child.name == "JumpUp")
            {
                jump_up_origin = child.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update () {

       
        if (health <= 0.0f)
        {
            IsAlive = false;
        }
        if (IsAlive)
        {
            switch (currentstate)
            {
                case States.Idle:
                    currentstate = States.Patrol;
                    break;
                case States.Patrol:
                    DetectingPlayer();
                    CheckForGroundAhead();
                    PartolHelper();
                    Patrol();
                    break;

                case States.Pursuit:
                    DetectingPlayer();
                    CheckForGroundAhead();
                    Pursuit();
                    break;

                case States.Attack:
                    Attack();
                    break;
            }
        }
        


     /*   if (IsAlive)
        {
            CheckForGroundAhead();
            DetectingPlayer();
            Patrol();
            Pursuit();
            DirectAttack();
            Attack();

         
        }*/
        else
        {
            Death();
        }
      
	}

    public void SetPatrolPos()     //Helper function for Patrol points
    {
        if (!m_HasPatrol)
        {
            waypoint1 = MovePoint1.transform.position;
            waypoint2 = MovePoint2.transform.position;
           

            m_HasPatrol = true;
        }
    }
    #region Ground Ahead Check
    void CheckForGroundAhead()
    {
       if(groundHit = Physics2D.Raycast(groundAhead.transform.position, -transform.up, 0.4f,groundMask))
        {
            hasgroundAhead = true;
        
        }
        else
        {
            hasgroundAhead = false;
          
        }
    }
    #endregion
   

    public void Death()
    {


        target = null;
        movway1 = false;
        movway2 = false;
        currentstate = States.Dead;
        GetComponent<SkeletonDeathScript>().Die();
        events.OnDeath.Invoke();
        anim.SetTrigger("Death");
        


    }

    public virtual void DetectingPlayer()
    {
        #region Previous Detection with raycasting
      /*  if (withinRange)
         {
             if (LookingLeft)
             {

                 if (vishit = Physics2D.Raycast(vision.transform.position, -transform.right, 10f, EnemyIgnoreMask))
                 {
                   
                     if (vishit.collider.gameObject.tag == "Player")
                     {
                         // anim.SetBool("Alert", true);
                         target = vishit.collider.gameObject;
                         targetpos = target.transform.position;
                         currentstate = States.Pursuit;
                     }
                 }

             }

             if (!LookingLeft)
             {
                 
                

                 if (vishit = Physics2D.Raycast(vision.transform.position, transform.right, 10f, EnemyIgnoreMask))
                 {
                   
                     if (vishit.collider.gameObject.tag == "Player")
                     {
                         // anim.SetBool("Alert", true);
                         target = vishit.collider.gameObject;
                         targetpos = target.transform.position;
                         currentstate = States.Pursuit;
                     }
                 }
             }
         }
         */
        #endregion

        if (withinRange)
        {
            targetpos = target.transform.position;
            currentstate = States.Pursuit;
        }

        else if (!withinRange)
        {
            target = null;
            currentstate = States.Patrol;
            
        }


     

    }

    public void PartolHelper()
    {
        if (movway1)
        {


          
            

            if (vishit = Physics2D.Raycast(vision.transform.position, -transform.right, 0.1f,EnemyIgnoreMask))
            {
                
                if (vishit.collider.gameObject.layer == 9)
                {

                    movway2 = true;
                    movway1 = false;
                }
            }

        }

        if (movway2)
        {
            

            if (vishit = Physics2D.Raycast(vision.transform.position, transform.right, 0.1f,EnemyIgnoreMask))
            {
              
                if (vishit.collider.gameObject.layer == 9)
                {

                    movway1 = true;
                    movway2 = false;
                }
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
                SetPatrolPos();
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
                m_HasPatrol = false;
                currentstate = States.Pursuit;
                CheckForFlip(target.transform.position);
                anim.SetBool("ShouldPursuit", true);
                if(Mathf.Abs(this.transform.position.y - target.transform.position.y) > 3f)
                {
                    currentstate = States.Patrol;
                    
                }

                if(Vector2.Distance(this.transform.position, target.transform.position) < 1f)
                {
                    currentstate = States.Attack;
                    anim.SetBool("Attack", true);
                }
                else
                {
                    if ((int)target.transform.position.y > (int)this.transform.position.y)
                    {
                        CheckJumpUp();
                    }

                    //if (hasgroundAhead)
                    //{
                    this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, movspeed * Time.deltaTime);
                       
                   // }

                    //else
                    //{
                      

                    //    CheckJumpSide();
                    
                   
                    //}

                   

                   
                }
               
            }
        }
    }

    void CheckJumpUp()
    {
        Debug.DrawRay(jump_up_origin.transform.position, -transform.up);
        
        if(jump_up_hit = Physics2D.Raycast(jump_up_origin.transform.position, -transform.up, 4f, EnemyIgnoreMask))
        {
            
            jump_up_position = jump_up_hit.point;



            if (jump_up_hit.collider.gameObject.tag == "ground")
            {


                if (Mathf.Approximately((int)target.transform.position.y, (int)jump_up_position.y))
                {



                    StartCoroutine(TeleportEnemy(jump_up_position, 1.5f));
                }
                

            }

            else
            {

            }
        }
    }

    void CheckJumpSide()
    {
       
        if(jumpsideHit = Physics2D.Raycast(jumpAhead.transform.position, -transform.up,12f,EnemyIgnoreMask))
        {


               
                jump_position = jumpsideHit.point;
        

            if (Mathf.Approximately((int)jump_position.y, (int)target.transform.position.y))
            {

                StartCoroutine(TeleportEnemy(jump_position, 1f));
            }
      
                   
                    

                   
                
              
                
            }
        
    }

    IEnumerator TeleportEnemy(Vector3 _jumpPosition,float _delayTime)
    {
        yield return new WaitForSeconds(_delayTime);
        this.transform.position = _jumpPosition;
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
            m_HasPatrol = false;
            rateofattack -= Time.deltaTime;
            if(rateofattack <= 0)
            {
                anim.SetBool("Attack", true);
                currentstate = States.Attack;
                
         
               
            }
           
        }
    }


    private void OnDrawGizmosSelected()
    {
        
       
        
    }
    public virtual void applyDamage(int damage)
    {
        
        this.health = this.health -  damage;

        events.OnTakeDamage.Invoke((float)this.health / (float)maxHealth);

        StartCoroutine(FlashRed());

    }

    public void AttackHit()
    {
        if (CollidedWithPlayer)
        {
            //target.gameObject.SendMessage("GetHit", -10f);
            rateofattack = 1f;
            if (PlayerLife.sharedInstance.currentLife <= 0)
            {
                withinRange = false;
                currentstate = States.Idle;
                Patrol();
            }

          
        }

        if (Vector2.Distance(this.transform.position, target.transform.position) > 1.5f)
        {
            currentstate = States.Idle;
            DetectingPlayer();
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
       
        if(collision.gameObject.tag == "Player")
        {
           
           CollidedWithPlayer = true;
          AttackReady = true;
        }
    }

 
    public virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            withinRange = true;
            target = collision.gameObject;


        }
    }

    public virtual void TriggerEnter2D(Collider2D collision)
    {
        if((/*collision.gameObject.tag == "projectile" || */collision.gameObject.tag == "Grabbable") && Vector3.Distance(collision.transform.position,transform.position) < 1.5f )
        {
            if (collision.GetType() == typeof(BoxCollider2D))
            { 
                
                    applyDamage(Random.Range(5,8));
                    if(collision.GetComponent<OrbitorObjectScript>())
                        collision.GetComponent<OrbitorObjectScript>().hit = true;
                    else if(collision.GetComponent<playerKnife2D>())
                        collision.GetComponent<playerKnife2D>().hasHit = true;
                //


                if (health > 0.0f)
                {
                    //Destroy(collision.gameObject);
                }

                var impact = Instantiate(ImpactAnim, collision.transform.position, Quaternion.identity);
                impact.gameObject.SetActive(true);

            }
        }

        if (collision.gameObject.name == "AttackTrigger" && !IsPunched && Vector3.Distance(collision.transform.position, transform.position) < 1.5f)
        {
            applyDamage(3);

            IsPunched = true;
            StartCoroutine(IsPunchedReset());

            var impact = Instantiate(ImpactAnim, new Vector2(transform.position.x, transform.position.y + 0.8f), Quaternion.identity);
            impact.gameObject.SetActive(true);

        }

        if (collision.gameObject.tag == "Player")
        {
           
            withinRange = true;
            target = collision.gameObject;


        }
    }

 

    //public void OnTriggerStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.name == "AttackTrigger" && Vector3.Distance(collision.transform.position, transform.position) < 1.5f)
    //    {
    //        applyDamage(5);
    //        if (health <= 0.0f)
    //            IsAlive = false;
    //        var impact = Instantiate(ImpactAnim, new Vector2(transform.position.x, transform.position.y + 0.8f), Quaternion.identity);
    //        impact.gameObject.SetActive(true);
    //    }
    //}

    public void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            CollidedWithPlayer = false;
            AttackReady = false;
        }
    }

    public void TriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            withinRange = false;
           
            currentstate = States.Idle;
           
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collider)
    {

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



    IEnumerator IsPunchedReset()
    {
        yield return new WaitForSeconds(0.25f);
        IsPunched = false;
    }

    IEnumerator FlashRed()
    {
        foreach (SpriteMeshInstance s in GetComponentsInChildren<SpriteMeshInstance>())
        {
            s.color = new Color(1,0,0,1);
        }
        yield return new WaitForSeconds(0.25f);
        foreach (SpriteMeshInstance s in GetComponentsInChildren<SpriteMeshInstance>())
        {
            s.color = new Color(1, 1, 1, 1);
        }
    }
  
}
