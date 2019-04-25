using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbPuzzle_End : MonoBehaviour
{
    // game object you want to affect
    public GameObject target;
    public GameObject[] targets;
    bool playerPresent;
    // only do once
    bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        triggered = false;
        playerPresent = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered == true)
        {
            foreach (GameObject g in targets)
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
        if (collision.gameObject.name.Contains("Orb") && !triggered)
        {
            // do whatever with target object

            // destroy orb
            Destroy(collision.gameObject);
            Timer.sharedInstance.Stop();
            ScreenPointer.sharedInstance.RemoveTarget();
            triggered = true;
            playerPresent = true;
            this.transform.GetComponent<SpriteRenderer>().color = new Color(0x00,0xFF,0xC7);
        }
    }
}
