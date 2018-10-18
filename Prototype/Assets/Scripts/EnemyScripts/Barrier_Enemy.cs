using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier_Enemy : MonoBehaviour {

    // Use this for initialization

    public float Range;
    public float moveTospeed;
    public GameObject targetPlayer;

    public bool playerdetected;

    public float decisionParameter = 0;
    public bool doAttack;
    
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        MoveToPlayer();
        Attack();

	}

    void Attack()
    {
        if (doAttack)
        {
            Debug.Log("Enemy attacks");
            targetPlayer.SendMessageUpwards("applyDamage", 5);
            doAttack = false;
        }
    }

    void MoveToPlayer()
    {
        if (targetPlayer && decisionParameter == 0)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetPlayer.transform.position, moveTospeed * Time.deltaTime);

            if(Vector2.Distance(this.transform.position,targetPlayer.transform.position) < 1.3f)
            {
                decisionParameter = 3f;
                doAttack = true;
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
}
