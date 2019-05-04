using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateOnCanvas : MonoBehaviour
{
    float count;
    public float rotationSpeed;
    public RectTransform image;

    void Start()
    {
        gameObject.SetActive(false);
        count = 0f;
    }

    void OnEnable()
    {
        count = 0f;
    }

    void Update()
    {
        if (enabled)
        {
            count += Time.deltaTime * rotationSpeed;
            if (count > 360f) count -= 360f;
            image.rotation = Quaternion.Euler(0f, 0f, count);
        }
    }
}
