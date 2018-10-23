using Anima2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathScript : BasicEnemy {

    public Transform BoneSystem;
    //public Transform BodyParts;
    public GameObject IKSystem;
    Collider2D[] playerColliders;
    Collider2D[] myColliders;
    SpriteMeshInstance[] mySprites;


    public override Vector3 lifebarOffset
    {
        get
        {
            return Vector3.up * 2f;
        }
    }

    // Use this for initialization
    void Start () {

        Physics2D.IgnoreLayerCollision(15, 15);

        
        IKSystem.SetActive(false);
        SetChildrenKinematic(true);
        IKSystem.SetActive(true);

        foreach (Transform t in IKSystem.GetComponentsInChildren<Transform>())
        {
            Debug.Log(t.gameObject.name);
            t.gameObject.SetActive(false);
            t.gameObject.SetActive(true);
        }

        myColliders = GetComponentsInChildren<Collider2D>();
        mySprites = GetComponentsInChildren<SpriteMeshInstance>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void TurnOffCollisions()
    {
        playerColliders = Movement2D.sharedInstance.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D playerCol in playerColliders)
        {
            foreach (Collider2D myCol in myColliders)
            {
                Physics2D.IgnoreCollision(playerCol, myCol, true);
            }
        }
    }

    IEnumerator DieRoutine()
    {
        //TurnOffCollisions();
        yield return new WaitForSeconds(2f);

        float speed = 2.5f;
        Color newColor = Color.white;
        for (float i = 0f; i < 1f; i += Time.deltaTime * speed)
        {
            foreach (SpriteMeshInstance s in mySprites)
            {
                newColor.a = 1f - i;
                s.color = newColor;
            }
            yield return 0;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Vector3.Distance(transform.position,collision.transform.position) < 2f)  // Checking which trigger
        {
            if (collision.gameObject.tag == "projectile" || collision.gameObject.tag == "Grabbable")
            {
                GetComponent<Animator>().enabled = false;
                SetChildrenKinematic(false);
                IKSystem.SetActive(false);
                //SetBodyPartsToBody();

                events.OnDeath.Invoke();
                StartCoroutine(DieRoutine());
                TurnOffCollisions();
            }
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
