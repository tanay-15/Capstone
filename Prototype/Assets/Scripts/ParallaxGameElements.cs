using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxGameElements : MonoBehaviour
{
    private float startPosX, startPosY;
    public GameObject Cam;
    public float parallaxAmountX;
    public float parallaxAmountY;
    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (Cam.transform.position.x * (1 - parallaxAmountX));
        float distX = (Cam.transform.position.x * parallaxAmountX);
        float distY = (Cam.transform.position.y * parallaxAmountY);

        transform.position = new Vector3(startPosX + distX, startPosY + distY /*transform.position.y*/, transform.position.z);
    }
}
