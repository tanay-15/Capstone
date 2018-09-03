using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedShuri : MonoBehaviour {

    // Use this for initialization
    public Vector2 targetposition;
    public GameObject goparent;

    private Vector2 movepos;

   
	void Start () {

        targetposition = this.transform.GetComponentInParent<RangeEnemy>().GetTarget().transform.position;
        
	}
	
	// Update is called once per frame
	void Update () {

        if(targetposition.x < this.transform.position.x)
        {
            movepos = new Vector2(-3f * Time.deltaTime, 0);
        }
       
        else if(targetposition.x > this.transform.position.x)
        {
            movepos = new Vector2(3f * Time.deltaTime, 0);
        }
      this.transform.Translate(movepos);
       
	}

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
