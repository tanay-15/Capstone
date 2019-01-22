using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
// Values
    public float speed = 5.0f;

// Bools
    [Header("Bools")]
    public bool facingRight = true;
    public bool grounded = false;
    public bool invulnerable = false;
    public bool immovable = false;

    
// References

    Animator PlayerAnimator;

    GameObject Human;
    GameObject Demon;

    Rigidbody2D Rb2d;



    enum State
    {
        Default,InAir
    };

    State status;





    void Start()
    {

        foreach (Transform child in transform)
        {
            if (child.name == "Human")
                Human = child.gameObject;
            else if (child.name == "Demon")
                Demon = child.gameObject;
        }

        PlayerAnimator = Human.GetComponent<Animator>();
        Rb2d = GetComponent<Rigidbody2D>();
    }






    void Update()
    {

        //// Movement ////

        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        //PlayerAnimator.SetFloat("Speed", Mathf.Abs(hAxis));

        if (hAxis > 0.5f)
            Rb2d.velocity = new Vector3(Mathf.Sign(hAxis) * speed, Rb2d.velocity.y, 0);
        else
            Rb2d.velocity = new Vector3(hAxis * speed, Rb2d.velocity.y, 0);

    }
}
