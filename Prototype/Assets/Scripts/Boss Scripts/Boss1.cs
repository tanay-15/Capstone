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

    [Header("Attack Shield Down")]
    public GameObject orbprefab;
    public GameObject orbloc1;
    public GameObject orbloc2;
    public GameObject orbloc3;
    public GameObject orbloc4;
    public bool shouldattack2;
    public float attack2counter = 0;

    private Quaternion o3rot;
    

	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
       // o3rot = Quaternion.Euler(0, 0, -23.5);
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
                attack2counter = 10;
            }
          
           
        }

        if (hasBarrier)
        {
            //shield is up, dark axe attack

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
