using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ParallaxObject {
    public GameObject background;
    public Vector3 cameraStartPosition { get; private set; }
    public float followAmount;


    Vector3 startPosition;

    public void Start(Vector3 startPos)
    {

        cameraStartPosition = startPos;
        startPosition = background.transform.position;
    }

    public void UpdatePosition(Vector3 cameraPos)
    {
        Vector3 dCameraPos = cameraPos - cameraStartPosition;
        Vector3 targetPos = startPosition + (dCameraPos * (1f - followAmount));
        background.transform.position = targetPos;
    }
}

public class Parallax : MonoBehaviour {

    public ParallaxObject[] objects;


    void Start () {

        foreach (ParallaxObject obj in objects)
        {
            obj.Start(transform.position);
        }
	}
	
	void Update () {
        foreach (ParallaxObject obj in objects)
        {
            obj.UpdatePosition(transform.position);
        }
	}
}
