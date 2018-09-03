using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonModeScript : MonoBehaviour {

    public GameObject bat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("z"))
        {
            StartCoroutine("Transform");
            for (int i = 0; i < 25; i++)
            {
                var Bat = Instantiate(bat, transform.position + new Vector3(0,-10,0), Quaternion.identity);
                Bat.GetComponent<Bat_Script>().player = gameObject;
            }
        }

	}

    IEnumerator Transform()
    {
        yield return new WaitForSeconds(3);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
