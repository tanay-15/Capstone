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
            if (Input.GetKeyDown(KeyCode.LeftControl) && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStates>().movable)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStates>().movable = false;
                grabbed = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStates>().movable = true;
                grabbed = false;
            }
        }
        if (grabbed)
        {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                this.transform.Rotate(Vector3.forward * 360 * Time.deltaTime);

                foreach(GameObject g in targets)
                {
                    g.GetComponent<PulleyTargets>().MoveTarget(1);
                }
            }
            if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                this.transform.Rotate(Vector3.forward * -360 * Time.deltaTime);

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
