using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public List<GameObject> getToggles;

    List<GameObject> standingOn;

    int numDown;

    public float offset;

    Vector2 startPos;
    Vector2 midPos;
    Vector2 topPos;

    float t;

	// Use this for initialization
	void Start () {
        standingOn = new List<GameObject>();
        t = 0.0f;
        numDown = 0;
        startPos = this.transform.position;
        midPos = startPos;
        midPos.y += offset;
        topPos = midPos;
        topPos.y += offset;
	}
	
	// Update is called once per frame
	void Update () {
        numDown = 0;
        foreach (GameObject g in getToggles)
        {
            if (g.GetComponent<ButtonToggle>().pressed)
            {
                numDown++;
            }
        }

        Vector2 target;
        switch (numDown)
        {
            case 0:
                target = startPos;
                break;
            case 1:
                target = midPos;
                break;
            case 2:
                target = topPos;
                break;
            default:
                target = startPos;
                break;
        }

        this.transform.position = Vector2.MoveTowards(this.transform.position, target, 0.05f);
        foreach(GameObject g in standingOn)
        {
            g.transform.position = Vector2.MoveTowards(g.transform.position, new Vector2(g.transform.position.x, g.transform.position.y) + (target - new Vector2(this.transform.position.x, this.transform.position.y)), 0.05f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            standingOn.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            standingOn.Remove(collision.gameObject);
        }
    }
}
