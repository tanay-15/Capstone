﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    bool trackJumpHeight = false;

    float trackedYPosition;
    float highestJumpHeight;

    public float speed = 5;
    public float jumpVelocity = 5;
    private Rigidbody player;
    public Animator myAnim;
    public bool facingRight;
    public float cooldownTime = 2f;
    private float nextFiretime = 0f;
    public Rigidbody knifePrefab;
    public Transform handEnd;
    private SpriteRenderer sprite;
    public static Movement sharedInstance;
    private WallJump wallJumpScript;

    float minJumpSpeed = 2.0f;

    [Header("ShadowSlash Collider")]
    public Collider2D sscollider;

    static Movement()
    {
        sharedInstance = null;
    }

    void ResetJumpTracking()
    {
        Debug.Log("Jump tracking reset.");
        trackedYPosition = transform.position.y;
        highestJumpHeight = 0f;
    }

    //private Rigidbody knifeInstance;
    // Use this for initialization
    void Start () {
        if (trackJumpHeight)
            ResetJumpTracking();

        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        player = GetComponent<Rigidbody>();
        
        myAnim = GameObject.Find("Normal").GetComponent<Animator>();
        //myAnim = GetComponent<Animator>();
        facingRight = true;
        sprite = GetComponentInChildren<SpriteRenderer>();
        wallJumpScript = GetComponent<WallJump>();

    }

    void FixedUpdate()
    {
        if (trackJumpHeight)
        {
            float height = transform.position.y - trackedYPosition;
            if (height > highestJumpHeight)
            {
                highestJumpHeight = height;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (trackJumpHeight)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetJumpTracking();
            }
            Debug.Log("Highest jump height: " + highestJumpHeight + "\nTracked y position: " + trackedYPosition);
        }


        if (transform.Find("Demon").gameObject.activeSelf)
        {
            sscollider = transform.Find("Demon").gameObject.GetComponent<BoxCollider2D> ();
        }

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

        if (Input.GetMouseButtonDown(0))
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
           // sprite.enabled = false;
            sprite.enabled = false;

            if (!facingRight)
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
        //if(Input.GetButtonDown("Jump") && isGrounded)
        if (Input.GetButtonDown("Jump") && transform.GetChild(2).GetComponent<Grounded>().grounded && !wallJumpScript.wallSliding)
        {
            player.velocity = Vector3.up * jumpVelocity * (1f + movement.magnitude * 2f);
        }

        if (Input.GetButtonUp("Jump") && !transform.GetChild(2).GetComponent<Grounded>().grounded && !wallJumpScript.wallSliding && player.velocity.y > minJumpSpeed)
        {
            player.velocity = Vector3.up * minJumpSpeed;
        }

        if (Time.time > nextFiretime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                
                // Rigidbody knifeInstance;
                nextFiretime = Time.time + cooldownTime;
                if (facingRight)
                {
                    Debug.Log("right click");
                    var knifeInstance = Instantiate(knifePrefab, handEnd.position, Quaternion.identity);
                    knifeInstance.GetComponent<Rigidbody>().velocity = handEnd.right * 2;
                }
                else
                {
                    var knifeInstance = Instantiate(knifePrefab, handEnd.position, Quaternion.identity);
                    knifeInstance.GetComponent<Rigidbody>().velocity = -handEnd.right * 2;
                }
            }
        }
            if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            //BackGround shift


        }
	}
    public void flip()
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
}
