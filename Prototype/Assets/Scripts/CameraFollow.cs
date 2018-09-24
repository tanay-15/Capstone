using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    public float speed = 0.075f;
    public Vector3 offset;

    public Transform lowerLeftBound;
    public Transform upperRightBound;


	// Use this for initialization
	void Start () {
		
	}

    Vector3 Vector3Clamp(Vector3 input, Vector3 min, Vector3 max)
    {
        float x = Mathf.Clamp(input.x, min.x, max.x);
        float y = Mathf.Clamp(input.y, min.y, max.y);
        float z = Mathf.Clamp(input.z, min.z, max.z);

        return new Vector3(x, y, z);
    }
	
	// Update is called once per frame
    // Changed to update to fix camera jitter
	void Update () {
        Vector3 desiredPosition = Vector3.zero;
        if (lowerLeftBound == null || upperRightBound == null)
        {
            desiredPosition = target.position + offset;
        }
        else
        {
            desiredPosition = Vector3Clamp(target.position, lowerLeftBound.position, upperRightBound.position) + offset;
        }
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime / 0.02f);
        transform.position = smoothPosition;		
	}
}
