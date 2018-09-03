using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour {
    private bool attacking = false;
    //private float attackTimer = 0f;
    //private float attackCd = 0.3f;
    //private Animator myAnim;
    public Collider attackTrigger;
    private Vector3 currentAngle;
    private Vector3 reset;
    private Vector3 oldPos;
   

    // Use this for initialization
    void Start () {
      //  myAnim = GetComponent<Animator>();
        reset = attackTrigger.transform.localEulerAngles;
        oldPos = attackTrigger.transform.position;
        attackTrigger.enabled = false;
        Debug.Log("oldPos" + oldPos);

    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0))
        {
            currentAngle.z = reset.z;
            attacking = true;
            attackTrigger.enabled = true;
            //attackTrigger.transform.localEulerAngles = new Vector3(attackTrigger.transform.localEulerAngles.x, attackTrigger.transform.localEulerAngles.y, currentAngle.z);
            //attackTrigger.transform.localEulerAngles = new Vector3(0, 90, 0);
            //attackTrigger.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.right);
            //attackTrigger.transform.localPosition = Vector3.Lerp(attackTrigger.transform.localPosition, new Vector3(1, 0, 0), 0.01f);
            //attackTrigger.transform.RotateAround(Vector3.zero, new Vector3(0,0,1), 45 * Time.deltaTime);
            //attackTrigger.transform.localPosition = Vector3.Slerp(attackTrigger.transform.localPosition, new Vector3(1, 0, 0), 0.01f);
            //attackTrigger.transform.localPosition = Vector3.Slerp(attackTrigger.transform.localPosition, new Vector3(1, 0, 0), 0.01f);
        }
        else 
        {
            
            //attackTrigger.transform.rotation = Quaternion.FromToRotation(Vector3.right, Vector3.up);
            attackTrigger.transform.position = oldPos;
           
            Debug.Log("trigger pos" + attackTrigger.transform.position);
            attackTrigger.enabled = false;
            
        }
      
	}
   
}
