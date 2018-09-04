using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float speed = 5;
    public float jumpVelocity = 5;
    private Rigidbody player;
    public Animator myAnim;
    bool facingRight;
    public float cooldownTime = 2f;
    private float nextFiretime = 0f;
    public Rigidbody knifePrefab;
    public Transform handEnd;
    private SpriteRenderer sprite;

    [Header("ShadowSlash Collider")]
    public Collider2D sscollider;

    //private Rigidbody knifeInstance;
    // Use this for initialization
    void Start () {
        player = GetComponent<Rigidbody>();
        
        myAnim = GameObject.Find("Normal").GetComponent<Animator>();
        //myAnim = GetComponent<Animator>();
        facingRight = true;
        sprite = GetComponentInChildren<SpriteRenderer>();

}
	
	// Update is called once per frame
	void Update () {

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
        if (Input.GetButtonDown("Jump") && transform.GetChild(2).GetComponent<Grounded>().grounded)
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
}
