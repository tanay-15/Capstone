﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTriggerScript : MonoBehaviour
{
    public bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10 || collision.gameObject.layer == 11 || collision.gameObject.layer == 18)
            grounded = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 9 || collision.gameObject.layer == 10 || collision.gameObject.layer == 11 || collision.gameObject.layer == 18) && !collision.gameObject.GetComponent<FallingPlatform>())
            grounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10 || collision.gameObject.layer == 11 || collision.gameObject.layer == 18)
            grounded = false;


        if (collision.gameObject.GetComponent<FallingPlatform>())
        {
            grounded = false;
        }
    }
}
