using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    // game objects to affect
    public GameObject[] targets;

    bool playerPresent;

    // Start is called before the first frame update
    void Start()
    {
        playerPresent = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerPresent && Input.GetKeyDown(KeyCode.LeftControl) || Input.GetButtonDown("PS4TRIANGLE"))
        {
            foreach(GameObject g in targets)
            {
                if (!g.GetComponent<PuzzleButton_Target>().moving)
                {
                    g.GetComponent<PuzzleButton_Target>().StartMove();
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
        if (collision.gameObject.tag == "Player")
        {
            playerPresent = false;
        }
    }
}
