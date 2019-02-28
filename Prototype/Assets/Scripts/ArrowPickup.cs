using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPickup : MonoBehaviour
{
    public GameObject sprites;
    float count;
    float scale;
    Rigidbody2D rb;
    bool canPickUp;
    void Start()
    {
        canPickUp = false;
        count = 0f;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.up;
    }

    void Update()
    {
        count += Time.deltaTime;
        scale = 1f + (0.3f * Mathf.Sin(count * 4f));
        sprites.transform.localScale = Vector3.one * scale;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground" || col.gameObject.tag == "Wall")
        {
            canPickUp = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (canPickUp)
            {
                Destroy(gameObject);
                ArrowCounter.sharedInstance.SetMaxArrows();
            }
        }
    }
}
