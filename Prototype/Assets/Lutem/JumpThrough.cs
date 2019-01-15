using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThrough : MonoBehaviour {

    BoxCollider2D collide;

    GameObject player;

	// Use this for initialization
	void Start () {
        collide = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(player.gameObject.transform.position.y < this.transform.position.y)
        {
            Physics2D.IgnoreCollision(collide, player.GetComponent<CapsuleCollider2D>());
        }
        else
        {
            Physics2D.IgnoreCollision(collide, player.GetComponent<CapsuleCollider2D>(), false);
        }
	}


}
