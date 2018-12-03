using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public List<GameObject> getToggles;

    int numDown;

    Vector2 startPos;
    Vector2 midPos;
    Vector2 topPos;

    float t;

	// Use this for initialization
	void Start () {
        t = 0.0f;
        numDown = 0;
        startPos = this.transform.position;
        midPos = startPos;
        midPos.y += 1.65f;
        topPos = midPos;
        topPos.y += 1.65f;
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
    }
    
}
