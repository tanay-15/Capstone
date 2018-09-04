using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSlash : MonoBehaviour {

    // Use this for initialization
    private Animator anim;
    private bool slashHit;
	void Start () {

        anim = GetComponent<Animator>();
        slashHit = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.SetTrigger("ShadowSlash");
        }

	}

    void OnTriggerEnter(Collider collider)
    {
        if ((collider.gameObject.tag == "Enemy") && slashHit)
        {
            Debug.Log("Shadow Slash Connects on " + collider.name);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Enemy")
        {
            slashHit = false;
        }
    }

    void SlashHit()
    {
        slashHit = true;
    }
}
