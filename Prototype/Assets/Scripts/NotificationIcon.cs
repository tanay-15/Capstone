using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationIcon : MonoBehaviour
{
    int count;
    float timeCount;
    public float speed;
    public float amplitude;
    public RectTransform scalingSprites;
    public GameObject counterObjects;
    public Text text;

    void Start()
    {
        count = 0;
        timeCount = 0f;

        SetCounter(0);
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
    }

    void UpdateCounter()
    {
        scalingSprites.gameObject.SetActive(count > 0);
        counterObjects.SetActive(count > 0);
        text.text = count.ToString();
        timeCount = (count > 0) ? timeCount : 0;
        Debug.Log(count > 0);
    }

    void OnEnable()
    {
        timeCount = 0f;
    }

    void Update()
    {
        if (enabled && count > 0)
        {
            timeCount += Time.deltaTime * speed;
            if (timeCount > 360f) timeCount -= 360f;
            scalingSprites.localScale = Vector3.one * (1f + amplitude * Mathf.Cos(timeCount));
        }
    }
}
