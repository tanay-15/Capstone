using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSlash : MonoBehaviour {

    // Use this for initialization
    private Animator anim;
    public bool slashHit;
    public bool inslash;
    public GameObject target;
	void Start () {

        anim = GetComponent<Animator>();
        slashHit = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Use ShadowSlash");
            anim.SetTrigger("ShadowSlash");
        }

	}

    void OnTriggerEnter(Collider collider)
    {
        if ((collider.gameObject.tag == "Enemy") && slashHit)
        {
            Debug.Log("Shadow Slash Connects on " + collider.name);
            collider.SendMessageUpwards("applyDamage", 5);
        }

        if(collider.gameObject.tag == "Enemy")
        {
            Debug.Log(collider.gameObject.name);
            inslash = true;
            target = collider.gameObject;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Enemy")
        {
            slashHit = false;
            inslash = false;
            target = null;
           
        }
    }

    void SlashHit()
    {
        slashHit = true;
        if (inslash)
        {
            Debug.Log("Shadow Slash connects on " + target.name);
            target.SendMessageUpwards("applyDamage", 10);
        }
    }
}
