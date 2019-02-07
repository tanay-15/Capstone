using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public bool timed;
    public int delayTime;
    public int respawnTime;

    Vector2 startPosition;
    Vector2 fallPosition;
    bool falling;
    bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        falling = false;
        triggered = false;
        fallPosition = this.transform.position;
        fallPosition.y -= 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (falling)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, fallPosition, 15.0f * Time.deltaTime);
        }
        if(this.transform.position == new Vector3(fallPosition.x, fallPosition.y, this.transform.position.z))
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            falling = false;
            StartCoroutine(Respawn());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.gameObject.transform.position.y > this.transform.position.y)
        {
            if (timed && !triggered)
            {
                triggered = true;
                StartCoroutine(TimedFall());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.transform.position.y > this.transform.position.y)
        {
            if (!timed && !triggered)
            {
                this.GetComponent<BoxCollider2D>().enabled = false;
                falling = true;
                triggered = true;
            }
        }
    }

    IEnumerator TimedFall()
    {
        this.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.65f, 0.0f);
        yield return new WaitForSeconds(delayTime / 3.0f);
        this.GetComponent<SpriteRenderer>().color = new Color(0.65f, 0.3f, 0.0f);
        yield return new WaitForSeconds(delayTime / 3.0f);
        this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
        yield return new WaitForSeconds(delayTime / 3.0f);
        this.GetComponent<BoxCollider2D>().enabled = false;
        falling = true;
        yield return null;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        this.transform.position = startPosition;
        if (timed)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
        }
        triggered = false;
        this.GetComponent<BoxCollider2D>().enabled = true;
        this.GetComponent<SpriteRenderer>().enabled = true;
        yield return null;
    }
    
}
