using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeController : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            PlayerLife.sharedInstance.AddLife(-10);
            if (PlayerLife.sharedInstance.currentLife <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
