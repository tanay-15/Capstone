using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathScript : MonoBehaviour {

    public Transform BoneSystem;
    //public Transform BodyParts;
    public GameObject IKSystem;

    // Use this for initialization
    void Start () {

        Physics2D.IgnoreLayerCollision(15, 15);
        IKSystem.SetActive(false);
        SetChildrenKinematic(true);
        IKSystem.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "projectile" || collision.gameObject.tag == "Grabbable")
        {
            GetComponent<Animator>().enabled = false;
            SetChildrenKinematic(false);
            IKSystem.SetActive(false);
            //SetBodyPartsToBody();
        }
    }
    /*
    void SetBodyPartsToBody()
    {

        
        foreach (Transform t in BodyParts)
        {
            foreach (Transform u in BoneSystem.GetComponentsInChildren<Transform>())
            {
                Debug.Log(u.name);
                if (u.name == "bone_" + t.name)
                {
                    t.transform.position = u.transform.position;
                    t.transform.rotation = u.transform.rotation;
                }
            }
        }
        BodyParts.gameObject.SetActive(true);
        Body.gameObject.SetActive(false);
        
    }
    */


    public void SetChildrenKinematic(bool state)
    {
        foreach (Rigidbody2D rb2d in BoneSystem.GetComponentsInChildren<Rigidbody2D>())
        {
            if (rb2d.name != gameObject.name)
            {
                rb2d.isKinematic = state;
                
                //Debug.Log(rb2d.name);
            }

        }

        foreach (Collider2D coll in BoneSystem.GetComponentsInChildren<Collider2D>())
            coll.enabled = !state;
    }
}
