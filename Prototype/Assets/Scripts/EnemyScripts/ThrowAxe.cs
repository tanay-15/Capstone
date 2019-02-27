using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAxe : MonoBehaviour {

    // Use this for initialization
    //Move in -x

    private Vector2 moveVector;
    private Rigidbody2D rigi;

    private GameObject player_target;
    private Vector3 startpoint;
    private float TimeToHit = 2f;
    
	void Start () {

      
        rigi = this.GetComponent<Rigidbody2D>();
    
        startpoint = this.transform.position;
         moveVector = 4*Vector3.Normalize(GameObject.Find("Player").transform.position- transform.position);
       

    }

    
	
	// Update is called once per frame
	void Update () {

      
        rigi.velocity = moveVector;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("GetHit", -15);
            Destroy(this.gameObject);
        }
        if(collision.gameObject.tag == "projectile")
        {
            Destroy(this.gameObject);
        }
        else
        {
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
        
    }
}
