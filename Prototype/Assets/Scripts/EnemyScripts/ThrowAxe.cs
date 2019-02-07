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

        //moveVector = new Vector3(-8f * Time.deltaTime, 0, 0);
        rigi = this.GetComponent<Rigidbody2D>();
        player_target = GameObject.Find("Character");
        startpoint = this.transform.position;
         moveVector = 4*Vector3.Normalize(GameObject.Find("Player").transform.position- transform.position);
        //Physics2D.gravity = new Vector2(0, -9.8f);
     

    }

    void AxeThrow()
    {
       
        Vector3 distance = startpoint - player_target.transform.position;
        distance.z = 0f;
        Vector2 distanceCalc = distance;
        distanceCalc.y = 0f;

        float Sy = distance.y;
        float Sx = distanceCalc.magnitude;

        float Vx = Sx / TimeToHit;
        float Vy = Sy / TimeToHit + 0.5f * Mathf.Abs(Physics.gravity.y) * TimeToHit;

        Vector2 result = distanceCalc.normalized;

        result = result * Vx;
        result.y = Vy;

        moveVector = result;
        
    }
	
	// Update is called once per frame
	void Update () {

        //this.transform.Translate(moveVector);

        //transform.rotation = Quaternion.LookRotation(moveVector);
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
