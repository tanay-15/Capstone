using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour {

    // Use this for initialization

    private float timecounter = 3f;

	void Start () {
		
	}



    // Update is called once per frame
    void Update() {

        timecounter -= Time.deltaTime;

        if (timecounter <= 0f)
        {
            Death();
        }
	}

    void Death()
    {
        Destroy(this.gameObject);
    }
}
