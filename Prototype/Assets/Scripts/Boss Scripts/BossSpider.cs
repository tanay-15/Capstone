using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpider : MonoBehaviour {

    // Use this for initialization
    public GameObject player;
    private Animator anim;

    public GameObject position1;
    public GameObject position2;
    public GameObject position3;
    public GameObject position4;
    public GameObject position5;

    private Vector3 positionA;
    private Vector3 positionB;

    public enum States
    {
        Idle,
        GoToStart,
        DecidingAttack,
        Attack1,
        Attack2,
        Attack3,
        Dead

    }

    public States currentState;
    public float moveSpeed;
    public float attackdoneCounter;
    public bool AttackDone;
    
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        anim = this.GetComponent<Animator>();
        positionA = position1.transform.position;
        positionB = position2.transform.position;
        this.transform.position = positionA;
	}
	
	// Update is called once per frame
	void Update () {

        Attack1();
        IsAttackDone();
        GoBackToPosition();
	}


    void IsAttackDone()
    {
        if (AttackDone)
        {
            attackdoneCounter = attackdoneCounter - Time.deltaTime;
            if(attackdoneCounter <= 0)
            {
                AttackDone = false;
                currentState = States.GoToStart;
            }
        }
    }

    void GoBackToPosition()
    {
        if(currentState == States.GoToStart)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, positionA, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(this.transform.position, positionA) < 0.3f)
            {
                currentState = States.Idle;
                attackdoneCounter = 4f;
            }
        }
    }

    void Attack1()
    {   
        if(currentState == States.Attack1)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, positionB, moveSpeed * Time.deltaTime);

            if(Vector2.Distance(this.transform.position,positionB)< 0.3f)
            {
                AttackDone = true;
            }
        }
        
    }

    
}
