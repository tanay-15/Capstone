using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    public float distance = 4f;
    private Movement2D playerMovement;
    private Grounded2D ground;
    private Rigidbody2D player;
    public bool wallSliding;
    public Transform wallCheckpoint;
    public bool wallCheck;
    public LayerMask wallLayerMask;

    // Use this for initialization
    void Start()
    {
        playerMovement = GetComponent<Movement2D>();
        ground = transform.GetChild(2).GetComponent<Grounded2D>();
        player = GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(player.velocity);
        if (!ground.grounded)
        {
            //wallCheck = Physics2D.OverlapCircle(wallCheckpoint.position, 0.5f, wallLayerMask);
            Collider2D hitColliders = Physics2D.OverlapCircle(wallCheckpoint.position, 0.05f, wallLayerMask);
            if (hitColliders)
            {
                //Debug.Log("wallcheck: " + wallCheck + "\nColliding");
                wallCheck = true;
            }
            else
            {
                wallCheck = false;
            }

            //if ((playerMovement.facingRight && Input.GetAxis("Horizontal")>0.1f )||(!playerMovement.facingRight && Input.GetAxis("Horizontal") < 0.1f))
            //{
            if (wallCheck)
            {

                handleWallSliding();
            }
            //}
        }

        //Set these variables back to false if the player is grounded
        else if (ground.grounded)
        {
            wallCheck = false;
            wallSliding = false;
        }

        if(wallSliding && Input.GetKeyDown("space"))
        {          
            //Use GetAxisRaw to handle if a button is being pressed or not
                if ((playerMovement.facingRight && Input.GetAxisRaw("Horizontal") >0.01f))// || playerMovement.facingRight)
                {
                    playerMovement.flip();
                    player.AddForce(new Vector2(-10, 15) * distance);
                    //wallCheck = false;
                }
                else if((!playerMovement.facingRight && Input.GetAxisRaw("Horizontal") < -0.01f))// || !playerMovement.facingRight)
                {
                    playerMovement.flip();
                    player.AddForce(new Vector2(10, 15) * distance);
                    //wallCheck = false;
                }

            
            wallCheck = false;
            //Debug.Log("wallcheck set" + wallCheck);
        }

        if (wallCheck == false)
        {
            wallSliding = false;
            //Debug.Log("wallsliding " + wallSliding);
        }


    }

    private void handleWallSliding()
    {
        wallSliding = true;
        Debug.Log("wallsliding set" + wallSliding);
        player.velocity = new Vector2(player.velocity.x, -0.8f);
    }

}
