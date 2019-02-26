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
    //Vector3 newPos;
    float ringInterval = 0.05f;
    float ringTime;
    Collider2D myCollider;
    [System.NonSerialized]
    public bool fullyCharged;

    public static int arrowCount;
    public const int maxArrows = 3;

    static ChargedArrow()
    {
        arrowCount = 0;
    }
	void Start () {
        hitObjects = new HashSet<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        startY = transform.position.y;
        startVelocity = rb.velocity;
        ringTime = ringInterval;
        myCollider = GetComponent<Collider2D>();
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * 180f / Mathf.PI;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        arrowCount++;
	}

    private void FixedUpdate()
    {
        //Keep the arrow from changing velocity
        //rb.velocity = startVelocity;
        //newPos.x = transform.position.x;
        //newPos.y = startY;
        //newPos.z = transform.position.z;
        //transform.position = newPos;

        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * 180f / Mathf.PI;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Update is called once per frame
    void Update () {
        GetComponent<SpriteRenderer>().flipX = false;
        ringTime -= Time.deltaTime;
        Vector3 offset = new Vector3(rb.velocity.x, rb.velocity.y, 0f).normalized;
        if (ringTime < 0f && fullyCharged)
        {
            ringTime = ringInterval;
            Instantiate(ringPrefab, transform.position + transform.localScale.x * offset * 0.2f, transform.rotation);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10)
        { 
            //Debug.Log(collision.gameObject.name);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "ArrowDestroy")
        {
            //Debug.Log(collision.gameObject.name);
            Destroy(collision.gameObject);
            //Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //Don't hit the same object more than once
            if (!hitObjects.Contains(collision.gameObject))
            {
                hitObjects.Add(collision.gameObject);
                foreach (Collider2D enemyCol in collision.gameObject.GetComponentsInChildren<Collider2D>())
                {
                    Physics2D.IgnoreCollision(myCollider, enemyCol);
                }

                // Impact Sprite
                var impact = Instantiate(ImpactAnim, transform.position, Quaternion.identity);
                impact.gameObject.SetActive(true);

                collision.gameObject.SendMessageUpwards("applyDamage", (fullyCharged) ? 10 : 5);  //Normal arrow does 5
                                                                             //StartCoroutine(DelayDestroy());
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        arrowCount--;
    }
}
