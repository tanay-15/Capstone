using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodWellScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<rageBar>().RBar.fillAmount <= 1)
            {collision.gameObject.GetComponent<rageBar>().
                RBar.fillAmount += 0.2f * Time.deltaTime;
            }
            
        }
    }
}
