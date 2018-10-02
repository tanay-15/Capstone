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
        //Direction = transform.TransformDirection(Direction);
        //float angle = Mathf.Atan2(vAxis, hAxis) * Mathf.Rad2Deg;




        //RaycastHit2D hit = Physics2D.Raycast(wallCheckpoint.position, Direction, maxDistance, LayerMask.GetMask("Walls"));
        // Debug.DrawRay(wallCheckpoint.position, Direction, Color.green);
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
           
            if ((hAxis > 0.1 || hAxis < -0.1) && (vAxis > 0.1 || vAxis < -0.1))
            {
                //RaycastHit2D hitRay = Physics2D.Raycast(wallCheckpoint.position, Direction, maxDistance, LayerMask.GetMask("Walls"));
                //Debug.DrawRay(wallCheckpoint.position, Direction, Color.red);
                //if (hitRay.collider != null)
                RaycastHit2D hitTest = Physics2D.Raycast(wallCheckpoint.position, Direction, maxDistance);
                Debug.DrawRay(wallCheckpoint.position, Direction, Color.green);
                if (Input.GetButtonDown("Jump") || Input.GetButtonDown("PS4Jump"))
                {
                    Debug.Log("inside walljump");
                    //Debug.Log("Direction" + Direction);
                    //player.AddForce(Direction * force);
                    //player.velocity = new Vector2(Direction.x,Direction.y) * force;
                    Debug.Log("Direction for jump " + Direction);
                    player.velocity = new Vector2(0,0);
                    //player.AddForce(new Vector2(Direction.x * forceX, Direction.y * forceY) * maxDistance);
                    player.velocity = new Vector2(Direction.x * forceX, Direction.y * forceY) * maxDistance;
                }
            }
            
                
            
        }
        //else
        //{
        //    playerMovement.enabled = true;
        //}
        if(ground.grounded)
        {
            Debug.Log("Testing");
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
