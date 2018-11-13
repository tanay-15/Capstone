using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedArrow : playerKnife2D {
    public GameObject ringPrefab;
    //Efficient data structure for lookup and storage
    HashSet<GameObject> hitObjects;
    Rigidbody2D rb;
    Vector3 startVelocity;
    float startY;
    Vector3 newPos;
    float ringInterval = 0.05f;
    float ringTime;
	void Start () {
        hitObjects = new HashSet<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        startY = transform.position.y;
        startVelocity = rb.velocity;
        ringTime = ringInterval;
	}

    private void FixedUpdate()
    {
        //Keep the arrow from changing velocity or y position
        rb.velocity = startVelocity;
        newPos.x = transform.position.x;
        newPos.y = startY;
        newPos.z = transform.position.z;
        transform.position = newPos;
    }

    // Update is called once per frame
    void Update () {
        ringTime -= Time.deltaTime;
        if (ringTime < 0f)
        {
            ringTime = ringInterval;
            Instantiate(ringPrefab, transform.position + Vector3.right * transform.localScale.x * 0.2f, Quaternion.identity);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!(collision.gameObject.layer == 15))
        {
            Debug.Log(collision.gameObject.name);
            Destroy(gameObject);
        }

        //Don't hit the same object more than once
        if (!hitObjects.Contains(collision.gameObject))
        {
             hitObjects.Add(collision.gameObject);
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
            transform.parent = collision.gameObject.transform;


            // Impact Sprite
            var impact = Instantiate(ImpactAnim, transform.position, Quaternion.identity);
            impact.gameObject.SetActive(true);

            collision.gameObject.SendMessageUpwards("ApplyDamage", 8);  //Normal arrow does 5
            //StartCoroutine(DelayDestroy());
        }
    }
}
