using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleProjectile : MonoBehaviour
{
    // speed at travel
    public float speed;
    public float distance;

    Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        // set target 10 units in direction
        target = this.transform.position + (transform.up * distance);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        if(this.transform.position == target)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // collision with ground doesn't seem to work
        // collision.gameObject.tag == "ground" || 

        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerLifeController>().GetHit(-10);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.layer == 18 || collision.gameObject.layer == 11)
            Destroy(this.gameObject);
    }
}
