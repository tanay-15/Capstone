using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Enemy
{
    // Start is called before the first frame update

    public bool isBlocking = false;
    void Start()
    {
        anim = this.GetComponent<Animator>();
        rigi = this.GetComponent<Rigidbody>();
        SetupValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0.0f)
        {
            IsAlive = false;
        }

        if (IsAlive)
           {
            //CheckForGroundAhead();
            IdleGame();
               DetectingPlayer();
               Patrol();
               Pursuit();
               //DirectAttack();
               Attack();
            Blocking();


           }

        else
        {
            Death();
        }
    }

    public override void DetectingPlayer()
    {
       
        
               if (LookingLeft)
               {

                   if (vishit = Physics2D.Raycast(vision.transform.position, -transform.right, 4f, EnemyIgnoreMask))
                   {

                       if (vishit.collider.gameObject.tag == "Player")
                       {
                           // anim.SetBool("Alert", true);
                           target = vishit.collider.gameObject;
                           targetpos = target.transform.position;
                           currentstate = States.Pursuit;
                       }
                   }

            else
            {
                target = null;
                currentstate = States.Patrol;
            }

               }

             else  if (!LookingLeft)
               {



                   if (vishit = Physics2D.Raycast(vision.transform.position, transform.right, 4f, EnemyIgnoreMask))
                   {

                       if (vishit.collider.gameObject.tag == "Player")
                       {
                           // anim.SetBool("Alert", true);
                           target = vishit.collider.gameObject;
                           targetpos = target.transform.position;
                           currentstate = States.Pursuit;
                       }
                   }

            else
            {
                target = null;
                currentstate = States.Patrol;
            }

        }


        else 
        {
            target = null;
            currentstate = States.Patrol;

        }

    }

    public void IdleGame()
    {
        if(currentstate == States.Idle)
        {
            isBlocking = false;
            anim.SetBool("Blocking", false);
            anim.SetBool("Walking", false);
            anim.SetBool("Attack", false);

        }
    }
    public override void Patrol()
    {
        isBlocking = false;
        anim.SetBool("Walking", true);
        anim.SetBool("Attack", false);
        anim.SetBool("Blocking", false);
        if (IsAlive)
        {
            if (target == null)
            {
                
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

    public override void applyDamage(int amount)
    {

        if (isBlocking)
        {

        }

        else if (!isBlocking)
        {
            this.health = this.health - amount;

            events.OnTakeDamage.Invoke((float)this.health / (float)maxHealth);
        }
    


    }
    public override void Pursuit()
    {
        isBlocking = false;
        anim.SetBool("Walking", true);
        anim.SetBool("Attack", false);
        anim.SetBool("Blocking", false);
        if (target && !AttackReady)
        {
            currentstate = States.Pursuit;
            CheckForFlip(target.transform.position);
            if (Mathf.Abs(this.transform.position.y - target.transform.position.y) > 3f)
            {
                currentstate = States.Patrol;

            }
            if(Vector2.Distance(this.transform.position,target.transform.position)< 1f)
            {
                currentstate = States.Attack;
                anim.SetBool("Attack", true);
            }
            else
            {

                this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, movspeed * Time.deltaTime);
            }
        }
    }

    public void Blocking()
    {
            if(target.GetComponent<PlayerStates>().status== PlayerStates.State.Melee)
        {
            isBlocking = true;
            Debug.Log("Perform block");
            anim.SetBool("Blocking", true);
            anim.SetBool("Attack", false);
        }
    }
}
