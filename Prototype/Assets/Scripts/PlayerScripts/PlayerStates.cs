using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{

// State
    public enum State
    {
        Default, InAir, Melee, Roll, RangedAim
    };
    [Header("State")]
    public State status;

// Values
    [Header("Values")]
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

        switch (status)
        {
            case State.Default:
                {
                    if (Mathf.Abs(hAxis) > 0.1f)
                        PlayerAnimator.Play("Run");
                    else
                        PlayerAnimator.Play("Idle");


                    if (Input.GetButtonDown("Jump"))
                        Rb2d.velocity = Vector3.up * jumpSpeed;

                    if (Input.GetButtonDown("Fire1"))
                        status = State.Melee;

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
        }
    }





// Helper Functions

    public void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(0.2f);
        movable = true;
        status = State.Default;
    }
}
