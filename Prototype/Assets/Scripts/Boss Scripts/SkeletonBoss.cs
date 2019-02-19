using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoss : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject leftpoint;
    public GameObject rightpoint;
    public GameObject startpoint;
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

    public GameObject skeletonPrefab;

    public GameObject flyingSkullPrefab;

    private float flyspawnCounter = 0f;
    private float skelspawnCounter = 0f;

    
    void Start()
    {
        this.transform.position = startpoint.transform.position;

        leftpointpos = leftpoint.transform.position;
        rightpointpos = rightpoint.transform.position;

        spawnPointLeftPos = spawnPointLeft.transform.position;
        spawnPointRightPos = spawnPointRight.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Death();
        Attack1();
        Attack2();
      

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
            collision.gameObject.SendMessage("applyDamage", 10f);

        }

       
    }
}
