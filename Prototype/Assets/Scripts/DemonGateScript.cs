using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonGateScript : MonoBehaviour
{
    //bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((collision.gameObject.name == "AttackTrigger") || (collision.gameObject.name == "StompTrigger")) && collision.transform.parent.parent.GetComponent<DemonTransformScript>().DemonModeActive)
        {
            GetComponent<Collider2D>().enabled = false;
            Explode();
        }
    }

    public void Explode(float magnitude = 8f)
    {
        Vector2 center;
        foreach (Rigidbody2D rb2d in GetComponentsInChildren<Rigidbody2D>(true))
        {
            center = rb2d.transform.parent.position;
            rb2d.transform.parent.GetComponent<SpriteRenderer>().enabled = false; ;
            rb2d.gameObject.SetActive(true);
            rb2d.transform.parent = null;                
            Vector2 direction = rb2d.position - center;
            direction.Normalize();
            rb2d.AddForce(direction * magnitude, ForceMode2D.Impulse);
            Destroy(rb2d.gameObject, 3);
        }
    }

    }
