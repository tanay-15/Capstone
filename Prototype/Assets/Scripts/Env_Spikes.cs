﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// environmental spikes

public class Env_Spikes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<BoxCollider2D>().size = new Vector2(this.GetComponent<SpriteRenderer>().size.x, this.GetComponent<BoxCollider2D>().size.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // deal damage to anything that collides with it
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // modify player health
    }
}
