using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallJump : MonoBehaviour
{
    private Movement playerMovement;
    private Grounded ground;
    // Use this for initialization
    void Start()
    {
        playerMovement = GetComponent<Movement>();
        ground = GetComponent<Grounded>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && ground.grounded == false)
        {

        }

    }
    public void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
