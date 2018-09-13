using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCheck : MonoBehaviour {

	List<GameObject> tiles;

	// Use this for initialization
	void Start () {
		tiles = new List<GameObject>();

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("swapTile")) {
            tiles.Add(g);
        }
	}
	
	// Update is called once per frame
	void Update () {
		bool die = true;
		
		foreach (GameObject g in tiles){

			if (g.gameObject.GetComponent<SpriteRenderer>().color != new Color(0,0,1.0f,1.0f)){
				die = false;
			}
		}

		if (die) {
			this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}
	}
}
