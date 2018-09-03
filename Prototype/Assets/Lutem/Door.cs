using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    List<GameObject> buttons;
    bool open;

	// Use this for initialization
	void Start () {
        buttons = new List<GameObject>();

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("button")) {
            buttons.Add(g);
        }
	}
	
	// Update is called once per frame
	void Update () {
        open = true;
        foreach(GameObject g in buttons)
        {
            if (!g.GetComponent<Button>().down)
            {
                open = false;
            }
        }

        if (open)
        {
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
	}
}
