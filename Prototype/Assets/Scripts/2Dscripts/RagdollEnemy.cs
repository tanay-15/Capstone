using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEnemy : MonoBehaviour {


    Component[] bones;

    public bool isDead = false;

    void Start ()
    {

        bones = gameObject.transform.GetComponentsInChildren<Rigidbody2D>();

        SetChildrenKinematic(true);


    }
	

	void Update () {

        if (isDead == true)
        {
            SetChildrenKinematic(false);
        }        

	}

    void SetChildrenKinematic(bool state)
    {
        foreach (Rigidbody2D rb2d in bones)
        {
            if(rb2d.name != gameObject.name)
            {
                rb2d.isKinematic = state;
                rb2d.GetComponent<Collider2D>().enabled = !state;
                //Debug.Log(rb2d.name);
            }
                
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "projectile")
        {
            isDead = true;
            Debug.Log("hit");
        }
    }
}
