using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerKnife2D : MonoBehaviour {

    bool hasHit = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if(!hasHit == true)
            //transform.Rotate(new Vector3(0,0,-Time.deltaTime * 300));

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "AttackTrigger")
        {
            Debug.Log(collision.gameObject.name);
            Debug.Log(collision.gameObject.transform.position);
        }
        {
            Debug.Log(collision.gameObject.name);
        }
        if (collision.gameObject.name == "Torso" || collision.gameObject.name == "Head" || collision.gameObject.name == "thigh" || collision.gameObject.name == "shin")
        {
            hasHit = true;
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
            transform.parent = collision.gameObject.transform;
        }
        else if(!(collision.gameObject.name == "biceps" || collision.gameObject.name == "forearm"))
        {
            Destroy(gameObject);
        }

     if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SendMessageUpwards("applyDamage", 5);
            Destroy(this.gameObject);
        }

   
    }

    

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Torso" || collision.gameObject.name == "thigh" || collision.gameObject.name == "Head" || collision.gameObject.name == "shin")
        {
            hasHit = true;
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
            transform.parent = collision.gameObject.transform;
        }
        
    }

    public float GetPosX()
    {
        return this.transform.position.x;
    }
    
}
