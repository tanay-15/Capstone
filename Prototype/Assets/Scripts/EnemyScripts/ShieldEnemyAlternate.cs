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
        LookingLeft = true;
        currentstate = States.Idle;
        anim = GetComponent<Animator>();
    }
    IEnumerator Charge()
    {
        //play running animation
        Debug.Log("before playing animation");
        anim.Play("AltShield_AttackReady");
        var dir = target.transform.position - this.transform.position;
        var dis = dir.magnitude;
        finalDirection = dir / dis;
        yield return new WaitForSeconds(2f);
        //transform.GetComponent<Rigidbody2D>().velocity = new Vector2(finalDirection.x, 0).normalized * speed;
        //transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(finalDirection.x, 0).normalized * speed, ForceMode2D.Impulse);
        Debug.Log("velocity : " + transform.GetComponent<Rigidbody2D>().velocity);
        anim.Play("AltShield_Attack");

        //transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 0.25f);
        //transform.GetComponent<Rigidbody2D>().velocity =  new Vector2(finalDirection.x, 0).normalized * speed;
        //transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(finalDirection.x, 0).normalized * speed, ForceMode2D.Impulse);
       
    }
    // Update is called once per frame
    void Update()
    {
        if(finalDirection.x > 0 && LookingLeft)
        {
            flip();

        }
        else if(finalDirection.x < 0 && !LookingLeft)
        {
            flip();
        }
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
                    Debug.Log("velocity " + transform.GetComponent<Rigidbody2D>().velocity);
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
