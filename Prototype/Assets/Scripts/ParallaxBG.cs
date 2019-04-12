using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    private float length, startPosX, startPosY;
    public GameObject Cam;
    public float parallaxAmount;

    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (Cam.transform.position.x * (1 - parallaxAmount));
        float distX = (Cam.transform.position.x * parallaxAmount);
        float distY = (Cam.transform.position.y * parallaxAmount);

        transform.position = new Vector3(startPosX + distX, transform.position.y, transform.position.z);
        if (temp > startPosX + length) startPosX += length;
        else if (temp < startPosX - length) startPosX -= length;
    }
}
