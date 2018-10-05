using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    public float speed = 0.075f;
    public Vector3 offset;

    public Transform lowerLeftBound;
    public Transform upperRightBound;

    float panSize = 1;
    float currentSize = 1;
    float fracLerp = 0;
    float panSpeed = 1;
    bool isPanning = false;

    Vector3 rawTransformPos;
    Vector3 shakeOffset;
    IEnumerator shakeRoutine;

	// Use this for initialization
	void Start () {
        shakeOffset = Vector3.zero;
        rawTransformPos = transform.position;
        shakeRoutine = null;
	}

    Vector3 Vector3Clamp(Vector3 input, Vector3 min, Vector3 max)
    {
        float x = Mathf.Clamp(input.x, min.x, max.x);
        float y = Mathf.Clamp(input.y, min.y, max.y);
        float z = Mathf.Clamp(input.z, min.z, max.z);

        return new Vector3(x, y, z);
    }

    IEnumerator ShakeRoutine()
    {
        float time = 0.2f;
        float ampl = 0.15f;
        for (float i = time; time > 0; time -= Time.deltaTime)
        {
            shakeOffset = Random.insideUnitCircle * (time / 0.2f) * ampl;
            yield return 0;
        }
        shakeOffset = Vector3.zero;
    }

    public void ShakeCamera()
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);
        shakeRoutine = ShakeRoutine();
        StartCoroutine(shakeRoutine);
    }
	
	// Update is called once per frame
    // Changed to update to fix camera jitter
	void Update () {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ShakeCamera();
        }

        Vector3 desiredPosition = Vector3.zero;
        if (lowerLeftBound == null || upperRightBound == null)
        {
            desiredPosition = target.position + offset;
        }
        else
        {
            desiredPosition = Vector3Clamp(target.position, lowerLeftBound.position, upperRightBound.position) + offset;
        }
        Vector3 smoothPosition = Vector3.Lerp(rawTransformPos, desiredPosition, speed * Time.deltaTime / 0.02f);
        rawTransformPos = smoothPosition;
        transform.position = rawTransformPos + shakeOffset;

        if (isPanning)
        {
            fracLerp += panSpeed * Time.deltaTime;
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(currentSize, panSize, fracLerp);

            if (fracLerp >= 1)
            {
                isPanning = false;
                fracLerp = 0;
            }
        }
	}


    public void CameraPan(float Size,float PanSpeed)
    {
        currentSize = GetComponent<Camera>().orthographicSize;
        panSize = Size;
        panSpeed = PanSpeed;
        isPanning = true;
    }
}
