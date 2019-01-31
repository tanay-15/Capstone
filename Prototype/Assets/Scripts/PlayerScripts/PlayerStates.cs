﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArrowInfo
{
    public GameObject arrowPrefab;
    [SerializeField] GameObject reticle;
    public Vector3 shinePosition = new Vector3(0.4f, 0f, 0f);
    private Vector3 reticlePosition;
    public float reticleDistance = 2f;
    public float minChargeTime = 0.6f;
    public float dReticleAngle = 0.02f;
    public float shootDistance = 1f;
    public float shootSpeed = 12f;

    [System.NonSerialized] public float chargeTime;
    [System.NonSerialized] public float reticleHeight;

    public bool IsCharged
    {
        get
        {
            return chargeTime > minChargeTime;
        }
    }

    public Vector2 GetShootingDirection(bool facingRight)
    {
        Vector2 shootingDirection = reticlePosition.normalized;
        shootingDirection.x *= (facingRight) ? 1f : -1f;
        return shootingDirection;
    }
    
    public void Initialize()
    {
        reticle.SetActive(false);
        reticlePosition = new Vector2(reticleDistance, 0f);
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

    public void End()
    {
        chargeTime = 0f;
        reticle.SetActive(false);
        reticlePosition = new Vector2(reticleDistance, 0f);
        reticleHeight = 0f;
    }
}

public class PlayerStates : MonoBehaviour
{

// State
    public enum State
    {
        Default, InAir, Melee, Roll, RangedAim, ChargingArrow,
    };
    [Header("State")]
    public State status;
    public State prevState;

// Values
    [Header("Values")]
    public ArrowInfo shootingArrowInfo;
    public float speed = 5.0f;
    public float jumpSpeed = 5.0f;

// Bools
    [Header("Bools")]
    public bool facingRight = true;
    public bool grounded = false;
    public bool invulnerable = false;
    public bool movable = true;

    
// References

    Animator PlayerAnimator;

    GameObject Human;
    GameObject Demon;
    GameObject GroundTrigger;

    Rigidbody2D Rb2d;


    
   





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






    void Update()
    {

        //// Movement ////

        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        if (movable)
        {
            if (Mathf.Abs(hAxis) > 0.5f)
                Rb2d.velocity = new Vector3(Mathf.Sign(hAxis) * speed, Rb2d.velocity.y, 0);
            else
                Rb2d.velocity = new Vector3(hAxis * speed, Rb2d.velocity.y, 0);
        }
        else
            Rb2d.velocity = new Vector3(0,Rb2d.velocity.y, 0);



        //// Flip ////

        if (hAxis > 0 && !facingRight)
            flip();
        else if (hAxis < 0 && facingRight)
            flip();


        //// Grounded ////

        grounded = GroundTrigger.GetComponent<GroundTriggerScript>().grounded;

        
        
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
                        Rb2d.velocity = Vector3.up * jumpSpeed;

                    if (Input.GetButtonDown("Fire1"))
                        status = State.Melee;

                    if (Input.GetButtonDown("Fire2"))
                    {
                        status = State.ChargingArrow;
                    }

                    if (grounded == false)
                        status = State.InAir;

                    break;
                }
            case State.InAir:
                {
                    PlayerAnimator.Play("Jump");


                    if (grounded == true)
                        status = State.Default;

                    break;
                }
            case State.Melee:
                {
                    PlayerAnimator.Play("MeleeAttack");

                    movable = false;
                    StartCoroutine("MeleeAttack");

                    break;
                }
            case State.Roll:
                {
                    break;
                }

            case State.ChargingArrow:
                {
                    ChargeArrow();
                    break;
                }
        }
    }

// Helper Functions

    void InitChargeArrow()
    {
        movable = false;
        shootingArrowInfo.Start();

        //TODO: Change to a charge arrow animation
        PlayerAnimator.Play("Idle");
    }

    void ChargeArrow()
    {
        shootingArrowInfo.Move(Input.GetAxisRaw("Vertical"));
        shootingArrowInfo.chargeTime += Time.deltaTime;
        if (Input.GetButtonUp("Fire2"))
        {
            //Shoot an arrow
            if (shootingArrowInfo.IsCharged)
            {
                Vector3 velocity;
                Vector3 position = Human.transform.position + (Vector3)shootingArrowInfo.GetShootingDirection(facingRight) * shootingArrowInfo.shootDistance;
                velocity = (Vector3)shootingArrowInfo.GetShootingDirection(facingRight) * shootingArrowInfo.shootSpeed;
                GameObject arrow = Instantiate(shootingArrowInfo.arrowPrefab, position, Quaternion.identity);
                arrow.GetComponent<Rigidbody2D>().velocity = velocity;
            }
            shootingArrowInfo.End();
            status = (grounded) ? State.Default : State.InAir;
            movable = true;
        }
    }



    public void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(0.5f);
        movable = true;
        status = State.Default;
    }
}
