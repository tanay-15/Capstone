using Anima2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathScript : MonoBehaviour {

    public Transform BoneSystem;
    //public Transform BodyParts;
    public GameObject IKSystem;
    Collider2D[] playerColliders;
    Collider2D[] myColliders;
    SpriteMeshInstance[] mySprites;

    public AudioClip[] BonesBreak;
    AudioSource Audio;

    public bool kill = false;
    public bool dead = false;
    //health death
    public int mhealth;

    // Use this for initialization
    void Start () {


        myColliders = GetComponentsInChildren<Collider2D>();
        mySprites = GetComponentsInChildren<SpriteMeshInstance>();

        IKSystem.SetActive(false);
        SetChildrenKinematic(true);
        IKSystem.SetActive(true);

        foreach (Transform t in IKSystem.GetComponentsInChildren<Transform>())
        {
            t.gameObject.SetActive(false);
            t.gameObject.SetActive(true);
        }

        //Audio = GetComponent<AudioSource>();
        //Audio.clip = BonesBreak[Random.Range(0, BonesBreak.Length - 1)];
    }
	
	// Update is called once per frame
	void Update () {

        if (kill)
            Die();

        mhealth = this.gameObject.GetComponent<Enemy>().GetHealth();
	}

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

    public void Die()
    {
        if (!dead)
        {
            dead = true;
            GetComponent<Animator>().enabled = false;
            SetChildrenKinematic(false);
            IKSystem.SetActive(false);

            Explode(new Vector2(transform.position.x, transform.position.y-1), 5);
            StartCoroutine(DieRoutine());
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
}
