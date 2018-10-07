using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Latching : MonoBehaviour {
    public Transform checkpoint;
    private Rigidbody2D player;
    private Movement2D movement;
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
        Debug.Log("Right analog values " + hAxis + vAxis);
        if ((hAxis > 0.1 || hAxis < -0.1) && (vAxis > 0.1 || vAxis < -0.1))
        {
            hit = Physics2D.Raycast(checkpoint.position, direction, maxDistance, LayerMask.GetMask("Walls", "Ground", "Ceilings"));
            Debug.DrawRay(checkpoint.position, direction, Color.red);
            if (hit.collider != null)
            {

                Debug.Log("Hitting Wall");
                hitCollider = true;
            }

        }
        else
            hitCollider = false;
       



        if (hitCollider && Input.GetButtonUp("RightTrigger1"))
        {
            Debug.Log("code for latching");
            movement.enabled = false;

            player.gravityScale = 0f;
            Debug.Log("raycast rot " + hit.transform.rotation.y);
            dir = (hit.collider.gameObject.transform.position - checkpoint.position);
            Debug.Log("Direction :" + dir);
            float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Debug.Log("rotation along z axis " + rot_z);
            
            player.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            Debug.Log("hit collider normal : " + hit.normal);
            player.velocity = new Vector2(0, 0);
            player.velocity = new Vector2(dir.x, dir.y) * speedX;
            Debug.Log("velocity" + player.velocity);
        }

        if (walkonCeilings)
        {
            movement.enabled = true;
            player.gravityScale = -1.5f;
        }
        else if(walkonGround)
        {
            movement.enabled = true;
            player.gravityScale = 1.5f;
        }
        else if (walkonWalls && dir.x < 0)
        {
            movement.enabled = true;
            player.gravityScale = 0;
            Force.force = new Vector2(-1.5f, 0);
            
        }
        else if (walkonWalls && dir.x > 0)
        {
            movement.enabled = true;
            player.gravityScale = 0;
            Force.force = new Vector2(1.5f, 0);
        }


      
       
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

