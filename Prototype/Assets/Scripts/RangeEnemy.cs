﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy {


    [Header("Range details")]
    public GameObject shurikenprefab;
    public GameObject shuriloct;
    private GameObject go;
    //private bool IsPunched;
    private bool Thrown = false;

    

    public override Vector3 lifebarOffset
    {
        get
        {
            return Vector3.up * 2f;
        }
    }

	// Use this for initialization
	void Start () {

        maxHealth = health;
        anim = this.GetComponent<Animator>();
        rigi = this.GetComponent<Rigidbody>();

        Physics2D.IgnoreLayerCollision(16,10);
        Physics2D.IgnoreLayerCollision(16,15);
        Physics2D.IgnoreLayerCollision(16,16);
    }
	
	// Update is called once per frame
	void Update () {

        if (IsAlive)
        {
            if (target != null)
            {
                
                CheckForPlayerPosition();

                if (!Thrown && rateofattack <= 0.2f)
                {


                    ThrowAxe();
                }
                if (rateofattack <= 0)
                {
                    rateofattack = 2f;
                    Thrown = false;
                    anim.Play("SkelAttack");
                }

                rateofattack = rateofattack - Time.deltaTime;
            }
        }
        if(health <=0)
        {
            Death();
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
        //rateofattack = 2f;
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

   public  void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "projectile" || collision.gameObject.tag == "Grabbable") && Vector3.Distance(collision.transform.position, transform.position) < 2f)
        {

            if(collision.GetType() == typeof(BoxCollider2D))
            {
                applyDamage(5);
                if (collision.GetComponent<OrbitorObjectScript>())
                    collision.GetComponent<OrbitorObjectScript>().hit = true;
                else if (collision.GetComponent<playerKnife2D>())
                    collision.GetComponent<playerKnife2D>().hasHit = true;

                if (health <= 0)
                    IsAlive = false;

                var impact = Instantiate(ImpactAnim, collision.transform.position, Quaternion.identity);
                impact.gameObject.SetActive(true);
            }


        }

        if (collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
            targetpos = collision.gameObject.transform.position;
            GetComponent<Animator>().SetBool("Attack", true);
            currentstate = States.Attack;
            AttackReady = true;
        }
    }

    public override void TriggerEnter2D(Collider2D collision)
    {
        if ((/*collision.gameObject.tag == "projectile" || */collision.gameObject.tag == "Grabbable") && Vector3.Distance(collision.transform.position, transform.position) < 1.5f)
        {
            if (collision.GetType() == typeof(BoxCollider2D))
            {

                applyDamage(Random.Range(5, 8));
                if (collision.GetComponent<OrbitorObjectScript>())
                    collision.GetComponent<OrbitorObjectScript>().hit = true;
                else if (collision.GetComponent<playerKnife2D>())
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

        if (((collision.gameObject.name == "AttackTrigger") || (collision.gameObject.name == "StompTrigger")) && !IsPunched && Vector3.Distance(collision.transform.position, transform.position) < 1.5f)
        {
            if ((SkillTree.info.nodesActivated & SkillNodes.D_2) == SkillNodes.D_2 && target.GetComponent<DemonTransformScript>().DemonModeActive)
                applyDamage(10);
            else
                applyDamage(3);

            IsPunched = true;
            StartCoroutine(IsPunchedReset());

            var impact = Instantiate(ImpactAnim, new Vector2(transform.position.x, transform.position.y + 0.8f), Quaternion.identity);
            impact.gameObject.SetActive(true);

            if ((SkillTree.info.nodesActivated & SkillNodes.D_4) == SkillNodes.D_4)
                collision.transform.parent.parent.gameObject.SendMessage("GetHit", 3);
        }

        if (collision.gameObject.tag == "Player")
        {

            target = collision.gameObject;
            targetpos = collision.gameObject.transform.position;
            GetComponent<Animator>().SetBool("Attack", true);
            currentstate = States.Attack;
            AttackReady = true;
            GetComponent<Animator>().speed = 1.0f;
            rateofattack = 2.0f;
        }
    }


    public override void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            target = null;
            GetComponent<Animator>().SetBool("Attack", false);
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

    IEnumerator IsPunchedReset()
    {
        yield return new WaitForSeconds(0.25f);
        IsPunched = false;
    }

}
