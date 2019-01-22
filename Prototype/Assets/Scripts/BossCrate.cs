using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrate : MonoBehaviour
{
    public Transform lowerLeftBound;
    public Transform upperRightBound;
    Vector3 startPos;
    Rigidbody2D rb;
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void CheckAndResetPosition()
    {
        if (transform.position.x < lowerLeftBound.position.x ||
            transform.position.y < lowerLeftBound.position.y ||
            transform.position.x > upperRightBound.position.x ||
            transform.position.y > upperRightBound.position.y)
        {
            if (Levitation.sharedInstance.HoldingObject && Levitation.sharedInstance.heldObject == gameObject)
            {
                Levitation.sharedInstance.ReleaseObject();
            }
            rb.velocity = Vector3.zero;
            transform.position = startPos;
            transform.rotation = Quaternion.identity;
        }
    }
}
