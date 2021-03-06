﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayColorSwitch : MonoBehaviour
{
    Material raysMat;
    // Start is called before the first frame update
    void Start()
    {
        raysMat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!FindObjectOfType<DemonTransformScript>().DemonModeActive)
        {
            raysMat.SetColor("_Color1", new Color32(1, 243, 255, 255));
            raysMat.SetColor("_Color2", new Color32(0, 241, 248, 0));
        }


        else if (FindObjectOfType<DemonTransformScript>().DemonModeActive)
        {
            raysMat.SetColor("_Color1", new Color32(243, 54, 0, 255));
            raysMat.SetColor("_Color2", new Color32(248, 145, 0, 0));
        }
    }

    void OnBecameVisible()
    {
        this.gameObject.GetComponent<RayColorSwitch>().enabled = true;

    }
    void OnBecameInvisible()
    {
        this.gameObject.GetComponent<RayColorSwitch>().enabled = false;
    }
}
