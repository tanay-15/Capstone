using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Manager : MonoBehaviour {

    // Use this for initialization

    public GameObject crate_prefab;
    public GameObject[] current_crates;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        current_crates = GameObject.FindGameObjectsWithTag("Grabbable");

        if(current_crates.Length == 0)
        {
            Instantiate(crate_prefab, this.transform.position, this.transform.rotation);
        }
	}
}
