﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class OrbitorObjectScript : MonoBehaviour {

    public bool isOrbiting = false;
    public bool hit = false;
    public GameObject impact;
    public UnityEvent OnPickedUp;
    Collider2D[] myColliders;
    public Collider2D[] ignoredColliders;
    AudioManager audioManager;

    // Use this for initialization
    void Start () {
        Physics2D.IgnoreLayerCollision(11, 12);

        Physics2D.IgnoreLayerCollision(17, 14);
        //Physics2D.IgnoreLayerCollision(17, 15);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        myColliders = GetComponentsInChildren<Collider2D>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
            
        GetComponent<Rigidbody2D>().gravityScale = 1;


        if (collision.gameObject.layer == 15 || collision.gameObject.tag == "Enemy")
        {
            if ((SkillTree.info.nodesActivated & SkillNodes.D_3) == SkillNodes.D_3)
                collision.gameObject.SendMessage("applyDamage", 20);
            else
                collision.gameObject.SendMessage("applyDamage", 10);
            Instantiate(impact, collision.GetContact(0).point, Quaternion.identity);
            //hit = true;
            Explode(8);
            hit = true;
            //IgnoreColliders(null, false);
        }

        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10 || collision.gameObject.tag == "Wall")
        {
            hit = true;
        }
        
    }

    public void IgnoreColliders(Collider2D[] colliders, bool ignore)
    {

        if (ignore)
        {
            ignoredColliders = colliders;
        }
        SetIgnore(ignore);
        if (!ignore)
        {
            ignoredColliders = null;
        }
    }

    private void SetIgnore(bool ignore)
    {
        foreach (Collider2D myCol in myColliders)
        {
            foreach (Collider2D col in ignoredColliders)
            {
                Physics2D.IgnoreCollision(myCol, col, ignore);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //hit = false;
    }


    public void Explode(float magnitude = 8f)
    {
        audioManager.Play("DStomp");
        Vector2 center = transform.position;
        foreach (Rigidbody2D rb2d in GetComponentsInChildren<Rigidbody2D>(true))
        {
            if (rb2d.name != gameObject.name)
            {
                rb2d.gameObject.SetActive(true);
                rb2d.transform.parent = null;
                Vector2 direction = rb2d.position - center;
                direction.Normalize();
                rb2d.AddForce(direction * magnitude, ForceMode2D.Impulse);
                Destroy(rb2d.gameObject,3);
            }

        }

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<ParticleSystem>().Play();

        StartCoroutine(DelayedDestroy());
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
