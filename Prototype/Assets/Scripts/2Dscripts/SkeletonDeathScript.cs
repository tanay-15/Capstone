﻿using Anima2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathScript : MonoBehaviour {

    public Transform BoneSystem;
    //public Transform BodyParts;
    //public GameObject IKSystem;
    Collider2D[] playerColliders;
    Collider2D[] myColliders;
    SpriteMeshInstance[] mySprites;

    public AudioClip[] BonesBreak;
    AudioSource Audio;

    public bool kill = false;
    public bool dead = false;
    //public bool spawn = true;
    public Vector4 color;
    //health death
    public int mhealth;

    // Use this for initialization
    void Start () {

        foreach (SpriteMeshInstance s in GetComponentsInChildren<SpriteMeshInstance>())
        {
            s.color = color;
        }

        myColliders = GetComponentsInChildren<Collider2D>();
        mySprites = GetComponentsInChildren<SpriteMeshInstance>();

        //IKSystem.SetActive(false);
        SetChildrenKinematic(true);
        //IKSystem.SetActive(true);

        //foreach (Transform t in IKSystem.GetComponentsInChildren<Transform>())
        //{
        //    t.gameObject.SetActive(false);
        //    t.gameObject.SetActive(true);
        //}

        //Audio = GetComponent<AudioSource>();
        //Audio.clip = BonesBreak[Random.Range(0, BonesBreak.Length - 1)];

        GetComponent<Animator>().Play("Assemble");
        GetComponent<Animator>().speed = 0;

    }

    // Update is called once per frame
    void Update () {

        //if (spawn)
        //{
            
        //    GetComponent<Animator>().speed = 1;
        //    StartCoroutine(Initialize());

            
        //}

        mhealth = this.gameObject.GetComponent<Enemy>().GetHealth();

    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == 14)
    //    {
    //        //Debug.Log("Spawnz");
    //        Spawn();
    //    }
    //}

    void TurnOffCollisions()
    {
        playerColliders = GameObject.Find("Player").GetComponentsInChildren<Collider2D>();
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
        //Audio.Play();

        yield return new WaitForSeconds(2f);

        float speed = 2.5f;
        Color newColor = color;
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

    public void Die()
    {
        if (!dead)
        {
            dead = true;
            
            GetComponent<Animator>().enabled = false;
            SetChildrenKinematic(false);
            //IKSystem.SetActive(false);

            Explode(new Vector2(transform.position.x, transform.position.y-1), 5);
            FindObjectOfType<AudioManager>().Play("BoneBreaks");
            StartCoroutine(DieRoutine());

            GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponents<BoxCollider2D>()[0].enabled = false;
            if(GetComponent<CircleCollider2D>())
                GetComponents<CircleCollider2D>()[0].enabled = false;


            TurnOffCollisions();
            
        }

    }





    public void SetChildrenKinematic(bool state)
    {
        foreach (Rigidbody2D rb2d in BoneSystem.GetComponentsInChildren<Rigidbody2D>())
        {
            if (rb2d.name != gameObject.name && rb2d.gameObject.layer == 15)
            {
                rb2d.isKinematic = state;

                //Debug.Log(rb2d.name);
            }

        }

        foreach (Collider2D coll in BoneSystem.GetComponentsInChildren<Collider2D>())
        {
            if(coll.gameObject.layer == 15)
            coll.enabled = !state;
        }
            
    }


    void Explode(Vector2 center, float magnitude)
    {

        foreach (Rigidbody2D rb2d in BoneSystem.GetComponentsInChildren<Rigidbody2D>())
        {
            if (rb2d.name != gameObject.name && rb2d.gameObject.layer == 15)
            {
                Vector2 direction = rb2d.position - center;
                direction.Normalize();
                rb2d.AddForce(direction*magnitude,ForceMode2D.Impulse);
            }

        }

    }

    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(1.0f);
        GetComponent<Enemy>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        //spawn = false;
        gameObject.SendMessageUpwards("EnableLifebar", true);
        //GetComponent<Animator>().Play("SkelWalk");
    }

    public void Spawn()
    {
        //spawn = true;
        GetComponent<Animator>().speed = 1;
        StartCoroutine(Initialize());
    }
}
