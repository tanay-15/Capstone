﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy {


    [Header("Range details")]
    public GameObject shurikenprefab;
    public GameObject shuriloct;
    private GameObject go;
    private bool Thrown = false;


	// Use this for initialization
	void Start () {

      
        anim = this.GetComponent<Animator>();
        rigi = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        if (IsAlive)
        {
            if(target!= null)
            {
                rateofattack = rateofattack - Time.deltaTime;
                CheckForPlayerPosition();
                   if(rateofattack <= 1f)
                {
                    Thrown = false;
                }
                if (!Thrown && rateofattack <=0)
                {
                    
                    Debug.Log("Found Target now attack him");
                    ThrowAxe();
                }
             
            }
        }
    }

    public void CheckForPlayerPosition()
    {
        if(target.transform.position.x < this.transform.position.x)
        {
            //check where it is looking
            //look left
            if (LookingLeft)
            {
                //ignore
            }
            if (!LookingLeft)
            {
                flip();
            }

        }

        if(target.transform.position.x > this.transform.position.x)
        {
            //check where it is looking
            //Look right

            if (LookingLeft)
            {
                flip();
            }

            if (!LookingLeft)
            {
                //ignore
            }
        }
    }

   public void ThrowAxe()
    {
        Instantiate(shurikenprefab, shuriloct.transform.position, shurikenprefab.transform.rotation);
        rateofattack = 1.3f;
        Thrown = true;
    }


  
    public override void Pursuit()
    {

        if (target && !AttackReady)
        {
            currentstate = States.Pursuit;
            anim.SetBool("ShouldPursuit", true);
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetpos, movspeed * Time.deltaTime);


            if(Vector2.Distance(this.transform.position,target.transform.position) <= 3f)
            {
                targetpos = target.transform.position;
                AttackReady = true;
            }
        }

    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "projectile" || collision.gameObject.tag == "Grabbable") && Vector3.Distance(collision.transform.position, transform.position) < 2f)
        {
            IsAlive = false;

            health = 0;

        }

        if (collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
            targetpos = collision.gameObject.transform.position;
            anim.SetBool("Attack", true);
            currentstate = States.Attack;
            AttackReady = true;
            Debug.Log("Enemy attack ready");
        }
    }

    public override void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            target = null;
            anim.SetBool("Attack", false);
            AttackReady = false;
            currentstate = States.Idle;
        }
    }

    public Vector2 GetTargetpos()
    {
        return targetpos;
    }

    public GameObject GetTarget()
    {
        return target;
    }
}
