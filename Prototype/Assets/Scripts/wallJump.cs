using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallJump : MonoBehaviour
{
    public float distance = 4f;
    private Movement playerMovement;
    private Grounded ground;
    private Rigidbody player;
    public bool wallSliding;
    public Transform wallCheckpoint;
    public bool wallCheck;
    public LayerMask wallLayerMask;
     
    // Use this for initialization
    void Start()
    {
        playerMovement = GetComponent<Movement>();
        ground = GetComponent<Grounded>();
        player = GetComponent<Rigidbody>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!transform.GetChild(2).GetComponent<Grounded>().grounded)
        {
            //wallCheck = Physics2D.OverlapCircle(wallCheckpoint.position, 0.5f, wallLayerMask);
            Collider[] hitColliders = Physics.OverlapSphere(wallCheckpoint.position, 0.005f,wallLayerMask);
            if (hitColliders.Length != 0)
            {
                Debug.Log("Colliding");
                wallCheck = true;
                Debug.Log("wallcheck" + wallCheck);
            }
            else
            {
                wallCheck = false;
            }

            //if ((playerMovement.facingRight && Input.GetAxis("Horizontal")>0.1f )||(!playerMovement.facingRight && Input.GetAxis("Horizontal") < 0.1f))
            //{
                if(wallCheck)
                {
                    handleWallSliding();
                }
            //}
        }

        if(wallCheck == false )
        {
            wallSliding = false;
            Debug.Log("wallsliding " + wallSliding);
        }
    }

    private void handleWallSliding()
    {
        wallSliding = true;
        Debug.Log("wallsliding set" + wallSliding);
        player.velocity = new Vector3(player.velocity.x, -0.7f, player.velocity.z);
        wallSliding = true;
        if(Input.GetButtonDown("Jump"))
        {
            if(playerMovement.facingRight)
            {
                playerMovement.flip();
                player.AddForce(new Vector3(-10, 25, 0)*distance);
            }
            else
            {
                playerMovement.flip();
                player.AddForce(new Vector3(10, 25, 0) * distance);
            }
            
        }
        wallCheck = false;
        Debug.Log("wallcheck set" + wallCheck);
    }

}
