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
        if (!ground.grounded)
        {
            
            Collider2D hitColliders = Physics2D.OverlapCircle(wallCheckpoint.position, 0.05f, wallLayerMask);
            if (hitColliders)
            {
                Debug.Log("Colliding");
                wallCheck = true;
                Debug.Log("wallcheck" + wallCheck);
            }
            else
            {
                wallCheck = false;
            }

            if (wallCheck)
            {

                handleWallSliding();
            }

        }
        else
        {
            wallCheck = false;
            wallSliding = false;
        }

        if (wallSliding)
        {
            if ((Input.GetKeyDown("space") && Input.GetAxisRaw("Horizontal") > 0.1f && playerMovement.facingRight) || Input.GetKeyDown("space"))
            {
                if (playerMovement.facingRight)
                {
                    playerMovement.flip();
                    player.AddForce(new Vector2(-10, 15) * distance);
                    
                }
                else if (!playerMovement.facingRight)
                {
                    playerMovement.flip();
                    player.AddForce(new Vector2(10, 15) * distance);
                   
                }
                wallSliding = false;
            }
            else if ((Input.GetKeyDown("space") && Input.GetAxisRaw("Horizontal") < 0.1f && !playerMovement.facingRight) || Input.GetKeyDown("space"))
            {
                if (playerMovement.facingRight)
                {
                    playerMovement.flip();
                    player.AddForce(new Vector2(-10, 15) * distance);
                    
                }
                else if (!playerMovement.facingRight)
                {
                    playerMovement.flip();
                    player.AddForce(new Vector2(10, 15) * distance);
                    
                }
                wallSliding = false;
            }

        }
    }

    private void handleWallSliding()
    {
        wallSliding = true;
        Debug.Log("wallsliding set" + wallSliding);
        player.velocity = new Vector2(player.velocity.x, -0.8f);
    }

}
