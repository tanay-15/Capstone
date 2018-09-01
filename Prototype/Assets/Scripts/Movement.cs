using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float speed = 5;
    private Rigidbody player;
    private SpriteRenderer sprite;
    private Animator myAnim;
    bool facingRight;
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
            flip();
        }
        else if (hAxis<0 && facingRight)
        {
            flip();
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
