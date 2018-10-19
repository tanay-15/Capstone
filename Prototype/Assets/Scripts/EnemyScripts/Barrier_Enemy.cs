using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier_Enemy : MonoBehaviour {

    // Use this for initialization

    public float Range;
    public float moveTospeed;
    public GameObject targetPlayer;

    public bool facingLeft;

    public bool playerdetected;

    public float decisionParameter = 0;
    public bool doAttack;


    
	void Start () {

        facingLeft = true;
	}
	
	// Update is called once per frame
	void Update () {

        MoveToPlayer();
        Attack();

        if(doAttack == false && decisionParameter > 0) 
        {
            decisionParameter = decisionParameter - Time.deltaTime;
        }

	}

    void Attack()
    {
        if (doAttack)
        {
            Debug.Log("Enemy attacks");
          // attack player
            doAttack = false;
        }
    }

    void MoveToPlayer()
    {
        if (targetPlayer && decisionParameter <= 0)
        {
            CheckForFlip();
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetPlayer.transform.position, moveTospeed * Time.deltaTime);

            if(Vector2.Distance(this.transform.position,targetPlayer.transform.position) < 1.8f)
            {
                decisionParameter = 3f;
                doAttack = true;
            }
        }
    }

    public void CheckForFlip()
    {
        if(this.transform.position.x > targetPlayer.transform.position.x)
        {
            // face left
            if (facingLeft)
            {
                //ignore
            }
            if (!facingLeft)
            {
                flip();
            }
        }

        if(this.transform.position.x < targetPlayer.transform.position.x)
        {
            //face right
            if (!facingLeft)
            {
                //ignore
            }
            if (facingLeft)
            {
                flip();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerdetected = true;
            targetPlayer = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerdetected = false;
            targetPlayer = collider.gameObject;
        }
    }

    public void flip()
    {
        facingLeft = !facingLeft;
        this.transform.localScale = new Vector2(this.transform.localScale.x * -1, this.transform.localScale.y);
    }
}
