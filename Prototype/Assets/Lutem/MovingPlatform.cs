using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public List<GameObject> getToggles;

    int numDown;

    Vector2 startPos;
    Vector2 endPos;

	// Use this for initialization
	void Start () {
        numDown = 0;
        startPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        endPos = startPos;
        numDown = 0;
        foreach (GameObject g in getToggles)
        {
            if (g.GetComponent<ButtonToggle>().pressed)
            {
                numDown++;
            }
        }

        switch (numDown)
        {
            case 0:
                this.transform.position = startPos;
                break;
            case 1:
                endPos.y += 1.7f;
                this.transform.position = endPos;
                break;
            case 2:
                endPos.y += 3.4f;
                this.transform.position = endPos;
                break;
        }
    }
    
        
}
