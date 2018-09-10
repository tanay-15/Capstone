using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallJump : MonoBehaviour
{
    public float distance = 4f;
    private Movement playerMovement;
    private Grounded ground;
    private Rigidbody player;
    private bool hitWall;
     
    // Use this for initialization
    void Start()
    {
        playerMovement = GetComponent<Movement>();
        ground = GetComponent<Grounded>();
        player = GetComponent<Rigidbody>();
        hitWall = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        
        
        
        if(Input.GetKeyDown(KeyCode.Space) && !transform.GetChild(2).GetComponent<Grounded>().grounded && hitWall)
        {
            Debug.Log("Wall Jump Test");
            if(playerMovement.facingRight)
            {
                player.AddForce(-2*distance, 3*distance, 0, ForceMode.Impulse);
                //playerMovement.flip();
                hitWall = false;
            }
            else if(!playerMovement.facingRight)
            {
                player.AddForce(2 * distance, 3* distance, 0, ForceMode.Impulse);
                //playerMovement.flip();
                hitWall = false;
            }
           
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Wall")
        {
            hitWall = true;
        }
    }
}
