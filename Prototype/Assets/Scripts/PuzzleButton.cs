using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    // game objects to affect
    public GameObject[] targets;
    public ParticleSystem particle;

    bool playerPresent;
    bool EmitPartciles = false;

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
            
            if (EmitPartciles == false)
            {
                EmitPartciles = true;
                particle.Emit(1);
            }
            else
                EmitPartciles = false;
            foreach (GameObject g in targets)
            {
                if (!g.GetComponent<PuzzleButton_Target>().moving)
                {
                    g.GetComponent<PuzzleButton_Target>().StartMove();
                }
            }
        }

        if (EmitPartciles == true)
        {
            particle.Emit(1);
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
