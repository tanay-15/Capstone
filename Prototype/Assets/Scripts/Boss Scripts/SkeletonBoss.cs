using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoss : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject leftpoint;
    public GameObject rightpoint;
    public GameObject startpoint;
    public GameObject AttackPoint;
    public float MoveSpeed;
    public float Health;

    public GameObject skullspawnPoint;
    public GameObject spawnPointLeft;
    public GameObject spawnPointRight;

    public bool moveToleftPoint = true;
    public bool moveTorightPoint;

    
    public Vector3 leftpointpos;
    public Vector3 rightpointpos;
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

    private Animator anim;

    
    void Start()
    {
        this.transform.position = startpoint.transform.position;

        leftpointpos = leftpoint.transform.position;
        rightpointpos = rightpoint.transform.position;

        startpointpos = startpoint.transform.position;
        spawnPointLeftPos = spawnPointLeft.transform.position;
        spawnPointRightPos = spawnPointRight.transform.position;
        attackPointpos = AttackPoint.transform.position;
        anim = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    
        BaseAttack();
        Attack1();
        Attack2();

        if (AttackDone)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, startpointpos, 4f * Time.deltaTime);

            if(Vector2.Distance(this.transform.position, startpointpos) < 1.5f)
            {
                AttackDone = false;
            }

        }
        Death();

    }

    void BaseAttack()
    {
        AttackCounter = AttackCounter - Time.deltaTime;
        if(AttackCounter <= 0)
        {

            this.transform.position = Vector2.MoveTowards(this.transform.position, attackPointpos, 4f * Time.deltaTime);

            if(Vector2.Distance(this.transform.position, attackPointpos) < 1.5f)
            {
                anim.SetBool("Attack", true);
                AttackCounter = 10f;
            }
                
            
        
        }

     
    }

    public void AttackOver()
    {
        Debug.Log("In here");

      
        AttackDone = true;
        anim.SetBool("Attack", false);
    }


    void Attack1()
    {
        if(this.Health <= 70f)
        {
            if(skelspawnCounter <= 0)
            {
                Attack_Resurect();
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
            this.transform.position = Vector2.MoveTowards(this.transform.position, leftpointpos, MoveSpeed * Time.deltaTime);

            if(Vector2.Distance(this.transform.position,leftpointpos) < 2f)
            {
                moveToleftPoint = false;
                moveTorightPoint = true;
            }
        }

        if (moveTorightPoint)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, rightpointpos, MoveSpeed * Time.deltaTime);

            if(Vector2.Distance(this.transform.position,rightpointpos) < 2f)
            {
                moveToleftPoint = true;
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

        Instantiate(skeletonPrefab, spawnPointLeftPos, Quaternion.identity);
        Instantiate(skeletonPrefab, spawnPointRightPos, Quaternion.identity);

        skelspawnCounter = 12f;
    }


    void Flying_Skull_Resurect()
    {
        Instantiate(flyingSkullPrefab, skullspawnPoint.transform.position, Quaternion.identity);

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
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Collided with player");
            collision.gameObject.SendMessage("GetHit", -10f);

            

        }

        
       
    }
}
