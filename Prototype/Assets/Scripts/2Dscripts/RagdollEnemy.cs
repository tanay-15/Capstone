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

    void SetChildrenKinematic(bool kinematic)
    {
        foreach (Rigidbody2D rb2d in bones)
        {
            if(rb2d.name != "Stickman")
                rb2d.isKinematic = kinematic;
        }
    }
}
