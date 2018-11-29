using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientScroll : MonoBehaviour {

    public Image gradient1;
    public Image gradient2;
    public Transform startPos;
    public Transform endPos;
    bool gradientEnabled = true;
    float count = 0f;
	void Start () {
        EnableGradient(false);
	}

    public void EnableGradient(bool enabled)
    {
        if (gradientEnabled != enabled)
        {
            gradientEnabled = enabled;
            gradient1.enabled = enabled;
            gradient2.enabled = enabled;
        }
    }
	
	void Update () {
        count += Time.deltaTime;
        count %= 1f;
        transform.position = Vector3.Lerp(startPos.position, endPos.position, count);
	}
}
