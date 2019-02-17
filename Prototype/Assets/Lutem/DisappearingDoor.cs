using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingDoor : MonoBehaviour {
    
    public List<GameObject> getToggles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bool open = true;

        foreach(GameObject g in getToggles)
        {
            if (g.GetComponent<ColorButton>())
            {
                if (!g.GetComponent<ColorButton>().pressed)
                {
                    open = false;
                }
            }
            else if (g.GetComponent<ButtonToggle>())
            {
                if (!g.GetComponent<ButtonToggle>().pressed)
                {
                    open = false;
                }
            }
        }

        if (open)
        {
            //this.GetComponent<BoxCollider2D>().enabled = false;
            //this.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.SetActive(false);
        }
	}
}
