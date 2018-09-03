using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy {


    [Header("Range details")]
    public GameObject shurikenprefab;
    public GameObject shuriloct;
    private GameObject go;


	// Use this for initialization
	void Start () {

        anim = this.GetComponent<Animator>();
        rigi = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        if (IsAlive)
        {
            DetectingPlayer();
            Patrol();
            Pursuit();
            Attack();
        }

        Death();
    }

    public override void Attack()
    {

        if (AttackReady)
        {
            rateofattack -= Time.deltaTime;

            if(rateofattack <= 0f)
            {
                //Throw shruriken
                currentstate = States.Attack;
                anim.SetBool("ShouldPursuit", false);
                anim.SetTrigger("Attack");
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    go = Instantiate(shurikenprefab, shuriloct.transform.position, shurikenprefab.transform.rotation);
                    go.transform.SetParent(this.gameObject.transform);
                    rateofattack = 3f;
                }
              
            }
        }

        /*
         *   if (AttackReady)
        {
            rateofattack -= Time.deltaTime;
            if(rateofattack <= 0)
            {
              
               
                
                Debug.Log("Enemy now attacks the player");
                rateofattack = 2f;
            }
           
        }
         */
        
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

    public Vector2 GetTargetpos()
    {
        return targetpos;
    }

    public GameObject GetTarget()
    {
        return target;
    }
}
