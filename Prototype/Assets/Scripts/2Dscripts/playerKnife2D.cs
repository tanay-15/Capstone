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
        
        }

        if (collision.gameObject.tag == "Enemy")
        {
            hasHit = true;
            GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
            transform.parent = collision.gameObject.transform;

            collision.gameObject.SendMessageUpwards("applyDamage", 5);
            StartCoroutine(DelayDestroy());
        }
        else if(!(collision.gameObject.layer == 15))
        {
            Debug.Log(collision.gameObject.name);
            Destroy(gameObject);
        }

        /*
     if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SendMessageUpwards("applyDamage", 5);
            Destroy(this.gameObject);
        }
        */
   
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

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
