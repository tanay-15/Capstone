using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class PlayerStates : MonoBehaviour
{

    // State
    public enum State
    {
        Default,
        InAir,
        Melee,
        Roll,
        RangedAim,
        ChargingArrow,
        Stomp,
        WallJump,
        WallSlide,
        Panning,
        WallCrawl,
        PlayerSwitch,
        Sleeping,
        WakingUp,
        FixedRun
    };
    public bool controlsEnabled = true;
    [Header("State")]
    public State status;
    public State prevState;

    // Values
    [Header("Values")]
    public float speed = 5.0f;
    public float jumpSpeed = 5.0f;
    public float forceX = 8.0f;
    public float forceY = 6.0f;
    public float slideSpeed = 1.8f;
    public ArrowInfo shootingArrowInfo;
    public int attackCounter = 0;
    int groundCount = 0;
    float cameraPanSensitivity = 0.08f;
    float maxPanRangeX = 9f;
    float maxUpperPanRange = 3f;
    float maxLowerPanRange = 7f;
    float minAnalogJumpSpeed = 3f;
    Vector2 prevMousePosition;

    // Bools
    [Header("Bools")]
    public bool facingRight = true;
    public bool grounded = false;
    public bool invulnerable = false;
    public bool movable = true;
    public bool onStateStart = true;
    public bool wallJump = false;
    public bool shootingEnabled = true;
    bool resetState = false;
    bool wallSliding = false;

    // References
    [Header("References")]
    public Animator PlayerAnimator;
    AudioManager audioManager;
    public Transform cameraFocus;
    public GameObject Human;
    public GameObject Demon;
    public GameObject bowAndArrow;
    public GameObject StoneBlock;
    public GameObject ImpactAnim;
    public GameObject RockPiece;
    public Transform wallCheckpoint;
    public Transform wallCheckpoint1;
    public Transform JumpCheckPoint;
    public LayerMask wallLayerMask;
    GameObject GroundTrigger;
    GameObject Smoke;
    GameObject SmokeOverlay;
    //GameObject AttackTrigger;
    [SerializeField]
    ParticleSystem DustParticles;
    Rigidbody2D Rb2d;
    private IEnumerator coroutine;
    Collider2D hit;
    Collider2D hitback;
    Collider2D jumpHit;
    public static bool playerSwitch = false;

    GameObject[] blocks = { null,null,null };

    //Bow and arrow objects
    public Transform bow;
    public Transform frontBicep;
    public Transform backBicep;
    float initialFrontBicepRotation = 25.82f;
    //float initialBackBicepRotation = 109.113f;





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
            else if (child.name == "Smoke")
                Smoke = child.gameObject;
            else if (child.name == "SmokeOverlay")
                SmokeOverlay = child.gameObject;
        }

        PlayerAnimator = Human.GetComponent<Animator>();
        Rb2d = GetComponent<Rigidbody2D>();
        shootingArrowInfo.Initialize();
        //audioManager = FindObjectOfType<AudioManager>();
        
        DustParticles = Smoke.GetComponent<ParticleSystem>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        if (controlsEnabled)
            status = State.Default;
        else
        {
            status = State.Sleeping;
            PlayerAnimator.Play("Sleep");
        }
        if (!shootingEnabled)
            bowAndArrow.SetActive(false);
    }


    private void LateUpdate()
    {
        //Rotate the arm for aiming arrows
        if (status == State.ChargingArrow)
        {
            //TODO: Aiming arrow should use an animation, not programmatically rotate the arms
            float right = facingRight ? 0f : 180f;
            float rotation = Mathf.Atan2(shootingArrowInfo.GetShootingDirectionToMouse(frontBicep.position, facingRight).y, shootingArrowInfo.GetShootingDirectionToMouse(frontBicep.position, facingRight).x) * 180 / Mathf.PI;
            frontBicep.rotation = Quaternion.Euler(0f, 0f, rotation + ((facingRight) ? -initialFrontBicepRotation : initialFrontBicepRotation) + right);
            //backBicep.rotation = Quaternion.Euler(0f, 0f, rotation + ((facingRight) ? -initialBackBicepRotation : initialBackBicepRotation) + right);
        }
    }

    public void WakeUp()
    {
        status = State.WakingUp;
        PlayerAnimator.Play("GetUp");
    }

    public void SetFixedRun()
    {
        status = State.FixedRun;
    }

    public void EnableShooting(bool enabled)
    {
        shootingEnabled = enabled;
        bowAndArrow.SetActive(enabled);
    }


    void Update()
    {
        //if (!controlsEnabled)
        //{
        //    //PlayerAnimator.Play("Sleep");
        //    return;
        //}
        //// Movement ////

        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        if (movable && controlsEnabled)
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

        if ((status != State.ChargingArrow || status!=State.InAir || status != State.WallJump)&& movable && controlsEnabled)
        {
            if (hAxis > 0 && !facingRight && Time.timeScale > 0f)
                flip();
            else if (hAxis < 0 && facingRight && Time.timeScale > 0f)
                flip();
        }


        //// Grounded ////

        if (Rb2d.velocity.y != 0)
            grounded = GroundTrigger.GetComponent<GroundTriggerScript>().grounded;
        else
            grounded = true;

        //Audio//

        //if(grounded && Rb2d.velocity.magnitude > 2f)
        //{
        //   audioManager.Play("FootStep");
        //}

      


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
                    groundCount = 0;

                    if (Time.timeScale > 0f){
                        if (Mathf.Abs(hAxis) > 0.1f)
                        {
                            PlayerAnimator.Play("Run");
                            audioManager.Play("FootStep");
                        }

                        else
                        {
                            PlayerAnimator.Play("Idle");
                        }
                    }

                    if (playerSwitch == true)
                    {
                        status = State.PlayerSwitch;
                    }

                    if (Time.timeScale > 0f && (Input.GetButtonDown("Jump") || Input.GetButtonDown("PS4Jump")))
                    {
                        Rb2d.velocity = Vector3.up * jumpSpeed;
                    }
                    //Analog jumping
                    if (Time.timeScale > 0f && Rb2d.velocity.y > minAnalogJumpSpeed && (Input.GetButtonUp("Jump") || Input.GetButtonUp("PS4Jump")))
                    {
                        Rb2d.velocity = new Vector2(Rb2d.velocity.x, minAnalogJumpSpeed);
                    }


                    if (Time.timeScale > 0f && Input.GetButtonDown("Fire1"))
                        status = State.Melee;

                    if (Time.timeScale > 0f && Input.GetButtonDown("Fire2") && shootingEnabled)
                    {
                        if (ArrowCounter.sharedInstance.ArrowCount > 0)
                        {
                            if (FindObjectOfType<DemonTransformScript>() != null && !FindObjectOfType<DemonTransformScript>().DemonModeActive)
                            {
                                status = State.ChargingArrow;
                            }
                        }
                        else
                        {
                            ArrowCounter.sharedInstance.Blink();
                        }
                    }

                    if (Time.timeScale > 0f && (Input.GetButtonDown("PS4CIRCLE") || (Input.GetAxis("Mouse ScrollWheel") > 0f)))
                    {
                        status = State.Roll;
                    }

                    if (grounded == false)
                        status = State.InAir;

                    if (grounded == true)
                        movable = true;

                    if (Time.timeScale > 0f && (Input.GetMouseButtonDown(2) || Input.GetButtonDown("LeftTrigger1")))
                    {
                        status = State.Panning;
                        prevMousePosition = (Vector2)Input.mousePosition;
                    }

                    break;
                }
            case State.InAir:
                {
                    //Analog jumping
                    if (Time.timeScale > 0f && Rb2d.velocity.y > minAnalogJumpSpeed && (Input.GetButtonUp("Jump") || Input.GetButtonUp("PS4Jump")))
                    {
                        Rb2d.velocity = new Vector2(Rb2d.velocity.x, minAnalogJumpSpeed);
                    }
                    //PlayerAnimator.Play("WallJump");
                    PlayerAnimator.Play("Jump");
                    //audioManager.Play("Jump");

                    if (Time.timeScale > 0f && vAxis < -0.5f && (Input.GetButtonDown("Jump") || Input.GetButtonDown("PS4Jump")))
                    {
                        Physics2D.IgnoreLayerCollision(14, 15);
                        status = State.Stomp;
                    }

                    if (Time.timeScale > 0f && Input.GetButtonDown("Fire1"))
                        status = State.Melee;


                    if (grounded == true)
                        status = State.Default;

                    if (Time.timeScale > 0 && Input.GetButtonDown("Fire2") && shootingEnabled)
                    {
                        if (ArrowCounter.sharedInstance.ArrowCount > 0)
                        {
                            if (FindObjectOfType<DemonTransformScript>() != null && !FindObjectOfType<DemonTransformScript>().DemonModeActive)
                            {
                                status = State.ChargingArrow;
                            }
                        }
                        else
                        {
                            ArrowCounter.sharedInstance.Blink();
                        }
                    }

                    hit = Physics2D.OverlapCircle(wallCheckpoint.position, 0.05f, wallLayerMask);
                    hitback = Physics2D.OverlapCircle(wallCheckpoint1.position, 0.05f, wallLayerMask);
                    jumpHit = Physics2D.OverlapCircle(JumpCheckPoint.position, 0.05f, wallLayerMask);
                    if ((hit && !grounded) || (hitback && !grounded))
                    {
                        if (hit != null && jumpHit!= null)
                        {
                            status = State.WallJump;
                            movable = true;
                        }
                    }
                    if (hitback == null && !grounded)
                    {
                        movable = true;
                    }
                    break;
                }
            case State.Stomp:
                {

                    if (groundCount < 1)
                    {
                        PlayerAnimator.Play("Jump");
                        Rb2d.velocity = Vector3.down * 2 * jumpSpeed;
                        if (grounded == true && onStateStart == true)
                        {
                            movable = false;
                            //Destroy(Instantiate(DustParticles.gameObject, GroundTrigger.transform.position, Quaternion.identity), 2f);

                            FindObjectOfType<CameraFollow>().ShakeCamera();
                            //DustParticles.Play();
                            //blocks = FindObjectsOfType<OrbitorObjectScript>();
                            if (GetComponent<DemonTransformScript>().DemonModeActive)
                            {
                                audioManager.Play("DStomp");
                                for (int i = 0; i < 3; i++)
                                {
                                    if (blocks[i] != null)
                                    {
                                        blocks[i].SendMessageUpwards("Explode", 8);
                                    }
                                    blocks[i] = Instantiate(StoneBlock, GroundTrigger.transform.position, Quaternion.identity);
                                }
                            }
                            else
                            {
                                audioManager.Play("Stomp");
                            }

  

                            onStateStart = false;
                            Physics2D.IgnoreLayerCollision(14, 15,false);
                            StartCoroutine("Stomp");

                            //status = State.Default;
                            groundCount++;
                        }
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
                            audioManager.Play("Punch");
                            attackCounter++;
                        }
                        else if (attackCounter == 1)
                        {
                            PlayerAnimator.Play("MeleeAttack2", -1, 0);
                            audioManager.Play("Punch");
                            attackCounter++;
                        }
                        else if (attackCounter == 2)
                        {
                            PlayerAnimator.Play("MeleeAttack3", -1, 0);
                            audioManager.Play("Punch");
                            attackCounter = 0;
                        }

                        coroutine = MeleeAttack();
                        StartCoroutine(coroutine);
                        onStateStart = false;
                        resetState = false;
                    }


                    if (Time.timeScale > 0f && (Input.GetButtonDown("PS4CIRCLE") || (Input.GetAxis("Mouse ScrollWheel") > 0f)))
                    {
                        movable = true;
                        onStateStart = true;
                        //AttackTrigger.GetComponent<BoxCollider2D>().enabled = false;
                        StopCoroutine(coroutine);
                        status = State.Roll;
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
                        Physics2D.IgnoreLayerCollision(14, 15);
                        onStateStart = false;
                    }

                    movable = false;
                    invulnerable = true;

                    hit = Physics2D.OverlapCircle(wallCheckpoint.position, 0.05f, wallLayerMask);
                    if (hit == null)
                        transform.position = new Vector3(transform.position.x + 5 * Time.deltaTime * ((facingRight) ? 1 : -1), transform.position.y, transform.position.z);

                    break;
                }

            case State.ChargingArrow:
                {
                    ChargeArrow();
                    break;
                }
            case State.WallJump:
                {

                    /*
                    if (movable)
                    {
                        flip();
                        movable = false;

                    }

                    if (!Input.GetButton("Jump") && !Input.GetButton("PS4Jump"))
                    {
                        hitback = Physics2D.OverlapCircle(wallCheckpoint1.position, 0.05f, wallLayerMask);
                        if (hitback != null && !grounded)
                        {
                            PlayerAnimator.Play("WallSlide");
                            Rb2d.velocity = new Vector2(Rb2d.velocity.x, -0.8f);
                        }

                        else if (hitback == null)
                        {
                            status = State.InAir;
                            movable = true;
                        }
                        else if (hitback != null && grounded)
                        {
                            status = State.Default;
                            movable = true;
                        }
                    }

                    else if (Input.GetButtonDown("Jump") || Input.GetButtonDown("PS4Jump"))
                    {
                        movable = false;
                        Rb2d.velocity = Vector2.zero;
                        StartCoroutine("WallJump");
                        //status = State.InAir;
                    }

                    if(grounded == true)
                    {
                        movable = true;
                        status = State.Default;
                    }
                    */

                    if (movable)
                    {
                        if (FindObjectOfType<DemonTransformScript>().DemonModeActive == false)
                        {
                            flip();
                        }
                        movable = false;
                    }

                    if (!Input.GetButton("Jump") && !Input.GetButton("PS4Jump"))
                    {
                        hitback = Physics2D.OverlapCircle(wallCheckpoint1.position, 0.05f, wallLayerMask);
                        if (FindObjectOfType<DemonTransformScript>().DemonModeActive == false)
                        {
                            if ((hitback != null && !grounded)) //|| movable == false)
                            {
                                status = State.WallSlide;
                            }
                            else if (hitback == null && !grounded)
                            {
                                //status = State.Default;
                                PlayerAnimator.Play("Jump");
                            }
                        }
                        else
                        {
                            if(hit != null)
                            status = State.WallSlide;
                            else if (hitback == null)
                            {
                                //status = State.Default;
                                PlayerAnimator.Play("Jump");
                            }
                        }

                        if ((hitback != null && grounded) || grounded)
                        {
                            status = State.Default;
                            movable = true;
                        }

                    }
                    break;
                }

            case State.WallSlide:
                {
                    hitback = Physics2D.OverlapCircle(wallCheckpoint1.position, 0.05f, wallLayerMask);
                    if (FindObjectOfType<DemonTransformScript>().DemonModeActive == false)
                    {
                        if (hitback != null)
                        {
                            PlayerAnimator.Play("WallSlide");
                            Rb2d.velocity = new Vector2(0, -slideSpeed);
                        }
                        else
                        {
                            status = State.InAir;
                        }

                        if (Time.timeScale > 0f && (Input.GetButtonDown("Jump") || Input.GetButtonDown("PS4Jump")) && hitback)
                        {
                            movable = false;
                            Rb2d.velocity = Vector2.zero;
                            status = State.WallJump;
                            StartCoroutine("WallJump");
                            audioManager.Play("WJump");
                            //status = State.InAir;
                        }

                    }
                    else
                    {
                        hit = Physics2D.OverlapCircle(wallCheckpoint.position, 0.05f, wallLayerMask);
                        if (hit != null)
                        {
                            PlayerAnimator.Play("WallSlide");
                            Rb2d.velocity = new Vector2(0, -slideSpeed);
                        }
                        if (Time.timeScale > 0f && ((Input.GetButtonDown("Jump") || Input.GetButtonDown("PS4Jump")) && (Mathf.Abs(hAxis) > 0.3f)))
                        {
                            movable = false;
                            Rb2d.velocity = Vector2.zero;
                            status = State.WallJump;
                            StartCoroutine("WallJump");
                            //status = State.InAir;
                        }
                        else if(Input.GetButtonDown("Jump") || Input.GetButtonDown("PS4Jump") && hit!=null)
                        {
                            movable = false;
                            Rb2d.velocity = Vector2.zero;
                            status = State.WallCrawl;
                            StartCoroutine("WallCrawl");
                            audioManager.Play("WCrawl");
                        }
                    }

                    if (grounded == true)
                    {
                        movable = true;
                        status = State.Default;
                    }
                    else if (hit == null && hitback == null)
                    {
                        status = State.InAir;
                    }
                  break;
                }

            case State.WallCrawl:
                {
                   // Rb2d.velocity = Vector2.zero;

                    break;
                }

            case State.Panning:
                if (Input.GetMouseButtonUp(2) || Input.GetButtonUp("LeftTrigger1"))
                {
                    status = (grounded) ? State.Default : State.InAir;
                    cameraFocus.localPosition = Vector3.zero;
                    break;
                }
                Pan();
                break;

            case State.PlayerSwitch:
                StartCoroutine("PlayerSwitch");
                break;

                //Shouldn't play an animation constantly
            case State.Sleeping:
                //PlayerAnimator.Play("Sleep");
                break;

            case State.WakingUp:
                //PlayerAnimator.Play("GetUp");
                if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
                    SetFixedRun();
                }
                break;

            case State.FixedRun:
                movable = false;
                PlayerAnimator.Play("Run");
                audioManager.Play("FootStep");
                Rb2d.velocity = new Vector2(speed, Rb2d.velocity.y);
                break;
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
        PlayerAnimator.Play("Bow");
        //shootingArrowInfo.Move(Input.GetAxisRaw("Vertical"));
        if (Time.timeScale > 0f)
            shootingArrowInfo.MoveWithMouse();

        shootingArrowInfo.chargeTime += Time.deltaTime;
        if (shootingArrowInfo.IsFullyCharged && !shootingArrowInfo.chargeFlag)
        {
            //PlayerAnimator.Play("ArrowCharged");
            shootingArrowInfo.chargeFlag = true;
            Instantiate(shootingArrowInfo.fullyChargeSparkPrefab, wallCheckpoint.transform.position, Quaternion.identity);
            
        }
            
        if (!Input.GetButton("Fire2") && Time.timeScale > 0f)
        {

            //Shoot an arrow
            Vector3 velocity;
            float chargeAmount = (0.2f + shootingArrowInfo.ChargeAmount * 0.8f);
            Vector3 position = Human.transform.position + (Vector3)shootingArrowInfo.GetShootingDirectionToMouse(transform.position, facingRight) * shootingArrowInfo.shootDistance;
            velocity = (Vector3)shootingArrowInfo.GetShootingDirectionToMouse(transform.position, facingRight) * shootingArrowInfo.shootSpeed * chargeAmount;
            GameObject arrow = Instantiate(shootingArrowInfo.arrowPrefab, position, Quaternion.identity);
            
            ArrowCounter.sharedInstance.AddArrowCount(-1);
            arrow.GetComponent<Rigidbody2D>().gravityScale = (shootingArrowInfo.IsFullyCharged) ? 0f : 1f;
            arrow.GetComponent<Rigidbody2D>().velocity = velocity;
            arrow.GetComponent<ChargedArrow>().fullyCharged = (shootingArrowInfo.IsFullyCharged);

            //Gravity scale formerly 0.4
            audioManager.Play("ShootArrow");
            shootingArrowInfo.End();
            status = (grounded) ? State.Default : State.InAir;
            //movable = true;
        }
    }

    void Pan()
    {
        Vector2 newPos = (Vector2)cameraFocus.localPosition;
        Vector2 dPos = ((Vector2)Input.mousePosition - prevMousePosition) * cameraPanSensitivity;
        Vector2 joystick = new Vector2(Input.GetAxis("RHorizontal"), Input.GetAxis("RVertical")) * cameraPanSensitivity * 3f;   //Multiply by 3f to increase joystick pan sensitivity
        dPos += joystick;
        dPos.x *= transform.localScale.x;
        newPos += dPos;
        newPos.x = Mathf.Clamp(newPos.x, -maxPanRangeX, maxPanRangeX);
        newPos.y = Mathf.Clamp(newPos.y, -maxLowerPanRange, maxUpperPanRange);
        cameraFocus.localPosition = newPos;
        prevMousePosition = (Vector2)Input.mousePosition;
        
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

        Vector3 newFocusPos = cameraFocus.localPosition;
        newFocusPos.x *= -1;
        cameraFocus.localPosition = newFocusPos;
    }



    IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(0.25f);

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

    IEnumerator Stomp()
    {
        PlayerAnimator.Play("Stomp");

        yield return new WaitForSeconds(0.05f);

        Smoke.GetComponent<ParticleSystem>().Play();
        SmokeOverlay.GetComponent<ParticleSystem>().Play();
        Vector2 center = Smoke.transform.position;
        var RockPiece1 = Instantiate(RockPiece, center + new Vector2(0.3f, 0.6f), Quaternion.identity);
        var RockPiece2 = Instantiate(RockPiece, center + new Vector2(-0.3f, 0.6f), Quaternion.identity);

        Vector2 direction = new Vector2(RockPiece1.gameObject.transform.position.x, RockPiece1.gameObject.transform.position.y) - center;
        direction.Normalize();
        RockPiece1.GetComponent<Rigidbody2D>().AddForce(direction * Random.Range(5, 7), ForceMode2D.Impulse);
        

        direction = new Vector2(RockPiece2.gameObject.transform.position.x, RockPiece2.gameObject.transform.position.y) - center;
        direction.Normalize();
        RockPiece2.GetComponent<Rigidbody2D>().AddForce(direction * Random.Range(5, 7), ForceMode2D.Impulse);

        Destroy(RockPiece1, 1.5f);
        Destroy(RockPiece2, 1.5f);

        yield return new WaitForSeconds(0.45f);

        onStateStart = true;
        status = State.Default;
    }


    IEnumerator PlayerSwitch()
    {
        if (FindObjectOfType<DemonTransformScript>().DemonModeActive == true)
            PlayerAnimator.Play("NormalSwitch");
        else
            PlayerAnimator.Play("PlayerSwitch");
        yield return new WaitForSeconds(1f);
        status = State.Default;
    }

    IEnumerator WallJump()
    {
        PlayerAnimator.Play("WallJump");
        if (FindObjectOfType<DemonTransformScript>().DemonModeActive == true)
            flip();
            if (facingRight)// && hAxis == 0f)
        {
            Rb2d.velocity = new Vector2(forceX, forceY);

        }
        else if (!facingRight)// && hAxis == 0f)
        {
            Rb2d.velocity = new Vector2(-forceX, forceY);
        }
        if(FindObjectOfType<DemonTransformScript>().DemonModeActive==false)
        yield return new WaitForSeconds(0.5f);
        status = State.InAir;

    }


    //Demon Wall Crawl
    IEnumerator WallCrawl()
    {
        //Debug.Log("Inside Crawl coroutine");
        PlayerAnimator.Play("Crawl");
        //transform.position = new Vector3(transform.position.x - 20 * Time.deltaTime * ((facingRight) ? 1 : -1), transform.position.y + 50 * Time.deltaTime, transform.position.z);
        Rb2d.velocity = Vector2.zero;
        float tempForceX = (((facingRight) ? 1.0f : -1.0f)* forceX);
        Rb2d.velocity = new Vector2(-tempForceX, forceY*2);
        yield return new WaitForSeconds(0.1f);
        Rb2d.velocity = Vector2.zero;
        //transform.position = new Vector3(transform.position.x + 23 * Time.deltaTime * ((facingRight) ? 1 : -1), transform.position.y + 50 * Time.deltaTime, transform.position.z);
        Rb2d.velocity = new Vector2(tempForceX*1.5f, forceY*2);
        yield return new WaitForSeconds(0.1f);
        // transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Rb2d.velocity = Vector2.zero;
        status = State.WallSlide;
    }

        public void RestartInAir()
    {
        grounded = false;
        status = State.InAir;
        GroundTrigger.GetComponent<GroundTriggerScript>().grounded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "AttackTrigger")
        {
            gameObject.SendMessage("GetHit", -10f);
            var impact = Instantiate(ImpactAnim, transform.position, Quaternion.identity);
            impact.gameObject.SetActive(true);
        }
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