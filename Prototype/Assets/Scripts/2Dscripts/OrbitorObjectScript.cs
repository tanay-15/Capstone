using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitorObjectScript : MonoBehaviour {

    public bool isOrbiting = false;
    public bool hit = true;

	// Use this for initialization
	void Start () {


        Physics2D.IgnoreLayerCollision(11, 12);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
            
        GetComponent<Rigidbody2D>().gravityScale = 1;
        hit = true;

        if(collision.gameObject.tag == "Enemy")
            collision.gameObject.SendMessage("applyDamage", 15);

    }
}
