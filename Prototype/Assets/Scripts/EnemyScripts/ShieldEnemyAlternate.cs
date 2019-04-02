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
       anim.Play("Run");
        var dir = target.transform.position - this.transform.position;
        var dis = dir.magnitude;
        finalDirection = dir / dis;
        yield return new WaitForSeconds(0.25f);
        transform.GetComponent<Rigidbody2D>().velocity = finalDirection.normalized * speed;
        //Debug.Log("Dir " + finalDirection);
        //transform.GetComponent<Rigidbody2D>().AddRelativeForce(finalDirection * speed);//, ForceMode2D.Force);
        //transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        

    }
    // Update is called once per frame
    void Update()
    {
        switch (currentstate)
        {
            case States.Idle:
                {
                    anim.Play("Idle");
                    //hit = Physics2D.OverlapCircle(this.transform.position, 8f, PlayerMask);

                    if (target && withinRange)
                    {
                        currentstate = States.Attack;
                    }

                    break;
                }
            case States.Attack:
                {
                    elapsedTime += Time.deltaTime;
                    if (elapsedTime >12.0f && target)
                    {
                        
                        StartCoroutine("Charge");
                        if ((target.transform.position - transform.position).magnitude < 2f)
                        {
                            Debug.Log("velocity " + transform.GetComponent<Rigidbody2D>().velocity);
                            currentstate = States.KnockBack;
                        }
                        elapsedTime = 0f;
                    } 
                    else if (!withinRange)
                    {
                        currentstate = States.Idle;
                    }
                    break;
                }
            case States.KnockBack:
                {
                    transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                    Debug.Log("velocity " + transform.GetComponent<Rigidbody2D>().velocity);
                    Debug.Log(" Its here");
                    target.GetComponent<Rigidbody2D>().AddForce(finalDirection * 2f, ForceMode2D.Impulse);
                    //currentstate = States.Idle;
                    break;
                }
                
        }
    }

  public void handleTriggerEvent(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            currentstate = States.KnockBack;
            
        }
    }
}
