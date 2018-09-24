using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded2D : MonoBehaviour {

    public bool grounded;

    // Use this for initialization
    void Start()
    {
        grounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Grounded: " + grounded);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ground" || collision.tag == "Wall" || collision.tag == "Enemy" || collision.tag == "Grabbable")
            grounded = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "ground" || collision.tag == "Wall" || collision.tag == "Enemy" || collision.tag == "Grabbable")
            grounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ground" || collision.tag == "Wall" || collision.tag == "Enemy" || collision.tag == "Grabbable")
            grounded = false;
    }
}
