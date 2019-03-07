using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagChanger : MonoBehaviour
{
    public string tag;
    public GameObject obj;
    public bool onDestroy;

    public void ChangeTag()
    {
        obj.tag = tag;
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        if (onDestroy)
            ChangeTag();
    }
}
