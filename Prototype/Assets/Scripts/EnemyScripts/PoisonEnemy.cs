using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEnemy : Enemy
{
    public GameObject PoisonProjectile;
    public GameObject PoisonEffect1;
    public GameObject PoisonEffect2;

    private float attackResetTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentstate = States.Idle;
        anim = GetComponent<Animator>();

        foreach (Transform child in transform)
        {

            if (child.name == "GroundTrigger")
            {
                groundTrigger = child.gameObject;
            }

        }

        this.maxHealth = this.health;

        //Physics2D.IgnoreLayerCollision(11,15);
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentstate)
        {
            case States.Idle:
            {
                    GetComponent<Animator>().Play("Idle");
                    GetComponent<Animator>().speed = 1.0f;

                    if (target && withinRange)
                    {
                        currentstate = States.Attack;
                    }

                    break;
            }
            case States.KnockBack:
            {
                    GetComponent<Animator>().Play("Knockback");
                    //transform.rotation = Quaternion.Euler(0,0,Vector2.Angle(Vector2.right, GetComponent<Rigidbody2D>().velocity.normalized));
                    transform.eulerAngles = -new Vector3(0, 0, Vector2.SignedAngle(GetComponent<Rigidbody2D>().velocity, new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0)));
                    GetComponent<BoxCollider2D>().enabled = false;


                    if (GetComponent<Rigidbody2D>().velocity.y == 0)
                    {
                        currentstate = States.Idle;

                        //GetComponent<Animator>().Play("Idle");
                        GetComponent<BoxCollider2D>().enabled = true;
                    }

                    break;
            }
            case States.Attack:
            {
                    attackResetTimer += Time.deltaTime;
                    GetComponent<Animator>().Play("Attack");

                    if (attackResetTimer >= 2.0f && target)
                    {
                        attackResetTimer = 0.0f;

                        var projectile = Instantiate(PoisonProjectile,transform.position + new Vector3(0,1,0), Quaternion.identity);

                        //float time = 2.5f;
                        float distanceX = target.transform.position.x - transform.position.x;
                        float distanceY = target.transform.position.y - transform.position.y - 1.0f;

                        float velocityX = distanceX ;
                        float velocityY = (distanceY ) - 0.5f  * (Physics2D.gravity.y);

                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityX,velocityY);
                        projectile.GetComponent<PoisonProjectile>().targetPos = new Vector2(target.transform.position.x, target.transform.position.y);
                        Physics2D.IgnoreCollision(projectile.GetComponent<CircleCollider2D>(), target.GetComponent<CapsuleCollider2D>());
                    }

                    if (!withinRange)
                    {
                        currentstate = States.Idle;
                    }

                    CheckForPlayerPosition();

                    break;
            }
        }

        if (health <= 0)
        {
            Death();
        }
    }

    public override void Death()
    {
        currentstate = States.Dead;
        GetComponent<SkeletonDeathScript>().Die();
        events.OnDeath.Invoke();

        //PoisonEffect1.GetComponent<ParticleSystem>().= false;
    }

    public void CheckForPlayerPosition()
    {
        if (target.transform.position.x > this.transform.position.x)
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

        if (target.transform.position.x < this.transform.position.x)
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
}
