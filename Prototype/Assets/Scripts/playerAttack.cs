using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour {
    private bool attacking = false;
    //private float attackTimer = 0f;
    //private float attackCd = 0.3f;
    private Animator myAnim;
    public Collider attackTrigger;
    private Vector3 currentAngle;
    private Vector3 reset;
	// Use this for initialization
	void Start () {
        myAnim = GetComponent<Animator>();
        reset = attackTrigger.transform.localEulerAngles;
        attackTrigger.enabled = false;
        
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButton(0))
        {
            currentAngle.z = reset.z;
            attacking = true;
            attackTrigger.enabled = true;
            attackTrigger.transform.localEulerAngles = new Vector3(attackTrigger.transform.localEulerAngles.x, attackTrigger.transform.localEulerAngles.y, currentAngle.z);
            //attackTrigger.transform.RotateAround(Vector3.zero, new Vector3(0,0,1), 45 * Time.deltaTime);
            //attackTrigger.transform.localPosition = Vector3.Slerp(attackTrigger.transform.localPosition, new Vector3(1, 0, 0), 0.01f);
            //attackTrigger.transform.localPosition = Vector3.Slerp(attackTrigger.transform.localPosition, new Vector3(1, 0, 0), 0.01f);
        }
        else
        {
            attackTrigger.enabled = false;
        }
		
        
	}
}
