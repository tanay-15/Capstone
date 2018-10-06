﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEnemy : MonoBehaviour {

    Collider2D[] playerColliders;
    Collider2D[] myColliders;
    SpriteRenderer[] mySpriteRenderers;

    Component[] bones;

    public bool fadeAfterDeath = true;
    

    public bool isDead = false;

   

    void Start ()
    {

        bones = gameObject.transform.GetComponentsInChildren<Rigidbody2D>();
        mySpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        SetChildrenKinematic(true);
    }
	

	void Update () {

	}

    public void SetChildrenKinematic(bool state)
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

    void TurnOffCollisions()
    {
        playerColliders = Movement2D.sharedInstance.GetComponentsInChildren<Collider2D>();
        myColliders = GetComponentsInChildren<Collider2D>();

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
        TurnOffCollisions();
        yield return new WaitForSeconds(2f);

        float speed = 2.5f;
        Color newColor = Color.white;
        for (float i = 0f; i < 1f; i += Time.deltaTime * speed)
        {
            foreach (SpriteRenderer spr in mySpriteRenderers)
            {
                newColor.a = 1f - i;
                spr.color = newColor;
            }
            yield return 0;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "projectile" || collision.gameObject.tag == "Grabbable")
        {
            if (fadeAfterDeath)
                StartCoroutine(DieRoutine());

            isDead = true;
            Debug.Log("hit");
            SetChildrenKinematic(false);
            GetComponents<BoxCollider2D>()[0].enabled = false;
            GetComponents<BoxCollider2D>()[1].enabled = false;
        }
    }
}
