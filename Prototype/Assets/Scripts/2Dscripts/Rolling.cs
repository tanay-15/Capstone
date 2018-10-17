using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolling : MonoBehaviour {
    Rigidbody2D player;
    private Movement2D playerMovement;
    // Use this for initialization
    void Start () {
        player = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<Movement2D>();

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Tab))
        {
            // player.enabled = false;
            // sprite.enabled = false;
            //sprite.enabled = false;

            if (!playerMovement.facingRight)
            {
                Vector3 targetPos = transform.position + transform.right * -0.25f;
                player.transform.position = targetPos;
                //Add rolling animation left
            }
            else
            {
                Vector3 targetPos = transform.position + transform.right * 0.25f;
                player.transform.position = targetPos;
                //Add rolling animation right
            }
        }

    }
}
