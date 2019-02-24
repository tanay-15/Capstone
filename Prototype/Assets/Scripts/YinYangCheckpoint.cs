using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YinYangCheckpoint : MonoBehaviour
{
    public static int checkpointIndex;
    public GameObject flashPrefab;
    public int index;
    public GameObject sprite;
    public GameObject red;
    static event Action OnPlayerContactEvent;

    static YinYangCheckpoint()
    {
        checkpointIndex = -1;
    }

    void Start()
    {
        red.SetActive(checkpointIndex >= index);
    }

    void OnEnable()
    {
        OnPlayerContactEvent += OnPlayerContact;
    }

    void OnDisable()
    {
        OnPlayerContactEvent -= OnPlayerContact;
    }

    void OnPlayerContact()
    {
        red.SetActive(checkpointIndex >= index);
    }

    float Evaluate(float x)
    {
        return (1 - Mathf.Pow(x - 1, 2));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && checkpointIndex < index)
        {
            checkpointIndex = index;
            col.gameObject.SendMessage("SetRespawnPosition", transform.position);
            StartCoroutine(Spin());
            StartCoroutine(CreateFlash());
            OnPlayerContactEvent();
        }
    }

    IEnumerator CreateFlash()
    {
        GameObject flash = Instantiate(flashPrefab, transform.position, transform.rotation);
        SpriteRenderer sr = flash.GetComponent<SpriteRenderer>();
        Color col = Color.white;
        float scale = 0.1f;

        for (float i = Time.deltaTime; i < 1f; i += Time.deltaTime * 4f)
        {
            scale = 0.1f + (i * 0.3f);
            col.a = 1f - i;
            sr.color = col;
            flash.transform.localScale = Vector3.one * scale;
            yield return 0;
        }

        Destroy(flash);
    }

    IEnumerator Spin()
    {
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            sprite.transform.rotation = Quaternion.Euler(0f, 0f, Evaluate(i) * (-360f));
            yield return 0;
        }
        sprite.transform.rotation = Quaternion.identity;
    }
}
