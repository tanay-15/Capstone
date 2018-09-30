using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    public int phase;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Tutorial.sharedInstance.SetPhase(phase);
            Destroy(gameObject);
        }
    }
}
