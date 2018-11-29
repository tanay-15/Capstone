using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Latching : MonoBehaviour {
    public Transform checkpoint;
    private Rigidbody2D player;
    private Movement2D movement;
    private vMovement verticalMovement;
    float maxDistance = Mathf.Infinity;
    [SerializeField]
    bool hitCollider;
    public float speedX = 10;
    public float speedY;
    RaycastHit2D hit;
    [SerializeField]
    public bool walkonWalls;
    [SerializeField]
    public bool walkonCeilings;
    [SerializeField]
    public bool walkonGround;
    [SerializeField]
    private bool noFlagSet;
    Vector2 dir;
    ConstantForce2D Force;

    // Use this for initialization
    void Start() {
        movement = GetComponent<Movement2D>();
        verticalMovement = GetComponent<vMovement>();
        player = GetComponent<Rigidbody2D>();
        Force = GetComponent<ConstantForce2D>();
        Debug.Log("gravity scale :" + player.gravityScale);
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxisRaw("RHorizontal");
        float vAxis = Input.GetAxisRaw("RVertical");
        Vector2 direction = new Vector2(hAxis, vAxis);
        
        if ((hAxis > 0.1 || hAxis < -0.1) || (vAxis > 0.1 || vAxis < -0.1))
        {
            hit = Physics2D.Raycast(checkpoint.position, direction, maxDistance, LayerMask.GetMask( "Ground", "Ceilings"));
            Debug.DrawRay(checkpoint.position, direction, Color.red);
            if (hit.collider != null)
            {
                hitCollider = true;
            }

        }
        else
            hitCollider = false;
       



        if (hitCollider && Input.GetButtonUp("RightTrigger1"))
        {
           
            movement.enabled = false;

            player.gravityScale = 0f;
            
            dir = (hit.collider.gameObject.transform.position - checkpoint.position);
            
            float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            player.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
           
            player.velocity = new Vector2(0, 0);
            player.velocity = new Vector2(dir.x, dir.y) * speedX;
            
        }

        if (walkonCeilings && !hitCollider)
        {
            //Debug.Log("Hit collider value :" + hitCollider);
            movement.enabled = true;
            verticalMovement.enabled = false;
            player.gravityScale = -1.5f;
        }
        else if(walkonGround && !hitCollider)
        {
            Debug.Log("Hit collider value :" + hitCollider);
            movement.enabled = true;
            verticalMovement.enabled = false;
            player.gravityScale = 1.5f;
            Debug.Log("gravity: " + player.gravityScale);
        }
        //else if (walkonWalls && dir.x < 0 && !hitCollider)
        //{
        //    verticalMovement.enabled = true;
        //    movement.enabled = false;
        //    player.gravityScale = 0;
        //    Force.force = new Vector2(-1.5f, 0);
            
        //}
        //else if (walkonWalls && dir.x > 0 && !hitCollider)
        //{
        //    verticalMovement.enabled = true;
        //    movement.enabled = false;
        //    player.gravityScale = 0;
        //    Force.force = new Vector2(1.5f, 0);
        //}     
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {          
            walkonWalls = true;
            walkonCeilings = false;
            walkonGround = false;            
        }
        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Ceilings"))
        {            
            walkonCeilings = true;
            walkonGround = false;
            walkonWalls = false;          
        }
        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {           
            walkonGround = true;
            walkonWalls = false;
            walkonCeilings = false;            
        }
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            walkonWalls = true;
            walkonCeilings = false;
            walkonGround = false;
        }
        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Ceilings"))
        {
            walkonCeilings = true;
            walkonGround = false;
            walkonWalls = false;
        }
        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            walkonGround = true;
            walkonWalls = false;
            walkonCeilings = false;
        }
    }



    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            walkonWalls = false;
        }
        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Ceilings"))
        {
            walkonCeilings = false;
        }
        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            walkonGround = false;
        }
    }
}

