using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrimReaper : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject startpos;
    public GameObject midpos;
    public GameObject endpos;

    public float movspeed;

    public Vector3 pointA;
    public Vector3 pointB;
    public Vector3 pointC;

    public bool reachedFinal;
    public bool getToB;
    public bool getToC;

    private Animator anim;
    public bool shouldmove = true;

    private bool HasAttacked = false;

    public BoxCollider2D attackCollider;

  
    void Start()
    {
        anim = this.GetComponent<Animator>();
        pointA = startpos.transform.position;
        pointB = midpos.transform.position;
        pointC = endpos.transform.position;
        attackCollider = this.GetComponent<BoxCollider2D>();
        attackCollider.enabled = false;
        getToB = true;
        getToC = false;
        reachedFinal = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldmove)
        {
            movement();
        }

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            attackCollider.enabled = false;
        }

        if (reachedFinal)
        {
            Destroy(this.gameObject);
        }
    }

    void movement()
    {
        
      
        if (getToB)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, pointB, movspeed * Time.deltaTime);

            if(Vector2.Distance(this.transform.position,pointB) <= 1.3f)
            {
                flip();
                getToB = false;
                getToC = true;
            }
        }

        if (getToC)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, pointC, movspeed * Time.deltaTime);

            if (Vector2.Distance(this.transform.position, pointC) <= 1f)
            {
                Debug.Log("Reached final position");
                getToC = false;
                reachedFinal = true;
            }
        }

        
    }


    public void flip()
    {

      

        this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);




    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            
            Debug.Log("Grim Reaper is attacking player");

            if (!HasAttacked)
            {
                shouldmove = false;
                anim.SetTrigger("Attack");
            }
           
            
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Grim Reaper reaps player's soul");
        }
    }

    public void AttackDone()
    {
        HasAttacked = true;
        shouldmove = true;
        attackCollider.enabled = true;
        
    }

    public void SwitchColliderOff()
    {
        attackCollider.enabled = false;
    }
}
