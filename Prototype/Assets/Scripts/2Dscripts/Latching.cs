using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Latching : MonoBehaviour {
    public Transform checkpoint;
    private Rigidbody2D player;
    private PlayerMovement movement;
    float maxDistance = Mathf.Infinity;
    bool hitCollider;
    public float speedX = 10;
    public float speedY;
    RaycastHit2D hit;
    bool walkonWalls;

	// Use this for initialization
	void Start () {
        movement = GetComponent<PlayerMovement>();
        player = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxisRaw("RHorizontal");
        float vAxis = Input.GetAxisRaw("RVertical");
        Vector2 direction = new Vector2(hAxis, vAxis);
        if ((hAxis > 0.1 || hAxis < -0.1) && (vAxis > 0.1 || vAxis < -0.1))
        {
            hit = Physics2D.Raycast(checkpoint.position, direction, maxDistance, LayerMask.GetMask("Walls"));
            Debug.DrawRay(checkpoint.position, direction, Color.green);
            if (hit.collider != null)
            {

                Debug.Log("Hitting Wall");
                hitCollider = true;
            }
            else
                hitCollider = false;
        }
        


        if (hitCollider && Input.GetButtonDown("RightTrigger1"))
        {
            Debug.Log("code for latching");
            movement.enabled = false;
           
            player.gravityScale = 0f;            
            Debug.Log("raycast rot " + hit.transform.rotation.y);           
            Vector2 dir = (hit.collider.gameObject.transform.position - checkpoint.position);
            Debug.Log("Direction :" + dir);
            float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Debug.Log("rotation along z axis " + rot_z);
            //Quaternion rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, rot_z);           
            player.transform.rotation = Quaternion.FromToRotation(Vector3.up, -dir);
            Debug.Log("hit collider normal : " + hit.normal);
            player.velocity = new Vector2(dir.x , dir.y) * speedX;
            Debug.Log("velocity" + player.velocity);           
        }       


    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Debug.Log("write wall walking script");
            walkonWalls = true;

        }
    }

}
