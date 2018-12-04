using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitorObjectScript : MonoBehaviour {

    public bool isOrbiting = false;
    public bool hit = false;
    public GameObject impact;

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


        if (collision.gameObject.layer == 15 || collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SendMessage("applyDamage", 15);
            Instantiate(impact, collision.GetContact(0).point, Quaternion.identity);
            hit = true;
        }

        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10 || collision.gameObject.tag == "Wall")
        {
            hit = true;
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //hit = false;
    }
}
