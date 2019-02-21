using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerStates : MonoBehaviour
{

// State
    public enum State
    {
        Default, InAir, Melee, Roll, RangedAim, ChargingArrow,Stomp, WallJump
    };
    [Header("State")]
    public State status;
    public State prevState;

// Values
    [Header("Values")]
    public float speed = 5.0f;
    public float jumpSpeed = 5.0f;
    public float forceX = 8.0f;
    public float forceY = 6.0f;
    public ArrowInfo shootingArrowInfo;
    int attackCounter = 0;

    // Bools
    [Header("Bools")]
    public bool facingRight = true;
    public bool grounded = false;
    public bool invulnerable = false;
    public bool movable = true;
    public bool onStateStart = true;
    bool resetState = false;
    bool wallSliding = false;

    // References

    [Header("References")]
    public Animator PlayerAnimator;

    public GameObject Human;
    public GameObject Demon;
    public GameObject StoneBlock;
    public Transform wallCheckpoint;
    public Transform wallCheckpoint1;
    public LayerMask wallLayerMask;
    GameObject GroundTrigger;
    [SerializeField]
    ParticleSystem DustParticles;
    Rigidbody2D Rb2d;
    private IEnumerator coroutine;
    Collider2D hit;
    Collider2D hitback;

    Object[] blocks;





    void Start()
    {

        foreach (Transform child in transform)
        {
            if (child.name == "Human")
                Human = child.gameObject;
            else if (child.name == "Demon")
                Demon = child.gameObject;
            else if (child.name == "GroundTrigger")
                GroundTrigger = child.gameObject;
        }

        PlayerAnimator = Human.GetComponent<Animator>();
        Rb2d = GetComponent<Rigidbody2D>();
        shootingArrowInfo.Initialize();
    }

    public void RestartInAir()
    {
        grounded = false;
        status = State.InAir;
        GroundTrigger.GetComponent<GroundTriggerScript>().grounded = false;
    }






    void Update()
    {

        //// Movement ////

        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        if (movable)
        {
            if (Mathf.Abs(hAxis) > 0.5f)
                Rb2d.velocity = new Vector3(Mathf.Sign(hAxis) * speed, Rb2d.velocity.y, 0);
                //Rb2d.velocity = new Vector2(forceX, forceY);
            else
                Rb2d.velocity = new Vector3(hAxis * speed, Rb2d.velocity.y, 0);
        }
        else
            Rb2d.velocity = new Vector3(Rb2d.velocity.x,Rb2d.velocity.y, 0);



        //// Flip ////

        if ((status != State.ChargingArrow || status!=State.InAir || status != State.WallJump)&& movable)
        {
            if (hAxis > 0 && !facingRight)
                flip();
            else if (hAxis < 0 && facingRight)
                flip();
        }


        //// Grounded ////

        if (Rb2d.velocity.y != 0)
            grounded = GroundTrigger.GetComponent<GroundTriggerScript>().grounded;
        else
            grounded = true;

        /////Wall Jump////

      


        //// State Switch ////

        //Initially changing to state
        if (prevState != status)
        {
            switch (status)
            {
                case State.ChargingArrow:
                    InitChargeArrow();
                    break;
            }
            prevState = status;
        }
        switch (status)
        {
            case State.Default:
                {
                    if (Mathf.Abs(hAxis) > 0.1f)
                        PlayerAnimator.Play("Run");
                    else
                        PlayerAnimator.Play("Idle");


                    if (Input.GetButtonDown("Jump")|| Input.GetButtonDown("PS4Jump"))
                    {
                        Rb2d.velocity = Vector3.up * jumpSpeed;
                        
                    }
                        

                    if (Input.GetButtonDown("Fire1"))
                        status = State.Melee;

                    if (Input.GetButtonDown("Fire2") && ChargedArrow.arrowCount < ChargedArrow.maxArrows)
                    {
                        if (FindObjectOfType<DemonTransformScript>() != null && !FindObjectOfType<DemonTransformScript>().DemonModeActive)
                            status = State.ChargingArrow;
                    }

                    if (Input.GetButtonDown("PS4CIRCLE"))
                    {
                        status = State.Roll;
                    }

                    if (grounded == false)
                        status = State.InAir;

                    if (grounded == true)
                        movable = true;

                    break;
                }
            case State.InAir:
                {
                    PlayerAnimator.Play("Jump");

                    if (vAxis < -0.5f && Input.GetButtonDown("PS4Jump"))
                        status = State.Stomp;

                        

                    if (grounded == true)
                        status = State.Default;

                    if (Input.GetButtonDown("Fire2") && ChargedArrow.arrowCount < ChargedArrow.maxArrows)
                    {
                        status = State.ChargingArrow;
                    }

                    hit = Physics2D.OverlapCircle(wallCheckpoint.position, 0.05f, wallLayerMask);
                    hitback = Physics2D.OverlapCircle(wallCheckpoint1.position, 0.05f, wallLayerMask); 
                    if ((hit && !grounded) || (hitback && !grounded))
                    {
                        if (hit != null)
                        {
                            status = State.WallJump;
                        }
                    }
                    if(hitback == null && !grounded)
                    {
                        movable = true;
                    }
                    break;
                }
            case State.Stomp:
                {
                    PlayerAnimator.Play("Stomp");
                    Rb2d.velocity = Vector3.down *2* jumpSpeed;
                    if (grounded == true)
                    {
                        Destroy(Instantiate(DustParticles.gameObject, GroundTrigger.transform.position, Quaternion.identity), 2f);
                        FindObjectOfType<CameraFollow>().ShakeCamera();
                        DustParticles.Play();

                        blocks = FindObjectsOfType<OrbitorObjectScript>();
                        if (GetComponent<DemonTransformScript>().DemonModeActive && blocks.Length < 7)
                        {
                            Instantiate(StoneBlock, GroundTrigger.transform.position, Quaternion.identity);
                            Instantiate(StoneBlock, GroundTrigger.transform.position, Quaternion.identity);
                            Instantiate(StoneBlock, GroundTrigger.transform.position, Quaternion.identity);
                        }


                        status = State.Default;
                    }

                    break;
                }
            case State.Melee:
                {
                    movable = false;

                    if (onStateStart)
                    {
                        // Melee Combo
                        if (attackCounter == 0)
                        {
                            PlayerAnimator.Play("MeleeAttack1", -1, 0);
                            attackCounter++;
                        }
                        else if (attackCounter == 1)
                        {
                            PlayerAnimator.Play("MeleeAttack2", -1, 0);
                            attackCounter = 0;
                        }
                        
                        coroutine = MeleeAttack();
                        StartCoroutine(coroutine);
                        onStateStart = false;
                        resetState = false;
                    }

                    // Reset

                    //if (Input.GetButtonDown("Fire1") && resetState)
                    //{
                    //    onStateStart = true;
                    //    StopCoroutine(coroutine);
                    //}

                    //transform.position = new Vector3(transform.position.x + 1 * Time.deltaTime * ((facingRight) ? 1 : -1), transform.position.y, transform.position.z);

                    break;
                }
            case State.Roll:
                {

                    if (onStateStart)
                    {
                        PlayerAnimator.Play("Roll");
                        StartCoroutine("Roll");
                        Physics2D.IgnoreLayerCollision(14,15);
                        onStateStart = false;
                    }
                        
                    movable = false;
                    invulnerable = true;

                    hit = Physics2D.OverlapCircle(wallCheckpoint.position, 0.05f, wallLayerMask);
                    if (hit == null)
                        transform.position = new Vector3(transform.position.x + 5*Time.deltaTime*((facingRight)?1:-1), transform.position.y, transform.position.z);

                    break;
                }

            case State.ChargingArrow:
                {
                    ChargeArrow();
                    break;
                }
            case State.WallJump:
                {
                    if(movable)
                    {
                        flip();
                        movable = false;
                    }
                    
                    
                    if (!Input.GetButton("Jump") && !Input.GetButton("PS4Jump"))
                    {
                        Rb2d.velocity = new Vector2(Rb2d.velocity.x, -0.8f);
                        hitback = Physics2D.OverlapCircle(wallCheckpoint1.position, 0.05f, wallLayerMask);
                        if (hitback == null)
                        {
                            status = State.InAir;
                            movable = true;
                        }
                        else if(hitback !=null && grounded)
                        {
                            status = State.Default;
                            movable = true;
                        }
                    }

                    else if (Input.GetButtonDown("Jump") || Input.GetButtonDown("PS4Jump"))
                    {
                        movable = false;
                        Rb2d.velocity = Vector2.zero;
                        if (facingRight)// && hAxis == 0f)
                        {
                            Rb2d.velocity = new Vector2(forceX , forceY);
                          
                            status = State.InAir;
    
                        }
                        else if(!facingRight)// && hAxis == 0f)
                        {
                            Rb2d.velocity = new Vector2(-forceX , forceY);
                     
                            status = State.InAir;                         
                        }
                    }

                    if(grounded == true)
                    {
                        movable = true;
                        status = State.Default;
                    }
                    break;
                }
        }
    }

// Helper Functions

    void InitChargeArrow()
    {
        //movable = false;
        shootingArrowInfo.Start();

        //TODO: Change to a charge arrow animation
        PlayerAnimator.Play("Idle");
    }

    void ChargeArrow()
    {
        //shootingArrowInfo.Move(Input.GetAxisRaw("Vertical"));
        shootingArrowInfo.MoveWithMouse();

        shootingArrowInfo.chargeTime += Time.deltaTime;
        if (shootingArrowInfo.IsFullyCharged && !shootingArrowInfo.chargeFlag)
        {
            shootingArrowInfo.chargeFlag = true;
            Instantiate(shootingArrowInfo.fullyChargeSparkPrefab, transform.position, Quaternion.identity);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            //Shoot an arrow
            Vector3 velocity;
            float chargeAmount = (0.2f + shootingArrowInfo.ChargeAmount * 0.8f);
            Vector3 position = Human.transform.position + (Vector3)shootingArrowInfo.GetShootingDirectionToMouse(transform.position, facingRight) * shootingArrowInfo.shootDistance;
            velocity = (Vector3)shootingArrowInfo.GetShootingDirectionToMouse(transform.position, facingRight) * shootingArrowInfo.shootSpeed * chargeAmount;
            GameObject arrow = Instantiate(shootingArrowInfo.arrowPrefab, position, Quaternion.identity);
            arrow.GetComponent<Rigidbody2D>().gravityScale = (shootingArrowInfo.IsFullyCharged) ? 0f : 1f;
            arrow.GetComponent<Rigidbody2D>().velocity = velocity;
            arrow.GetComponent<ChargedArrow>().fullyCharged = (shootingArrowInfo.IsFullyCharged);

            //Gravity scale formerly 0.4
            shootingArrowInfo.End();
            status = (grounded) ? State.Default : State.InAir;
            //movable = true;
        }
    }



    public void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        theScale = GameObject.Find("OrbitingSystem").transform.localScale;
        theScale.x *= -1;
        GameObject.Find("OrbitingSystem").transform.localScale = theScale;
    }

    IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(0.125f);
        resetState = true;
        yield return new WaitForSeconds(0.125f);

        movable = true;
        onStateStart = true;
        status = State.Default;
    }

    IEnumerator Roll()
    {
        yield return new WaitForSeconds(0.5f);

        invulnerable = false;
        movable = true;
        Physics2D.IgnoreLayerCollision(14, 15,false);
        onStateStart = true;
        status = State.Default;
    }

    //IEnumerator InAirTimeLimit()
    //{
    //    yield return new WaitForSeconds(4f);
    //    onStateStart = true;
    //    status = State.Default;
    //}
}



[System.Serializable]
public class ArrowInfo
{
    public GameObject arrowPrefab;
    public GameObject fullyChargeSparkPrefab;
    [SerializeField] GameObject reticle;
    public Vector3 shinePosition = new Vector3(0.4f, 0f, 0f);
    private Vector3 reticlePosition;
    public bool chargeFlag;    //False, set to true when fully charged
    public float reticleDistance = 2f;
    public float minChargeTime = 0.6f;
    public float dReticleAngle = 0.02f;
    public float shootDistance = 1f;
    public float shootSpeed = 12f;

    [System.NonSerialized] public float chargeTime;
    [System.NonSerialized] public float reticleHeight;

    public bool IsFullyCharged
    {
        get
        {
            return chargeTime > minChargeTime;
        }
    }

    public float ChargeAmount
    {
        get
        {
            return Mathf.Min(chargeTime / minChargeTime, 1f);
        }
    }

    public Vector2 GetShootingDirection(bool facingRight)
    {
        Vector2 shootingDirection = reticlePosition.normalized;
        shootingDirection.x *= (facingRight) ? 1f : -1f;
        return shootingDirection;
    }

    public Vector2 GetShootingDirectionToMouse(Vector3 position, bool facingRight)
    {
        Vector2 shootingDirection = Input.mousePosition;
        if (Levitation.sharedInstance != null)
            shootingDirection = (Levitation.sharedInstance.grabPosition - position).normalized;
        return shootingDirection;
    }

    public void Initialize()
    {
        reticle.SetActive(false);
        reticlePosition = new Vector2(reticleDistance, 0f);
        chargeFlag = false;
    }

    public void Start()
    {
        reticle.SetActive(true);
    }

    public void Move(float axis)
    {
        reticleHeight += axis * dReticleAngle;
        reticleHeight = Mathf.Clamp(reticleHeight, -1f, 1f);

        reticlePosition.x = Mathf.Cos(reticleHeight * Mathf.PI / 2);
        reticlePosition.y = Mathf.Sin(reticleHeight * Mathf.PI / 2);
        reticlePosition.Normalize();
        reticlePosition *= reticleDistance;
        reticle.transform.localPosition = (Vector3)reticlePosition;
    }

    public void MoveWithMouse()
    {
        if (Levitation.sharedInstance != null)
            reticle.transform.position = Levitation.sharedInstance.grabPosition;
        else
            reticle.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
    }

    public void End()
    {
        chargeTime = 0f;
        reticle.SetActive(false);
        reticlePosition = new Vector2(reticleDistance, 0f);
        reticleHeight = 0f;
        chargeFlag = false;
    }
}