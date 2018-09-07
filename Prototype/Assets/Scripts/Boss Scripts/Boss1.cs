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

	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void applyDamage(int damage)
    {
        Debug.Log("Boss has been attacked!");
    }
}
