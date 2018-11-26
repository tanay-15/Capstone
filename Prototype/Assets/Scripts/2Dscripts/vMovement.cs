using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vMovement : MonoBehaviour {
    private Rigidbody2D player;
    public float speed = 5;
    // Use this for initialization
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float vAxis = Input.GetAxis("Vertical");
        if (vAxis > 0.5f)
            player.velocity = new Vector3(player.velocity.x, (Mathf.Sign(vAxis) * speed), 0);
        else
            player.velocity = new Vector3(player.velocity.x, vAxis * speed, 0);
    }
}
