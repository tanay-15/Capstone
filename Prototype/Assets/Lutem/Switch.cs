using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

    public List<GameObject> targets;

    bool player;

	// Use this for initialization
	void Start () {
        player = false;	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftControl) && player)
        {
            Interact();
        }
	}

    void Interact()
    {
        foreach (GameObject g in targets)
        {
            if (g.GetComponent<Swap>())
            {
                g.GetComponent<Swap>().toggle();
            }
            if (g.GetComponent<ChangeDoorStatus>())
            {
                g.GetComponent<ChangeDoorStatus>().toggle();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = true;
            collision.gameObject.SendMessage("Show", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = false;
            collision.gameObject.SendMessage("Show", false);
        }
    }
}
