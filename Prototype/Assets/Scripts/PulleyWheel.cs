using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyWheel : MonoBehaviour
{
    public GameObject[] targets;

    bool playerPresent;
    bool grabbed;

    // Start is called before the first frame update
    void Start()
    {
        playerPresent = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPresent)
        {
            // replace these with whatever keys/inputs we need
            if ((Input.GetKeyDown(KeyCode.LeftControl)|| Input.GetButtonDown("PS4TRIANGLE")) && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStates>().enabled)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStates>().Human.GetComponent<Animator>().Play("Idle");
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStates>().enabled = false;
                grabbed = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetButtonDown("PS4TRIANGLE"))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStates>().enabled = true;
                grabbed = false;
            }
        }
        if (grabbed)
        {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Horizontal") > 0)
            {
                this.transform.Rotate(Vector3.forward * 180 * Time.deltaTime);

                foreach(GameObject g in targets)
                {
                    g.GetComponent<PulleyTargets>().MoveTarget(1);
                }
            }
            if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0 || Input.GetAxis("Horizontal") < 0)
            {
                this.transform.Rotate(Vector3.forward * -180 * Time.deltaTime);

                foreach (GameObject g in targets)
                {
                    g.GetComponent<PulleyTargets>().MoveTarget(-1);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerPresent = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerPresent = false;
        }
    }
}
