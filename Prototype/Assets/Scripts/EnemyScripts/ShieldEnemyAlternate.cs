using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyAlternate : Enemy
{
    public float speed;
    Vector2 finalDirection;
    float elapsedTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentstate = States.Idle;
        anim = GetComponent<Animator>();
    }
    IEnumerator Charge()
    {
        //play running animation
        anim.Play("AltShield_AttackReady");
        var dir = target.transform.position - this.transform.position;
        var dis = dir.magnitude;
        finalDirection = dir / dis;
        yield return new WaitForSeconds(2f);       
        anim.Play("AltShield_Attack");
        //transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 0.25f);
        //transform.GetComponent<Rigidbody2D>().velocity =  new Vector2(finalDirection.x, 0).normalized * speed;
        //transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(finalDirection.x, 0).normalized * speed, ForceMode2D.Impulse);
        Debug.Log("velocity : " + transform.GetComponent<Rigidbody2D>().velocity);
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentstate)
        {
            case States.Idle:
                {
                    anim.Play("AltShieldIdle");
                    elapsedTime += Time.deltaTime;
                    if (elapsedTime > 3.0f && target && withinRange)
                    {
                        elapsedTime = 0f;

                        StartCoroutine("Charge");
                        currentstate = States.Attack;
                    }
                    break;
                }
            case States.Attack:
                {



                    transform.GetComponent<Rigidbody2D>().velocity = new Vector2(finalDirection.x, 0).normalized * speed;
                    //if ((target.transform.position - transform.position).magnitude < 0.5f)
                    //{

                    //    currentstate = States.KnockBack;
                    //}


                    if (!withinRange)
                    {
                        currentstate = States.Idle;
                    }
                    


                    break;
                }
            case States.KnockBack:
                {
                    
                    transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                    Debug.Log("velocity " + transform.GetComponent<Rigidbody2D>().velocity);
                    target.GetComponent<Rigidbody2D>().AddForce(finalDirection * 2f, ForceMode2D.Impulse);
                    
                    currentstate = States.Idle;


                   

                    break;
                }
                
        }
    }

  public void handleTriggerEvent(Collision2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            currentstate = States.KnockBack;            
        }
    }
}
