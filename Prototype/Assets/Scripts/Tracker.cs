using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tracker : MonoBehaviour
{
    public GameObject obj;
    public Text text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = obj.transform.position.ToString();
    }
}
