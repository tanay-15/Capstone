using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRing : MonoBehaviour {
    Vector3 baseScale;
    SpriteRenderer spr;
    Color newColor;
    float time = 0f;
	void Start () {
        baseScale = transform.localScale;
        spr = GetComponent<SpriteRenderer>();
        newColor = Color.white;
        transform.localScale = Vector3.zero;
	}
	
	void Update () {
        time += Time.deltaTime * 3f;
        transform.localScale = baseScale * time;
        newColor.a = (1f - time);
        spr.color = newColor;
        if (time > 1f)
            Destroy(gameObject);
	}
}
