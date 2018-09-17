using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded2D : MonoBehaviour {

    public bool grounded= false;

    // Use this for initialization
    void Start()
    {
        grounded = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        grounded = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        grounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        grounded = false;
    }
}
