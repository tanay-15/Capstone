using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbPuzzle_End : MonoBehaviour
{
    // game object you want to affect
    public GameObject target;

    // only do once
    bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        triggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
    }
}
