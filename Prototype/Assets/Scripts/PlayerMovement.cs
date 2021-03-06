﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool trackJumpHeight = false;
    float hAxis;
    float trackedYPosition;
    float highestJumpHeight;

    public float speed = 5;
    public float jumpVelocity = 5;
    private Rigidbody2D player;
    public Animator myAnim;
    public bool facingRight;
    public float cooldownTime = 2f;
    private float nextFiretime = 0f;
    public GameObject knifePrefab;
    public Transform handEnd;
    private SpriteRenderer sprite;
    public static PlayerMovement sharedInstance;
    private WallJump wallJumpScript;

    float minJumpSpeed = 2.0f;

    [Header("ShadowSlash Collider")]
    public Collider2D sscollider;

    static PlayerMovement()
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
    void Start()
    {
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
        player = GetComponent<Rigidbody2D>();
        wallJumpScript = GetComponent<WallJump>();

        myAnim = GameObject.Find("Normal").GetComponent<Animator>();
        //myAnim = GetComponent<Animator>();
        facingRight = true;
        sprite = GetComponentInChildren<SpriteRenderer>();

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
    void Update()
    {
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
            sscollider = transform.Find("Demon").gameObject.GetComponent<BoxCollider2D>();
        }
       
        float hAxis = Input.GetAxis("Horizontal");
        myAnim.SetFloat("Speed", Mathf.Abs(hAxis));
        player.velocity = new Vector3(hAxis * speed, player.velocity.y, 0);
        //float phAxis = Input.GetAxis("PS4Horizontal");
        //myAnim.SetFloat("Speed", Mathf.Abs(phAxis));
        //player.velocity = new Vector3(phAxis * speed, player.velocity.y, 0);



        if (hAxis > 0 && !facingRight) 
        {
            myAnim.SetBool("isWalking", true);
            myAnim.SetBool("isIdle", false);
            flip();
        }
        else if (hAxis < 0 && facingRight) 
        {
            myAnim.SetBool("isWalking", true);
            myAnim.SetBool("isIdle", false);
            flip();
        }
        else if (hAxis == 0) 
        {
            myAnim.SetBool("isWalking", false);
            myAnim.SetBool("isIdle", true);
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



        // Jumping

        //Keyboard
        if ((Input.GetButtonDown("Jump") && transform.GetChild(2).GetComponent<Grounded2D>().grounded && !wallJumpScript.wallSliding) ||
            (Input.GetButtonDown("PS4Jump") && transform.GetChild(2).GetComponent<Grounded2D>().grounded && !wallJumpScript.wallSliding))
        {
            player.velocity = Vector3.up * jumpVelocity;// *(1f + movement.magnitude * 2f);
            //Debug.Log(player.velocity);
        }

        if ((Input.GetButtonUp("Jump") && !transform.GetChild(2).GetComponent<Grounded2D>().grounded && !wallJumpScript.wallSliding && player.velocity.y > minJumpSpeed) ||
            (Input.GetButtonUp("PS4Jump") && !transform.GetChild(2).GetComponent<Grounded2D>().grounded && !wallJumpScript.wallSliding && player.velocity.y > minJumpSpeed)) //||
            //(Input.GetButtonUp("XboxJump") && !transform.GetChild(2).GetComponent<Grounded2D>().grounded && !wallJumpScript.wallSliding && player.velocity.y > minJumpSpeed))
        {
            player.velocity = Vector3.up * minJumpSpeed;
        }







        // Ranged Attack

        if (Time.time > nextFiretime)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                myAnim.SetBool("isAttacking", true);

                // Rigidbody knifeInstance;
                nextFiretime = Time.time + cooldownTime;

                StartCoroutine("DelayedAttack");
            }
            else
                myAnim.SetBool("isAttacking", false);
        }
        else if (!Input.GetKeyDown(KeyCode.E))
        {
            myAnim.SetBool("isAttacking", false);
        }


        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            //BackGround shift


        }
    }


    // Flipping chracter

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


    IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(0.1f);

        if (facingRight)
        {
            var knifeInstance = Instantiate(knifePrefab, handEnd.position, Quaternion.identity);
            knifeInstance.GetComponent<Rigidbody2D>().velocity = handEnd.right * 5;
        }
        else
        {
            var knifeInstance = Instantiate(knifePrefab, handEnd.position, new Quaternion(knifePrefab.transform.rotation.x, knifePrefab.transform.rotation.y, knifePrefab.transform.rotation.z, 1));
            knifeInstance.GetComponent<SpriteRenderer>().flipX = true;
            knifeInstance.GetComponent<Rigidbody2D>().velocity = -handEnd.right * 5;
        }
    }
}
