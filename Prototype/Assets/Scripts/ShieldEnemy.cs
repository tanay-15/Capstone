using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        rigi = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAlive)
        {
            DetectingPlayer();
            Patrol();
            Pursuit();
            Attack();
        }
    }


    public override void Patrol()
    {
        anim.SetBool("Walking", true);
        anim.SetBool("Attack", false);
        if (IsAlive)
        {
            if (target == null)
            {
                // anim.SetTrigger("StartPatrol");
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
}
