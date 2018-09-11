using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour {

    // Use this for initialization

    [Header("Player Details")]
    public GameObject player;

    public enum State
    {
        Idle,
        Walk,
        ReadyToAttack,
        GetHit,
        Dead,
        Attack1,
        Attack2,
        Reanimate
    }
    [Header("Boss Details")]
    public State currentState;
    public bool hasBarrier = true;
    public int barrCounter = 3;
    public float barrierback = 10;
    private int tempvalue;
    public bool deciderparameter = false;

    [Header("Attack Shield Up")]
    //Attack 1 is AOE slash
    public Collider aoecollider;
    //Attack 2 is Dash

    [Header("Attack Shield Down")]
    public GameObject orbprefab;
    public GameObject orbloc1;
    public GameObject orbloc2;
    public GameObject orbloc3;
    public GameObject orbloc4;
    public bool shouldattack2;
    public float attack2counter = 0;

    [Header("Boss movement")]
    public GameObject waypoint1;
    public GameObject waypoint2;
    private bool movetoA = true;
    private bool movetoB;
    public bool nearby;

    private Quaternion o3rot;
    

	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");

        aoecollider = this.transform.GetChild(4).GetComponent<Collider>();
      
	}
	
	// Update is called once per frame
	void Update () {

        BarrierLogic();

        if (shouldattack2)
        {

            //shield is down, shadow orb attack
            attack2counter = attack2counter - Time.deltaTime;
            if(attack2counter <= 0)
            {
                ShieldDownAttack();
                attack2counter = 4;
            }
          
           
        }

        if (hasBarrier)
        {
            PlayerNearby();
            //shield is up, dark axe attack
            if (!nearby)
            {
                Movement();
            }

            else
            {
                Debug.Log("Player is nearby! check for his moment!");

                Decider();

            }
        }
	}

    private void Decider()
    {
        if (!deciderparameter)
        {
            tempvalue = (int)Random.Range(0, 8);
            
        }
            

            if (tempvalue <= 5)
            {
                Debug.Log("Dash power");
                Dash();
                //deciderparameter = true;
            }

            if (tempvalue > 5)
            {
               
                Debug.Log("Aoe slash attack");
                //deciderparameter = true;
            }
        
          

        
       
    }

    public void PlayerNearby()
    {
        if(Vector2.Distance(player.transform.position,this.transform.position) < 7f)
        {
            nearby = true;
          
        }

        else
        {
            //flip to player's side.
            //take player position.
            //attack ready
            //dash
            //or aoe slash on both sides
            nearby = false;
        }
    }
    public void Dash()
    {
        if(this.transform.position.x < player.transform.position.x)
        {
            Debug.Log("Dash right");

            this.transform.position = Vector3.MoveTowards(this.transform.position, waypoint2.transform.position, 7 * Time.deltaTime);
           
        }

        if(this.transform.position.x > player.transform.position.x)
        {
            Debug.Log("Dash left");
            //dash to wp1
            this.transform.position = Vector3.MoveTowards(this.transform.position, waypoint1.transform.position, 7 * Time.deltaTime);
          
        }
    }

    public void AOESlashing()
    {
       
    }

    public void Movement()
    {
       

        if (movetoA)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, waypoint1.transform.position, 1f * Time.deltaTime);

            if (Vector3.Distance(waypoint1.transform.position, this.transform.position) <= 2f)
            {
                movetoA = false;
                movetoB = true;
            }
        }

        if (movetoB)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, waypoint2.transform.position, 1f * Time.deltaTime);

            if (Vector3.Distance(waypoint2.transform.position, this.transform.position) <= 2f)
            {
                movetoA = true;
                movetoB = false;
            }
        }
    }
        
    


    public void ShieldDownAttack()
    {
        //attack when shield is down

        Instantiate(orbprefab, orbloc1.transform.position, orbloc1.transform.rotation);
        Instantiate(orbprefab, orbloc2.transform.position, orbloc2.transform.rotation);
        Instantiate(orbprefab, orbloc3.transform.position,orbloc3.transform.rotation);
        Instantiate(orbprefab, orbloc4.transform.position, orbloc4.transform.rotation);
        
    }



    public void BarrierLogic()
    {

        //logic for barrier
        if (!hasBarrier)
        {
            shouldattack2 = true;
            barrierback = barrierback - Time.deltaTime;
          

            if (barrierback <= 0)
            {
                barrCounter = 2;
                hasBarrier = true;
                barrierback = 10;
                Debug.Log("Barrier is back up!");
                shouldattack2 = false;
            }
        }
    }

    public void applyDamage(int damage)
    {
        Debug.Log("Boss has been attacked!");
        if (barrCounter >= 0)
        {
           
            Debug.Log("Barrier strength is : " + barrCounter);
            barrCounter--;
        }

        if(barrCounter < 0)
        {
            Debug.Log("Barrier down!");
            hasBarrier = false;
        }
       
    }
}
