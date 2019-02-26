using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToggle : MonoBehaviour {
    
    public bool pressed;

    List<GameObject> colliders;

	// Use this for initialization
	void Start () {
        colliders = new List<GameObject>();
        pressed = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (colliders.Count > 0)
        {
            pressed = true;
        }
        else
        {
            pressed = false;
        }
	}
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "PuzzleElement")
        {
            colliders.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "PuzzleElement")
        {
            colliders.Remove(collision.gameObject);
        }
    }
}
