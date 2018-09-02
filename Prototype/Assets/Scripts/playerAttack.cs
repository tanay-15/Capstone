using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour {
    private bool attacking = false;
    //private float attackTimer = 0f;
    //private float attackCd = 0.3f;
    private Animator myAnim;
    public Collider attackTrigger;
	// Use this for initialization
	void Start () {
        myAnim = GetComponent<Animator>();
        attackTrigger.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButton(0))
        {
            attacking = true;
            attackTrigger.enabled = true;
        }
        else
        {
            attackTrigger.enabled = false;
        }
		
        
	}
}
