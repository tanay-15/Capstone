using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonModeScript : MonoBehaviour {

    public GameObject bat;
    public bool DemonModeActive;
    bool transitioning;

	// Use this for initialization
	void Start () {
        DemonModeActive = false;
        transitioning = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("z") && !transitioning)
        {
            transitioning = true;
            StartCoroutine("Transform", !DemonModeActive);
            for (int i = 0; i < 40; i++)
            {
                var Bat = Instantiate(bat, transform.position + new Vector3(0,-10,0), Quaternion.identity);
                Bat.GetComponent<Bat_Script>().player = gameObject;
            }
        }

	}

    IEnumerator Transform(bool toDemon)
    {
        yield return new WaitForSeconds(3);

        transform.GetComponent<Movement2D>().myAnim = (toDemon) ? transform.Find("Demon").GetComponent<Animator>() : transform.Find("Normal").GetComponent<Animator>();
        transform.Find("Normal").gameObject.SetActive(!toDemon);
        transform.Find("Demon").gameObject.SetActive(toDemon);
        transitioning = false;
        DemonModeActive = toDemon;
    }
}
