using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPickup : MonoBehaviour
{
    float count;
    float scale;
    void Start()
    {
        count = 0f;
    }

    void Update()
    {
        count += Time.deltaTime;
        scale = 1f + (0.3f * Mathf.Sin(count * 4f));
        transform.localScale = Vector3.one * scale;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
        ArrowCounter.sharedInstance.SetMaxArrows();
    }
}
