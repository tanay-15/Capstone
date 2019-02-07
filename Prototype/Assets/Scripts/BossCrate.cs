using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrate : MonoBehaviour
{
    public Transform lowerLeftBound;
    public Transform upperRightBound;
    Vector3 startPos;
    Rigidbody2D rb;
    Collider2D myCollider;
    SpriteRenderer sr;
    float bounceSpeed = 4f;
    IEnumerator routine;
    Color startColor;

    Collider2D[] ignoredColliders;
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        routine = null;
        startColor = sr.color;
    }

    void IgnoreColliders(bool ignore)
    {
        foreach(Collider2D col in ignoredColliders)
        {
            Physics2D.IgnoreCollision(myCollider, col, ignore);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (Levitation.sharedInstance.HoldingObject && Levitation.sharedInstance.heldObject == gameObject)
            {
                Levitation.sharedInstance.ReleaseObject();
            }
            ignoredColliders = collision.gameObject.GetComponentsInChildren<Collider2D>();
            IgnoreColliders(true);

            Vector3 direction = (collision.gameObject.transform.position - transform.position).normalized;
            rb.velocity = -direction * bounceSpeed;
            if (routine != null)
                StopCoroutine(routine);
            routine = Cooldown();
            StartCoroutine(routine);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.gameObject.tag == "Enemy")
        {
            if (Levitation.sharedInstance.HoldingObject && Levitation.sharedInstance.heldObject == gameObject)
            {
                Levitation.sharedInstance.ReleaseObject();
            }
            ignoredColliders = collision.gameObject.GetComponentsInChildren<Collider2D>();
            IgnoreColliders(true);

            Vector3 direction = (collision.gameObject.transform.position - transform.position).normalized;
            rb.velocity = -direction * bounceSpeed;
            if (routine != null)
                StopCoroutine(routine);
            routine = Cooldown();
            StartCoroutine(routine);
        }
        */
    }

    IEnumerator Cooldown()
    {
        Color transparent = startColor;
        transparent.a = 0.5f;
        for(int i = 0; i < 13; i++)
        {
            sr.color = (i%2 == 0) ? transparent : startColor;
            yield return new WaitForSeconds(0.1f);
        }
        sr.color = startColor;
        IgnoreColliders(false);
    }

    // Update is called once per frame
    public void CheckAndResetPosition()
    {
        if (transform.position.x < lowerLeftBound.position.x ||
            transform.position.y < lowerLeftBound.position.y ||
            transform.position.x > upperRightBound.position.x ||
            transform.position.y > upperRightBound.position.y)
        {
            if (Levitation.sharedInstance.HoldingObject && Levitation.sharedInstance.heldObject == gameObject)
            {
                Levitation.sharedInstance.ReleaseObject();
            }
            rb.velocity = Vector3.zero;
            transform.position = startPos;
            transform.rotation = Quaternion.identity;
        }
    }
}
