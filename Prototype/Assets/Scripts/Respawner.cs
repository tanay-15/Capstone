using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour {

    public GameObject[] trackedObjects;
    Vector3[] originalPosition;

    void Start()
    {
        originalPosition = new Vector3[trackedObjects.Length];
        for (int i = 0; i < trackedObjects.Length; i++)
        {
            originalPosition[i] = trackedObjects[i].transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        for (int i = 0; i < trackedObjects.Length; i++)
        {
            if (trackedObjects[i] == col.gameObject)
            {
                col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                col.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                col.gameObject.transform.position = originalPosition[i];
            }
        }
    }
}
