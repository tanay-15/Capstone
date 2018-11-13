using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    bool trackJumpHeight = false;

    float trackedYPosition;
    float highestJumpHeight;
    int currentAnimation = 0;
    public float speed = 5;
    public float jumpVelocity = 5;
    private Rigidbody2D player;
    public Animator myAnim;
    public Animator demonAnim;
    public bool facingRight;
    public float cooldownTime = 1f;
    private float nextFiretime = 0f;
    public GameObject knifePrefab;
    public GameObject chargedArrowPrefab;
    public Transform handEnd;
    private SpriteRenderer sprite;
    public static Movement2D sharedInstance;
    private WallJump wallJumpScript;
    private Latching latchingScript;
    float minJumpSpeed = 2.0f;

    public GameObject shinePrefab;
    Vector3 shinePosition = new Vector3(0.4f, 0f, 0f);
    float arrowChargeTime = 0.6f;

    [Header("ShadowSlash Collider")]
    public Collider2D sscollider;

    static Movement2D()
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

        if (sharedInstance != null)
        {
            Destroy(sharedInstance);
        }
        sharedInstance = this;
        player = GetComponent<Rigidbody2D>();
        wallJumpScript = GetComponent<WallJump>();

        myAnim = GameObject.Find("Normal").GetComponent<Animator>();
        //demonAnim = GameObject.Find("Demon").GetComponent<Animator>();
        //myAnim = GetComponent<Animator>();
        facingRight = true;
        sprite = GetComponentInChildren<SpriteRenderer>();
        latchingScript = GetComponent<Latching>();
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

        if(hAxis > 0.5f)
            player.velocity = new Vector3(Mathf.Sign(hAxis) * speed, player.velocity.y, 0);
        else
            player.velocity = new Vector3(hAxis * speed, player.velocity.y, 0);
        //Vector3 movement = new Vector3(hAxis, 0, 0) * speed * Time.deltaTime;
        //player.MovePosition(transform.position + movement);
        //player.position += new Vector2(movement.x,movement.y);

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

        //if(Input.GetButtonDown("Jump") && isGrounded)
        if ((Input.GetButtonDown("Jump") && transform.GetChild(2).GetComponent<Grounded2D>().grounded && !wallJumpScript.wallSliding) ||
             (Input.GetButtonDown("PS4Jump") && transform.GetChild(2).GetComponent<Grounded2D>().grounded && !wallJumpScript.wallSliding))
        {
            player.velocity = Vector3.up * jumpVelocity;// *(1f + movement.magnitude * 2f);
            //Debug.Log(player.velocity);
        }

        if ((Input.GetButtonUp("Jump") && !transform.GetChild(2).GetComponent<Grounded2D>().grounded && !wallJumpScript.wallSliding && player.velocity.y > minJumpSpeed) ||
            (Input.GetButtonUp("PS4Jump") && !transform.GetChild(2).GetComponent<Grounded2D>().grounded && !wallJumpScript.wallSliding && player.velocity.y > minJumpSpeed))
        {
            player.velocity = Vector3.up * minJumpSpeed;
        }







// Ranged Attack

        if (Time.time > nextFiretime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                myAnim.SetBool("isAttacking", true);

                // Rigidbody knifeInstance;
                nextFiretime = Time.time + cooldownTime;

                StartCoroutine("ShootArrow");   //"DelayedAttack"
                UIIcons.sharedInstance.icons[1].GetComponent<UIIcon>().SetEmpty();
            }
            else
                myAnim.SetBool("isAttacking", false);
        }
        else if (!Input.GetKeyDown(KeyCode.E))
        {
            myAnim.SetBool("isAttacking", false);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            //if(currentAnimation == -1)
            switch(currentAnimation)
            {
                case 0:
                    myAnim.ResetTrigger("secondPunch");
                    myAnim.SetTrigger("firstPunch");
                    myAnim.SetBool("isPunching", true);
                    currentAnimation++;
                    
                    break;
                case 1:
                    myAnim.ResetTrigger("firstPunch");
                    myAnim.SetTrigger("secondPunch");
                    myAnim.SetBool("isPunching", true);
                    currentAnimation = 0;
                    Debug.Log("z being pressed");
                    break;
                default:
                    myAnim.SetBool("isPunching", false);
                    myAnim.ResetTrigger("firstPunch");
                    myAnim.ResetTrigger("secondPunch");
                    break;
            }
            
            
            
        }
        else
        {
            myAnim.SetBool("isPunching", false);   
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

        theScale = GameObject.Find("OrbitingSystem").transform.localScale;
        theScale.x *= -1;
        GameObject.Find("OrbitingSystem").transform.localScale = theScale;

        /*
        facingRight = !facingRight;
        transform.Find("Normal").GetComponent<SpriteRenderer>().flipX = !transform.Find("Normal").GetComponent<SpriteRenderer>().flipX;
        transform.Find("Demon").GetComponent<SpriteRenderer>().flipX = !transform.Find("Demon").GetComponent<SpriteRenderer>().flipX;
        */
    }
    void Enabler()
    {
        //player.enabled = true;
        sprite.enabled = true;
    }

    void CreateShine()
    {
        Vector3 offset = shinePosition * (facingRight ? 1f : -1f);
        GameObject shine = Instantiate(shinePrefab, transform.position + offset, Quaternion.identity);
        shine.transform.localScale *= 0.5f;
        Destroy(shine, 1f);
    }

    IEnumerator ShootArrow()
    {
        float charge = 0f;
        //Normalized time where player shoots arrow: 0.625
        AnimatorStateInfo info = myAnim.GetCurrentAnimatorStateInfo(0);
        for(float i = 0f; i < info.length * 0.625f; i += Time.deltaTime) {yield return 0;}
        while (Input.GetKey(KeyCode.E))
        {
            myAnim.speed = 0f;
            if (charge < arrowChargeTime) charge += Time.deltaTime;
            if (charge > arrowChargeTime) { charge = arrowChargeTime; CreateShine(); }
            yield return 0;
        }
        myAnim.speed = 1f;

        GameObject shootingObject = (charge >= arrowChargeTime) ? chargedArrowPrefab : knifePrefab;

        //Shoot
        if (facingRight)
        {
            var knifeInstance = Instantiate(shootingObject, handEnd.position, Quaternion.identity);
            knifeInstance.GetComponent<Rigidbody2D>().velocity = (charge >= arrowChargeTime) ? Vector3.right * 12f : (handEnd.right * 7) + new Vector3(0, 1, 0);
        }
        else
        {
            var knifeInstance = Instantiate(shootingObject, handEnd.position, new Quaternion(shootingObject.transform.rotation.x, shootingObject.transform.rotation.y, shootingObject.transform.rotation.z, 1));
            knifeInstance.GetComponent<SpriteRenderer>().flipX = true;
            knifeInstance.GetComponent<Rigidbody2D>().velocity = (charge >= arrowChargeTime) ? Vector3.left * 12f : (-handEnd.right * 7) + new Vector3(0, 1, 0);
        }
    }


    IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(0.1f);

         if (facingRight)
                {
                    var knifeInstance = Instantiate(knifePrefab, handEnd.position, Quaternion.identity);
                    knifeInstance.GetComponent<Rigidbody2D>().velocity = (handEnd.right * 7) + new Vector3(0,1,0);
                }
                else
                {
                    var knifeInstance = Instantiate(knifePrefab, handEnd.position, new Quaternion(knifePrefab.transform.rotation.x, knifePrefab.transform.rotation.y, knifePrefab.transform.rotation.z, 1));
                    knifeInstance.GetComponent<SpriteRenderer>().flipX = true;
                    knifeInstance.GetComponent<Rigidbody2D>().velocity = (-handEnd.right * 7) + new Vector3(0, 1, 0); 
                }
    }
}
