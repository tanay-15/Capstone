using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerArrow : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rigi;
    
    void Start()
    {
        rigi = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigi.velocity = new Vector2(-4f, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "projectile")
        {
            Destroy(this.gameObject);
        }

        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessageUpwards("GetHit", -10f);
            Destroy(this.gameObject);
        }

        Destroy(this.gameObject);
    }
}
