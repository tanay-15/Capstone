using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            collision.gameObject.SendMessageUpwards("GetHit", -5.0f);
        }
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }


}
