using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationIcon : MonoBehaviour
{
    int count = 0;
    float timeCount;
    public float speed;
    public float amplitude;
    public RectTransform scalingSprites;
    public GameObject counterObjects;
    public Text text;
    IEnumerator routine;

    void Start()
    {
        timeCount = 0f;
        routine = null;

        SetCounter(count);
    }

    public void SetCounter(int amount)
    {
        count = amount;
        UpdateCounter();
    }

    public void AddCounter(int amount)
    {
        count += amount;
        UpdateCounter();

        if (routine != null)
            StopCoroutine(routine);
        routine = Shake();
        StartCoroutine(Shake());
    }

    void UpdateCounter()
    {
        scalingSprites.gameObject.SetActive(count > 0);
        counterObjects.SetActive(count > 0);
        text.text = count.ToString();
        timeCount = (count > 0) ? timeCount : 0;
    }

    void OnEnable()
    {
        timeCount = 0f;
    }

    IEnumerator Shake()
    {
        if (enabled && count > 0)
        {
            for (float i = 1f; i > 0f; i-= Time.deltaTime * 0.5f)
            {
                timeCount += Time.deltaTime * speed;
                if (timeCount > 360f) timeCount -= 360f;
                scalingSprites.localScale = Vector3.one * (1f + amplitude * i * Mathf.Cos(timeCount));
                yield return 0;
            }
            scalingSprites.localScale = Vector3.one;
        }
    }

    void Update()
    {
        //if (enabled && count > 0)
        //{
        //    timeCount += Time.deltaTime * speed;
        //    if (timeCount > 360f) timeCount -= 360f;
        //    scalingSprites.localScale = Vector3.one * (1f + amplitude * Mathf.Cos(timeCount));
        //}
    }
}
