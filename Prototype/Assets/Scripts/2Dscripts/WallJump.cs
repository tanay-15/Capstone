using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    public float distance = 4f;
    public float maxDistance = 30f;
    Vector2 Direction;
    float hAxis,vAxis;
    private PlayerMovement playerMovement;
    private Grounded2D ground;
    private Rigidbody2D player;
    public bool wallSliding;
    public Transform wallCheckpoint;
    public Transform wallCheckpoint1;
    public bool wallCheck;
    public LayerMask wallLayerMask;

    // Use this for initialization
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        ground = transform.GetChild(2).GetComponent<Grounded2D>();
        player = GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        Direction = new Vector2(hAxis, vAxis);  
        Direction = transform.TransformDirection(Direction);
        //float angle = Mathf.Atan2(vAxis, hAxis) * Mathf.Rad2Deg;




        //RaycastHit2D hit = Physics2D.Raycast(wallCheckpoint.position, Direction, maxDistance, LayerMask.GetMask("Walls"));
        // Debug.DrawRay(wallCheckpoint.position, Direction, Color.green);
        Collider2D hit = Physics2D.OverlapCircle(wallCheckpoint.position, 0.05f, wallLayerMask);
        Collider2D hitback = Physics2D.OverlapCircle(wallCheckpoint1.position, 0.05f, wallLayerMask);
        if ((hit && !ground.grounded) || (hitback && !ground.grounded))
        {
          wallCheck = true;
           
          
          
        }
        else
        {
          wallCheck = false;
          wallSliding = false;
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
            RaycastHit2D hitRay = Physics2D.Raycast(wallCheckpoint.position, Direction, maxDistance, LayerMask.GetMask("Walls"));
            Debug.DrawRay(wallCheckpoint.position, Direction, Color.red);
            if(hitRay.collider !=null)
            {
                Debug.Log("not null");
                if(playerMovement.facingRight && Input.GetButtonDown("Jump") && Input.GetAxisRaw("Horizontal")> 0)
                {
                    playerMovement.flip();
                    player.AddForce(new Vector2(-10, 15) * distance);
                }
                else if(!playerMovement.facingRight && Input.GetButtonDown("Jump"))
                {
                    player.AddForce(new Vector2(10, 15) * distance);
                }
                player.AddForce(Direction * distance);
            }
                
            
        }
            
        
        //if (!ground.grounded)
        //{

        //    Collider2D hitColliders = Physics2D.OverlapCircle(wallCheckpoint.position, 0.05f, wallLayerMask);
        //    if (hitColliders)
        //    {
        //        Debug.Log("Colliding");
        //        wallCheck = true;
        //        Debug.Log("wallcheck" + wallCheck);
        //    }
        //    else
        //    {
        //        wallCheck = false;
        //    }

        //    if (wallCheck)
        //    {

        //        handleWallSliding();
        //    }

        //}
        //else
        //{
        //    wallCheck = false;
        //    wallSliding = false;
        //}

        //if (wallSliding)
        //{
        //    if ((Input.GetKeyDown("space") && Input.GetAxisRaw("Horizontal") > 0.1f && playerMovement.facingRight) || Input.GetKeyDown("space"))
        //    {
        //        if (playerMovement.facingRight)
        //        {
        //            playerMovement.flip();
        //            player.AddForce(new Vector2(-10, 15) * distance);

        //        }
        //        else if (!playerMovement.facingRight)
        //        {
        //            playerMovement.flip();
        //            player.AddForce(new Vector2(10, 15) * distance);

        //        }
        //        wallSliding = false;
        //    }
        //    else if ((Input.GetKeyDown("space") && Input.GetAxisRaw("Horizontal") < 0.1f && !playerMovement.facingRight) || Input.GetKeyDown("space"))
        //    {
        //        if (playerMovement.facingRight)
        //        {
        //            playerMovement.flip();
        //            player.AddForce(new Vector2(-10, 15) * distance);

        //        }
        //        else if (!playerMovement.facingRight)
        //        {
        //            playerMovement.flip();
        //            player.AddForce(new Vector2(10, 15) * distance);

        //        }
        //        wallSliding = false;
        //    }

        //}
    }

    private void handleWallSliding()
    {
        //wallSliding = true;
        //Debug.Log("wallsliding set" + wallSliding);
        if (!playerMovement.facingRight && hAxis > 0)
        {
            Debug.Log("will face left");
            playerMovement.flip();
        }
        else if (playerMovement.facingRight && hAxis < 0)
        {
            playerMovement.flip();
        }
        player.velocity = new Vector2(player.velocity.x, -0.8f);

    }
  
    
}
