using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    public float forceX;
    public float forceY;
    public float maxDistance = 30f;
    Vector2 Direction;
    float hAxis,vAxis;
    private Movement2D playerMovement;
    private Grounded2D ground;
    private Rigidbody2D player;
    public bool wallSliding;
    public Transform wallCheckpoint;
    public Transform wallCheckpoint1;
    public bool wallCheck;
    public bool iswallSliding;
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
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        Direction = new Vector2(hAxis, vAxis).normalized;
        
        Collider2D hit = Physics2D.OverlapCircle(wallCheckpoint.position, 0.05f, wallLayerMask);
        Collider2D hitback = Physics2D.OverlapCircle(wallCheckpoint1.position, 0.05f, wallLayerMask);
        if ((hit && !ground.grounded) || (hitback && !ground.grounded))
        {
             wallCheck = true;
            if(hit != null)
                {
                    playerMovement.flip();
                }                  
        }
        else
        {
            wallCheck = false;
            wallSliding = false;
            iswallSliding = false;
        }

        if(wallCheck)
        {
            wallSliding = true;
        }
        if(wallSliding)
        {
          handleWallSliding();
        }
      
        if (wallCheck && wallSliding)
        {
            playerMovement.enabled = false;
            iswallSliding = true;

            if (playerMovement.facingRight)
            {
                if (Input.GetButtonDown("Jump") || Input.GetButtonDown("PS4Jump"))
                {               
                    player.velocity = new Vector2(0, 0);
                    player.velocity = new Vector2(forceX, forceY);                
                }
                else if (Input.GetButtonDown("PS4CIRCLE"))
                {
                    player.velocity = new Vector2(2, 2);
                }
            }
            else if(!playerMovement.facingRight)
            {
                if (Input.GetButtonDown("Jump") || Input.GetButtonDown("PS4Jump"))
                {                                        
                    player.velocity = new Vector2(0, 0);
                    player.velocity = new Vector2(-forceX, forceY);
                }
                else if (Input.GetButtonDown("PS4CIRCLE"))
                {
                    player.velocity = new Vector2(-2, 2);
                }
            }
               
           // }
            
                
            
        }      
        if(ground.grounded)
        {
            //Debug.Log("Testing");
            playerMovement.enabled = true;
        }
        
        
    }

    private void handleWallSliding()
    {
        if (!Input.GetButton("Jump") && !Input.GetButton("PS4Jump"))
        {
            player.velocity = new Vector2(player.velocity.x, -0.8f);
        }
           

    }
  
    
}
