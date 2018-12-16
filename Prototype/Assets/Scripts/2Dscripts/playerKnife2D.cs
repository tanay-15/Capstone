using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerKnife2D : MonoBehaviour {

    bool hasHit = false;
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

        if(!(collision.gameObject.layer == 15))
        {
            Debug.Log(collision.gameObject.name);
            Destroy(gameObject);
        }

        hasHit = true;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        transform.parent = collision.gameObject.transform;


        // Impact Sprite
        var impact = Instantiate(ImpactAnim, transform.position, Quaternion.identity);
        impact.gameObject.SetActive(true);
        //StartCoroutine(DelayDestroy());


        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SendMessageUpwards("applyDamage", 5);
            
        }


    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        //if ((other.gameObject.layer != 14) && (other.GetType() == typeof(BoxCollider2D)))
        //{
        //    var impact = Instantiate(ImpactAnim, transform.position, Quaternion.identity);
        //    impact.gameObject.SetActive(true);

        //    other.gameObject.SendMessageUpwards("applyDamage", 5);
        //    Destroy(gameObject);
        //}

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
