using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingPlatform : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(Drop());
            
        }
    }

    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().gravityScale = 5.0f;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -25.0f));
        GetComponents<BoxCollider2D>()[0].enabled = false;
        yield return null;
    }
}
