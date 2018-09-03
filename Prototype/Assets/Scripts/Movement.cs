using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float speed = 5;
    public float jumpVelocity = 5;
    public float cooldownTime =2f;
    private float nextFiretime = 0f;
    private Rigidbody player;
    private SpriteRenderer sprite;
    private Animator myAnim;
    bool facingRight;
    public Rigidbody knifePrefab;
    public Transform handEnd;
    private bool isGrounded;
    private Rigidbody knifeInstance;
    // Use this for initialization
    void Start () {
        player = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();
        facingRight = true;
	}
	
	// Update is called once per frame
	void Update () {
        
        float hAxis = Input.GetAxis("Horizontal");
        myAnim.SetFloat("Speed", Mathf.Abs(hAxis));
        Vector3 movement = new Vector3(hAxis, 0, 0) * speed * Time.deltaTime;
        player.MovePosition(transform.position + movement);

        if(hAxis > 0 && !facingRight)
        {
            myAnim.SetBool("isWalking", true);
            myAnim.SetBool("isIdle", false);
            flip();
        }
        else if (hAxis<0 && facingRight)
        {
            myAnim.SetBool("isWalking", true);
            myAnim.SetBool("isIdle", false);
            flip();
        }
        else if(hAxis == 0)
        {
            myAnim.SetBool("isWalking", false);
            myAnim.SetBool("isIdle", true);
        }

        if (Input.GetMouseButton(0))
        {
            myAnim.SetBool("isAttacking", true);
        }
        else
        {
            myAnim.SetBool("isAttacking", false);
        }
            

        if (Input.GetKey(KeyCode.Tab))
        {
           // player.enabled = false;
            sprite.enabled = false;

            if(!facingRight)
            {
                Vector3 targetPos = transform.position + transform.right * -0.25f;
                player.transform.position = targetPos;
                Invoke("Enabler", 0.5f);
            }       
            else
            {
                Vector3 targetPos = transform.position + transform.right * 0.25f;
                player.transform.position = targetPos;
                Invoke("Enabler", 0.5f);
            }
        }
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            player.velocity = Vector3.up * jumpVelocity;
        }
        if (Time.time > nextFiretime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                // Rigidbody knifeInstance;
                nextFiretime = Time.time + cooldownTime;
                if (facingRight)
                {
                    knifeInstance = Instantiate(knifePrefab, handEnd.position, knifePrefab.rotation) as Rigidbody;
                    knifeInstance.AddForce(handEnd.right * 1500);
                }
                else
                {
                    knifeInstance = Instantiate(knifePrefab, handEnd.position, knifePrefab.rotation) as Rigidbody;
                    knifeInstance.AddForce(handEnd.right * -1500);
                }


            }
        }
       
    }
  
    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    void Enabler()
    {
        //player.enabled = true;
        sprite.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    { 

        if (collision.gameObject.tag  == "ground")
        {
            Debug.Log("colliding");
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "ground")
        {
            
            isGrounded = false;
        }
    }
}
