using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonBoss : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject leftpoint;
    public GameObject rightpoint;
    public GameObject startpoint;
    public GameObject AttackPoint;
    public GameObject middlePoint;
    public float MoveSpeed;
    public float Health;

    public GameObject skullspawnPoint;
    public GameObject spawnPointLeft;
    public GameObject spawnPointRight;

    public bool moveToleftPoint = true;
    public bool moveTorightPoint;
    public bool moveToMiddlePoint;

    
    public Vector3 leftpointpos;
    public Vector3 rightpointpos;
    public Vector3 middlePointpos;
    public Vector3 spawnPointLeftPos;
    public Vector3 spawnPointRightPos;
    public Vector3 attackPointpos;
    private Vector3 startpointpos;

    public GameObject skeletonPrefab;

    public GameObject flyingSkullPrefab;

    private float flyspawnCounter = 0f;
    private float skelspawnCounter = 0f;

    public float AttackCounter = 10f;
    public bool AttackDone = false;

    public bool IsPunched;

    public GameObject ImpactAnim;

    public Slider BossHealthBar;


    public GameObject player;

    public enum BossStates
    {
        Idle,
        AttackReady,
        Attacking
    }

    public BossStates currentState;

    private Animator anim;

    public List<GameObject> skeletons;
    public GameObject flyskull1;
    public GameObject skel1;
    public GameObject skel2;

    public bool IsInDemonMode = false;
    public bool IsInHumanMode = false;
    public bool PlayerMode = false;

    
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        this.transform.position = startpoint.transform.position;

        leftpointpos = leftpoint.transform.position;
        rightpointpos = rightpoint.transform.position;
        middlePointpos = middlePoint.transform.position;


        startpointpos = startpoint.transform.position;
        spawnPointLeftPos = spawnPointLeft.transform.position;
        spawnPointRightPos = spawnPointRight.transform.position;
        attackPointpos = AttackPoint.transform.position;
        anim = this.GetComponent<Animator>();
        currentState = BossStates.Idle;

    }

    

    // Update is called once per frame
    void Update()
    {
        DeterminingPlayerMode();
        Movement();
    
        BaseAttack();
        Attack1();
        Attack2();

        HealthUpdate();
        if (AttackDone)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, startpointpos, (MoveSpeed + 2.5f) * Time.deltaTime);

            if(Vector2.Distance(this.transform.position, startpointpos) < 1.5f)
            {
                AttackDone = false;
            }

        }
        Death();
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -2.0f);
    }


    void DeterminingPlayerMode()
    {
        if(player != null)
        {
            PlayerMode = player.transform.Find("Human").gameObject.activeSelf;

            if (PlayerMode)
            {
                IsInHumanMode = true;
                IsInDemonMode = false;
            }

            else
            {
                IsInDemonMode = true;
                IsInHumanMode = false;
            }
        }
    }

    void BaseAttack()
    {
        AttackCounter = AttackCounter - Time.deltaTime;
        if(AttackCounter <= 0)
        {

            currentState = BossStates.AttackReady;
            anim.SetBool("AttackReady",true);
            this.transform.position = Vector2.MoveTowards(this.transform.position, attackPointpos, (MoveSpeed + 1.5f) * Time.deltaTime);
          
            if (Vector2.Distance(this.transform.position, attackPointpos) < 1.5f)

            {
                anim.SetBool("AttackReady", false);
                currentState = BossStates.Attacking;
                anim.SetBool("Attack", true);

                if (IsInHumanMode)
                {
                    AttackCounter = 6f;
                    //Original MoveSpeed is 3
                    MoveSpeed = 3f;
                }

                if (IsInDemonMode)
                {
                    AttackCounter = 4f;

                    MoveSpeed = 4.5f;
                }
               
            }
                
            
        
        }

     
    }

    void HealthUpdate()
    {
       
        BossHealthBar.value = this.Health;
        if(this.Health < 0)
        {
            Destroy(BossHealthBar);
        }
    }

    public void AttackOver()
    {
        Debug.Log("In here");

      
        AttackDone = true;
        currentState = BossStates.Idle;
        anim.SetBool("Attack", false);
    }


    void Attack1()
    {
        if(this.Health <= 70f)
        {
            if(skelspawnCounter <= 0)
            {
                Attack_Resurect();

                if (IsInDemonMode)
                {
                    //Will spawn an extra skeleton warrior

                    Instantiate(skeletonPrefab, new Vector3(spawnPointLeftPos.x + 5f, spawnPointLeftPos.y, spawnPointLeftPos.z), Quaternion.identity);
                }
            }

            if(skeletons.Count > 0)
            {
                if (skel1 == null)
                {
                    skeletons.Remove(skel1);
                }

                if (skel2 == null)
                {
                    skeletons.Remove(skel2);
                }
            }
         

        }

        skelspawnCounter = skelspawnCounter - Time.deltaTime;
    }

    void Attack2()
    {
        if(this.Health <= 40f)
        {
            if(flyspawnCounter <=0)
            {
                Flying_Skull_Resurect();
            }

            flyspawnCounter = flyspawnCounter - Time.deltaTime;
        }
    }


    void Movement()
    {
        
        if (moveToleftPoint)
        {
            currentState = BossStates.Idle;
            this.transform.position = Vector2.MoveTowards(this.transform.position, leftpointpos, MoveSpeed * Time.deltaTime);

            if(Vector2.Distance(this.transform.position,leftpointpos) < 2f)
            {
                moveToleftPoint = false;
                moveTorightPoint = true;
                moveToMiddlePoint = false;
            }
        }

        if (moveTorightPoint)
        {
            currentState = BossStates.Idle;
            this.transform.position = Vector2.MoveTowards(this.transform.position, rightpointpos, MoveSpeed * Time.deltaTime);

            if(Vector2.Distance(this.transform.position,rightpointpos) < 2f)
            {
                moveToleftPoint = false;
                moveTorightPoint = false;
                moveToMiddlePoint = true;
            }

        }

        if (moveToMiddlePoint)
        {
            currentState = BossStates.Idle;
            this.transform.position = Vector2.MoveTowards(this.transform.position, middlePointpos, MoveSpeed * Time.deltaTime);

            if(Vector2.Distance(this.transform.position, middlePointpos) < 1f)
            {
                moveToleftPoint = true;
                moveToMiddlePoint = false;
                moveTorightPoint = false;
            }
        }

       
    }

    void applyDamage(int amount)
    {
        this.Health = this.Health - amount;
    }


    void Attack_Resurect()
    {
        //Spawn 2 skeletons: one at left point, one at right point
        //Need to check how many skeletons are there
        if(skel1 == null && skel2 == null)
        {
            skel1 = Instantiate(skeletonPrefab, spawnPointLeftPos, Quaternion.identity);
            skel2 = Instantiate(skeletonPrefab, spawnPointRightPos, Quaternion.identity);
           skeletons.Add(skel1);
            skeletons.Add(skel2);
        }

      

        skelspawnCounter = 12f;
    }


    void Flying_Skull_Resurect()
    {
        if(flyskull1 == null)
        {
            flyskull1 = Instantiate(flyingSkullPrefab, skullspawnPoint.transform.position, Quaternion.identity);
        }
        

        flyspawnCounter = 14f;
    }

    void Death()
    {
        if(this.Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.gameObject.tag == "Player")
        //{
        //    Debug.Log("Collided with player");
        //    collision.gameObject.SendMessage("GetHit", -10f);

            

        //}

        if (collision.gameObject.name == "AttackTrigger" && !IsPunched && Vector3.Distance(collision.transform.position, transform.position) < 2.2f && collision.gameObject.layer != 15)
        {
            applyDamage(13);

            IsPunched = true;
            StartCoroutine(IsPunchedReset());

            var impact = Instantiate(ImpactAnim, new Vector2(transform.position.x, transform.position.y + 0.8f), Quaternion.identity);
            impact.gameObject.SetActive(true);

        }

        if(collision.gameObject.tag == "projectile" && Vector3.Distance(collision.transform.position,this.transform.position) < 2.2f)
        {
            applyDamage(15);

            var impact = Instantiate(ImpactAnim, new Vector2(this.transform.position.x, this.transform.position.y + 0.8f), Quaternion.identity);
            impact.gameObject.SetActive(true);
        }

       



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Grabbable")
        {
            //this.Health = this.Health - 15;
            applyDamage(15);

            var impact = Instantiate(ImpactAnim, new Vector2(this.transform.position.x, this.transform.position.y + 0.8f), Quaternion.identity);
            impact.gameObject.SetActive(true);
        }
    }

    IEnumerator IsPunchedReset()
    {
        yield return new WaitForSeconds(0.25f);
        IsPunched = false;
    }
}
