using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitation : MonoBehaviour {

    public static Levitation sharedInstance;
    public GameObject particles;
    [System.NonSerialized]
    public GameObject heldObject;

    static Levitation()
    {
        sharedInstance = null;
    }

	void Start () {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        heldObject = null;
	}
	
	void Update () {
        Vector3 mouseOnNearClipPlane = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.gameObject.transform.position.z);
        particles.transform.position = Camera.main.ScreenToWorldPoint(mouseOnNearClipPlane);
	}
}
