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
    bool respawning;

    // Start is called before the first frame update
    void Start()
    {
        this.startPosition = this.transform.position;
        this.falling = false;
        this.triggered = false;
        respawning = false;
        this.fallPosition = this.transform.position;
        this.fallPosition.y -= 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.falling)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, fallPosition, 10.0f * Time.deltaTime);
        }
        if(this.transform.position == new Vector3(fallPosition.x, fallPosition.y, this.transform.position.z) && !respawning)
        {
            respawning = true;
            this.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(Respawn());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && (collision.gameObject.transform.position.y + 0.3f) > this.transform.position.y)
        {
            if (timed && !triggered)
            {
                this.triggered = true;
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
                this.GetComponents<BoxCollider2D>()[0].enabled = false;
                this.GetComponents<BoxCollider2D>()[1].enabled = false;
                this.falling = true;
                this.triggered = true;
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
        this.falling = true;
        yield return null;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        this.transform.position = startPosition;
        RecreatePlatform();
        yield return null;
    }

    private void RecreatePlatform()
    {
        if (timed)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            this.GetComponents<BoxCollider2D>()[0].enabled = true;
            this.GetComponents<BoxCollider2D>()[1].enabled = true;
        }
        respawning = false;
        this.falling = false;
        this.triggered = false;
        this.GetComponent<SpriteRenderer>().enabled = true;
    }
    
}
