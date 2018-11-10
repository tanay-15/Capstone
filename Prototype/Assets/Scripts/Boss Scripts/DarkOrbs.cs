using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkOrbs : MonoBehaviour {

    // Use this for initialization

    private Vector3 moveVector;
    private float descounter = 5f;

    public GameObject HitIndicator;
    private GameObject go;

    private Vector3 indicPos;
    private bool indicflag = false;
    private RaycastHit2D hit;
    private int layer_mask;
    public float movespeed;
	void Start () {

        moveVector = new Vector3(0, -movespeed * Time.deltaTime, 0);
        layer_mask = LayerMask.NameToLayer("Ground");
	}
	
	// Update is called once per frame
	void Update () {

        Debug.Log(layer_mask);
        this.transform.Translate(moveVector);

        descounter = descounter - Time.deltaTime;
        if(descounter <= 0)
        {
            Destroy(this.gameObject);
        }

        if (hit = Physics2D.Raycast(this.transform.position, -transform.up,10, 1<< layer_mask)) 
        {
            Debug.Log(hit.collider.gameObject.name + "This is colliding");
            indicPos = hit.point;
            if (!indicflag)
            {
              go =   Instantiate(HitIndicator, indicPos, HitIndicator.transform.rotation) as GameObject;
                indicflag = true;
            }
           
            
        }
        Debug.DrawRay(this.transform.position, -transform.up);
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hits " + collision.gameObject.name);
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("GetHit", -15);
        }
        Destroy(go);
        Destroy(this.gameObject);
    }
}
