using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerKnife2D : MonoBehaviour {

    public bool hasHit = false;
    public GameObject ImpactAnim;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if(!hasHit)
            transform.eulerAngles = -new Vector3(0, 0, Vector2.SignedAngle(GetComponent<Rigidbody2D>().velocity, new Vector2(GetComponent<Rigidbody2D>().velocity.x,0)));
        
        
        //if(!hasHit == true)
            //transform.Rotate(new Vector3(0,0,-Time.deltaTime * 300));

	}

    

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.layer != 15)
        {
            Debug.Log(collision.gameObject.name);
            Destroy(gameObject);
        }

        if (!hasHit)
        {
            var impact = Instantiate(ImpactAnim, transform.position, Quaternion.identity);
            impact.gameObject.SetActive(true);
        }

        hasHit = true;

        //GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        transform.parent = collision.gameObject.transform;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        



    }




    private void OnTriggerEnter2D(Collider2D other)
    {


    }

    virtual public float GetPosX()
    {
        Debug.Log("Position requested");
        return this.transform.position.x;
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
