using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerKnife2D : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(new Vector3(0,0,-Time.deltaTime * 250));

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
